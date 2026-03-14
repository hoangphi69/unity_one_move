using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitlePanel : MonoBehaviour
{
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

  void continueClicked() => GameSceneManager.Instance.OnTitleContinue();
  void newGameClicked() => GameSceneManager.Instance.OnTitleNewGame();
  void optionsClicked() => TitleScreenManager.Instance.OpenOptions();
  void savesClicked() => TitleScreenManager.Instance.OpenSaves();
  void quitClicked() => Application.Quit();
}