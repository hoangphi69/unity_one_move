using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmOverlay : MonoBehaviour
{
  [Header("Visual")]
  [SerializeField] private TextMeshProUGUI title;
  [SerializeField] private TextMeshProUGUI message;
  [SerializeField] private Button confirmButton;
  [SerializeField] private Button cancelButton;

  public void Show(string title, string message, Action onConfirm)
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

    cancelButton.onClick.AddListener(Hide);

    gameObject.SetActive(true);
  }

  public void Hide()
  {
    gameObject.SetActive(false);
  }
}