using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum InputState
{
  World,
  UI,
}

public class InputActionsManager : MonoBehaviour
{
  public static InputActionsManager Instance { get; private set; }

  public InputActions inputActions;
  public InputState CurrentState { get; private set; }
  public event Func<bool> OnUIBackRequested;

  void Awake()
  {
    Instance = this;
    inputActions = new();
  }

  void OnEnable()
  {
    inputActions.Player.Escape.performed += OnEscapeTriggered;
    inputActions.UI.Escape.performed += OnEscapeTriggered;

    // Always start with UI (Title Screen)
    SetState(InputState.UI);
  }

  void OnDisable()
  {
    inputActions.Player.Escape.performed -= OnEscapeTriggered;
    inputActions.UI.Escape.performed -= OnEscapeTriggered;
    inputActions.Disable();
  }

  public void SetState(InputState newState)
  {
    CurrentState = newState;

    inputActions.Player.Disable();
    inputActions.UI.Disable();

    switch (newState)
    {
      case InputState.World:
        inputActions.Player.Enable();
        break;
      case InputState.UI:
        inputActions.UI.Enable();
        break;
    }

    print($"Input State: {CurrentState}");
  }

  private void OnEscapeTriggered(InputAction.CallbackContext context)
  {
    switch (CurrentState)
    {
      case InputState.World:
        GameSceneManager.Instance.TogglePause();
        break;
      case InputState.UI:
        bool backRequested = OnUIBackRequested?.Invoke() ?? false;
        if (!backRequested)
        {
          print("escape");
          GameSceneManager.Instance.TogglePause();
        }
        break;
    }
  }
}