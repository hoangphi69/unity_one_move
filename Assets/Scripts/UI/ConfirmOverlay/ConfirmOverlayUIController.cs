using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ConfirmOverlayUIController : MonoBehaviour
{
  public static ConfirmOverlayUIController Instance { get; private set; }

  [Header("Visual")]
  [SerializeField] private RectTransform backgroundPanel;
  [SerializeField] private TextMeshProUGUI title;
  [SerializeField] private TextMeshProUGUI message;
  [SerializeField] private Button confirmButton;
  [SerializeField] private Button cancelButton;

  private Action currentOnCancel;

  [Header("Animation Settings")]
  [SerializeField] private float duration = 0.15f;
  [SerializeField] private Ease showEase = Ease.OutSine;
  [SerializeField] private Ease hideEase = Ease.InSine;

  private CanvasGroup confirmCanvasGroup;
  private CanvasGroup cancelCanvasGroup;
  private Sequence currentAnimation;

  private void Awake()
  {
    if (Instance == null) Instance = this;

    confirmCanvasGroup = confirmButton.GetComponent<CanvasGroup>();
    cancelCanvasGroup = cancelButton.GetComponent<CanvasGroup>();

    gameObject.SetActive(false);
  }

  public void Show(string title, string message, Action onConfirm, Action onCancel = null)
  {
    this.message.text = message;
    this.title.text = title;

    confirmButton.onClick.RemoveAllListeners();
    confirmButton.onClick.AddListener(() => { onConfirm?.Invoke(); Hide(); });

    cancelButton.onClick.RemoveAllListeners();
    cancelButton.onClick.AddListener(() => { onCancel?.Invoke(); Hide(); });
    currentOnCancel = onCancel;

    GameInputManager.Instance.Actions.UI.Escape.performed -= HandleEscape;
    GameInputManager.Instance.Actions.UI.Escape.performed += HandleEscape;

    gameObject.SetActive(true);

    // Animation
    currentAnimation?.Kill();

    this.title.alpha = 0f;
    this.message.alpha = 0f;
    confirmCanvasGroup.alpha = 0;
    cancelCanvasGroup.alpha = 0;

    // Build the DOTween Sequence
    currentAnimation = DOTween.Sequence();
    currentAnimation.SetUpdate(true);

    currentAnimation
      .Join(backgroundPanel.DOScaleY(1, duration).From(0).SetEase(showEase))
      .Insert(duration / 2, confirmCanvasGroup.DOFade(1, duration).SetEase(showEase))
      .Join(cancelCanvasGroup.DOFade(1, duration).SetEase(showEase))
      .Join(this.title.DOFade(1f, duration))
      .Join(this.message.DOFade(1f, duration));
  }

  public void Hide()
  {
    if (!gameObject.activeInHierarchy) return;

    currentAnimation?.Kill();

    currentAnimation = DOTween.Sequence();
    currentAnimation.SetUpdate(true);

    currentAnimation
      .Join(title.DOFade(0f, duration))
      .Join(message.DOFade(0f, duration))
      .Join(confirmCanvasGroup.DOFade(0, duration).SetEase(hideEase))
      .Join(cancelCanvasGroup.DOFade(0, duration).SetEase(hideEase))
      .Insert(duration / 2, backgroundPanel.DOScaleY(0, duration).SetEase(hideEase))
      .OnComplete(() => gameObject.SetActive(false)); // Turn off the object only after the animation finishes
  }

  void HandleEscape(InputAction.CallbackContext context)
  {
    if (currentAnimation.active) return;
    currentOnCancel?.Invoke();
    Hide();
  }
}