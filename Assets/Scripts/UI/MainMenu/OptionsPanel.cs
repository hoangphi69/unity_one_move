using UnityEngine;
using UnityEngine.UI;

public class OptionsPanel : MonoBehaviour
{
  [Header("Buttons")]
  [SerializeField] private Button backButton;

  void OnEnable()
  {
    backButton.onClick.AddListener(backClicked);
  }

  void OnDisable()
  {
    backButton.onClick.RemoveListener(backClicked);
  }

  void backClicked()
  {
    TitleScreenManager.Instance.CloseCurrentPanel();
  }
}