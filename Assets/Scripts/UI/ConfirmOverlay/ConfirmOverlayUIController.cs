using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmOverlayUIController : MonoBehaviour
{
  public static ConfirmOverlayUIController Instance { get; private set; }

  [Header("Visual")]
  [SerializeField] private TextMeshProUGUI title;
  [SerializeField] private TextMeshProUGUI message;
  [SerializeField] private Button confirmButton;
  [SerializeField] private Button cancelButton;

  private void Awake()
  {
    if (Instance != null && Instance != this)
    {
      Destroy(gameObject);
      return;
    }
    Instance = this;

    Hide();
  }

  public void Show(string title, string message, Action onConfirm, Action onCancel = null)
  {
    this.message.text = message;
    this.title.text = title;

    confirmButton.onClick.RemoveAllListeners();
    cancelButton.onClick.RemoveAllListeners();

    confirmButton.onClick.AddListener(() =>
    {
      onConfirm?.Invoke();
      Hide();
    });

    cancelButton.onClick.AddListener(() =>
    {
      onCancel?.Invoke();
      Hide();
    });

    gameObject.SetActive(true);
  }

  public void Hide()
  {
    gameObject.SetActive(false);
  }
}