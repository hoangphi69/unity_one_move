using UnityEngine;
using UnityEngine.UI;

public class PausePanel : NavigationPanel
{
  [Header("Buttons")]
  [SerializeField] private Button restartButton;
  [SerializeField] private Button resumeButton;
  [SerializeField] private Button optionsButton;
  [SerializeField] private Button saveButton;
  [SerializeField] private Button returnTitleButton;
  [SerializeField] private Button backButton;

  void OnEnable()
  {
    restartButton.onClick.AddListener(OnRestartClicked);
    resumeButton.onClick.AddListener(OnResumeClicked);
    backButton.onClick.AddListener(OnResumeClicked);
    optionsButton.onClick.AddListener(OnOptionsClicked);
    returnTitleButton.onClick.AddListener(OnReturnTitleClicked);

    restartButton.gameObject.SetActive(GameplayManager.Instance.isPuzzleStage());
  }

  void OnDisable()
  {
    restartButton.onClick.RemoveListener(OnRestartClicked);
    resumeButton.onClick.RemoveListener(OnResumeClicked);
    backButton.onClick.RemoveListener(OnResumeClicked);
    optionsButton.onClick.RemoveListener(OnOptionsClicked);
    returnTitleButton.onClick.RemoveListener(OnReturnTitleClicked);
  }

  void OnRestartClicked()
  {
    CloseAllPanels();
    GameEventsManager.Instance.turnEvents.RestartStage();
  }

  void OnResumeClicked()
  {
    CloseAllPanels();
    GameEventsManager.Instance.flowEvents.ContinueGame();
  }

  void OnOptionsClicked()
  {
    Navigate(PauseScreenRoutes.OPTIONS);
  }

  void OnReturnTitleClicked()
  {
    ConfirmOverlayUIController.Instance.Show(
      "Quit to Menu",
      "Are you sure you want to quit to the main menu? Unsaved progress will be lost.",
      onConfirm: () =>
      {
        CloseAllPanels();
        GameEventsManager.Instance.flowEvents.ReturnToTitle();
      }
    );
  }
}