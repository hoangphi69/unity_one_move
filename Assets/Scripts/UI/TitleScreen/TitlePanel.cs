using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitlePanel : NavigationPanel
{
  [Header("Visual")]
  [SerializeField] private CanvasGroup buttons;
  [SerializeField] private CanvasGroup title;
  [SerializeField] private RectTransform background;
  [SerializeField] private float duration = 0.5f;
  [SerializeField] private float zoomScale = 2f;

  [Header("Buttons")]
  [SerializeField] private Button continueButton;
  [SerializeField] private Button newGameButton;
  [SerializeField] private Button optionsButton;
  [SerializeField] private Button savesButton;
  [SerializeField] private Button quitButton;

  void OnEnable()
  {
    continueButton.onClick.AddListener(continueClicked);
    newGameButton.onClick.AddListener(newGameClicked);
    optionsButton.onClick.AddListener(optionsClicked);
    savesButton.onClick.AddListener(savesClicked);
    quitButton.onClick.AddListener(quitClicked);

    continueButton.gameObject.SetActive(GameDataManager.Instance.HasData());
    savesButton.GetComponentInParent<TextMeshProUGUI>().text = $"Save [{GameDataManager.Instance.selectedProfileID}]";
  }

  void OnDisable()
  {
    continueButton.onClick.RemoveListener(continueClicked);
    newGameButton.onClick.RemoveListener(newGameClicked);
    optionsButton.onClick.RemoveListener(optionsClicked);
    savesButton.onClick.RemoveListener(savesClicked);
    quitButton.onClick.RemoveListener(quitClicked);
  }

  public override void Show()
  {
    base.Show();

    // Kill any active tweens on these objects to prevent glitches if toggled rapidly
    DOTween.Kill(title);
    DOTween.Kill(buttons);
    DOTween.Kill(background);

    // Reset the layout to default state
    title.alpha = 1f;
    buttons.alpha = 1f;
    buttons.interactable = true; // Re-enable interaction
    background.localScale = Vector3.one;
  }

  async Task EnterGameplay()
  {
    buttons.interactable = false;

    title.DOFade(0f, .2f);
    buttons.DOFade(0f, .2f);

    GameplayManager.Instance.ZoomCamera(true);

    await background.DOScale(Vector3.one * zoomScale, duration)
                    .SetEase(Ease.InOutQuad)
                    .AsyncWaitForCompletion();

    CloseAllPanels();
  }

  async void continueClicked()
  {
    await EnterGameplay();
    GameEventsManager.Instance.flowEvents.ContinueGame();
  }

  void newGameClicked()
  {
    async void StartNewGame()
    {
      await EnterGameplay();
      GameEventsManager.Instance.flowEvents.NewGame();
    }

    if (GameDataManager.Instance.HasData())
      ConfirmOverlayUIController.Instance.Show(
        "New Game",
        "Are you sure you want to start a new game on this save file? This will <color=#D57B19>erase</color> all of your previous progress.",
        onConfirm: StartNewGame
      );
    else StartNewGame();
  }

  void optionsClicked() => Navigate(TitleScreenRoutes.OPTIONS);

  void savesClicked() => Navigate(TitleScreenRoutes.SAVES);

  void quitClicked()
  {
    ConfirmOverlayUIController.Instance.Show(
      "Exit game?",
      "Are you sure you want to exit the game?",
      onConfirm: Application.Quit
    );
  }
}