using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public enum InputState { Gameplay, UI }

public class GameInputManager : MonoBehaviour
{
  public static GameInputManager Instance { get; private set; }

  public InputActions Actions { get; private set; }
  private InputState state;
  private Dictionary<InputState, InputActionMap> _maps;

  void Awake()
  {
    Instance = this;
    Actions = new();
    _maps = new()
      {
        { InputState.Gameplay, Actions.Player },
        { InputState.UI, Actions.UI },
      };
  }

  void OnDisable() => Actions.Disable();

  public void SetState(InputState newState)
  {
    state = newState;
    foreach (var map in _maps.Values) map.Disable();
    _maps[newState].Enable();
    print($"Input: {state}");
  }
}