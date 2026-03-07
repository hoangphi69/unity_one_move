using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public enum InputState { Gameplay, UI }

public class InputActionsManager : MonoBehaviour
{
  public static InputActionsManager Instance { get; private set; }
  public InputActions inputActions;
  public InputState CurrentState { get; private set; }

  private Dictionary<InputState, InputActionMap> _maps;

  void Awake()
  {
    Instance = this;
    inputActions = new();
    _maps = new()
      {
        { InputState.Gameplay, inputActions.Player },
        { InputState.UI, inputActions.UI },
      };
  }

  void OnDisable() => inputActions.Disable();

  public void SetState(InputState newState)
  {
    CurrentState = newState;
    foreach (var map in _maps.Values) map.Disable();
    _maps[newState].Enable();
    print($"Input: {CurrentState}");
  }
}