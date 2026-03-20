using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseScreenUIController : MonoBehaviour
{

  [SerializeField] private GameObject uiContainer;
  [SerializeField] private Button restartButton;
  [SerializeField] private Button resumeButton;
  [SerializeField] private Button optionsButton;
  [SerializeField] private Button saveButton;
  [SerializeField] private Button returnTitleButton;
  [SerializeField] private Button backButton;

  void OnEnable()
  {
    GameEventsManager.Instance.flowEvents.onGamePaused += Show;
    restartButton.onClick.AddListener(onRestartClicked);
    resumeButton.onClick.AddListener(onResumeClicked);
    backButton.onClick.AddListener(onResumeClicked);
    returnTitleButton.onClick.AddListener(onReturnTitleClicked);
  }

  void OnDisable()
  {
    GameEventsManager.Instance.flowEvents.onGamePaused -= Show;
    restartButton.onClick.RemoveAllListeners();
    resumeButton.onClick.RemoveAllListeners();
    returnTitleButton.onClick.RemoveAllListeners();
    backButton.onClick.RemoveAllListeners();
  }

  void Start() => Hide();

  void Show()
  {
    uiContainer.SetActive(true);
    Time.timeScale = 0f;
    GameInputManager.Instance.Actions.UI.Escape.performed += HandleEscape;
    restartButton.gameObject.SetActive(GameplayManager.Instance.isPuzzleStage());
  }

  void Hide()
  {
    uiContainer.SetActive(false);
    Time.timeScale = 1f;
    GameInputManager.Instance.Actions.UI.Escape.performed -= HandleEscape;
  }

  void HandleEscape(InputAction.CallbackContext context)
  {
    Hide();
    GameEventsManager.Instance.flowEvents.ContinueGame();
  }

  void onRestartClicked()
  {
    Hide();
    GameEventsManager.Instance.turnEvents.RestartStage();
  }

  void onResumeClicked()
  {
    Hide();
    GameEventsManager.Instance.flowEvents.ContinueGame();
  }

  void onReturnTitleClicked()
  {
    Hide();
    GameEventsManager.Instance.flowEvents.ReturnToTitle();
  }
}