using UnityEngine;
using UnityEngine.InputSystem;

public static class PauseScreenRoutes
{
  public const string PAUSE = "pause";
  public const string OPTIONS = "options";
  public const string SAVE = "save";
}

public class PauseScreenUIController : NavigationUIController
{
  [Header("Panels")]
  [SerializeField] private NavigationPanel pausePanel;
  [SerializeField] private NavigationPanel optionsPanel;
  [SerializeField] private NavigationPanel savePanel;

  void Awake()
  {
    RegisterPanel(PauseScreenRoutes.PAUSE, pausePanel);
    RegisterPanel(PauseScreenRoutes.OPTIONS, optionsPanel);
    RegisterPanel(PauseScreenRoutes.SAVE, savePanel);
  }

  void OnDestroy() => UnregisterAllPanels();

  void OnEnable()
  {
    GameEventsManager.Instance.flowEvents.onGamePaused += Show;
  }

  void OnDisable()
  {
    GameEventsManager.Instance.flowEvents.onGamePaused -= Show;
  }

  void Start() => Hide(); // Ensure it starts hidden

  public override void Show()
  {
    base.Show();
    ClearStack();
    NavigateToPanel(PauseScreenRoutes.PAUSE);

    // Pause time and start listening for UI escape presses
    Time.timeScale = 0f;
    GameInputManager.Instance.Actions.UI.Escape.performed += HandleEscape;
  }

  public override void Hide()
  {
    base.Hide();
    ClearStack();

    // Resume time and stop listening for UI escape presses
    Time.timeScale = 1f;
    GameInputManager.Instance.Actions.UI.Escape.performed -= HandleEscape;
  }

  private void HandleEscape(InputAction.CallbackContext context)
  {
    // If we are deep in a menu (like Options), just go back one screen
    if (menuStack.Count > 1)
    {
      CloseCurrentPanel();
    }
    else
    {
      // If we are at the root (the main Pause panel), unpause the game
      CloseEntireUI();
      GameEventsManager.Instance.flowEvents.ContinueGame();
    }
  }
}