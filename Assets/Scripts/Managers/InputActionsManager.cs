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

  // Reference to your generated Input Action class
  public InputActions inputActions;

  public InputState CurrentState { get; private set; }

  void Awake()
  {
    Instance = this;
    inputActions = new();
  }

  void OnEnable()
  {
    // Centralized Escape/Back Handler
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

    // Reset all first to ensure clean slate
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

    // Debug.Log($"Input State Switched to: {newState}");
  }

  private void OnEscapeTriggered(InputAction.CallbackContext context)
  {
    // Route the Escape key based on context
    switch (CurrentState)
    {
      case InputState.World:
        GameSceneManager.Instance.TogglePause();
        break;
      case InputState.UI:
        GameSceneManager.Instance.TogglePause();
        break;
    }
  }
}