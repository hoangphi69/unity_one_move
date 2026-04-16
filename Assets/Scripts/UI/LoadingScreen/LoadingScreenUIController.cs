using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LoadingScreenUIController : MonoBehaviour
{
  public static LoadingScreenUIController Instance { get; private set; }

  [Header("UI References")]
  [SerializeField] private RectTransform uiContainer;
  [SerializeField] private CanvasGroup loadingText;

  [Header("Strips Configuration")]
  [SerializeField] private Color stripColor = Color.black;
  [SerializeField] private int numberOfStrips = 6;

  [Tooltip("The total time it will take for ALL strips to finish moving")]
  [SerializeField] private float duration = 0.2f;
  [SerializeField] private float delay = 0.05f;
  [SerializeField] private Ease stripEase = Ease.OutCubic;

  private RectTransform[] generatedStrips;
  private CanvasGroup background;
  private float screenHeight;

  void Awake()
  {
    if (Instance == null) Instance = this;

    GenerateStrips();
    uiContainer.gameObject.SetActive(false);
  }

  public bool isActive() => uiContainer.gameObject.activeSelf;

  private void GenerateStrips()
  {
    generatedStrips = new RectTransform[numberOfStrips];

    GameObject stripsParentObj = new GameObject("StripsContainer", typeof(RectTransform), typeof(CanvasGroup));
    RectTransform stripsParent = stripsParentObj.GetComponent<RectTransform>();
    stripsParent.SetParent(uiContainer, false);
    stripsParent.SetAsFirstSibling();

    background = stripsParentObj.GetComponent<CanvasGroup>();

    stripsParent.anchorMin = Vector2.zero;
    stripsParent.anchorMax = Vector2.one;
    stripsParent.offsetMin = Vector2.zero;
    stripsParent.offsetMax = Vector2.zero;

    for (int i = 0; i < numberOfStrips; i++)
    {
      GameObject stripObj = new GameObject($"Strip_{i}", typeof(RectTransform), typeof(Image));
      RectTransform stripRect = stripObj.GetComponent<RectTransform>();
      stripRect.SetParent(stripsParent, false);

      Image img = stripObj.GetComponent<Image>();
      img.color = stripColor;

      float minX = (float)i / numberOfStrips;
      float maxX = (float)(i + 1) / numberOfStrips;

      stripRect.anchorMin = new Vector2(minX, 0);
      stripRect.anchorMax = new Vector2(maxX, 1);
      stripRect.offsetMin = Vector2.zero;
      stripRect.offsetMax = Vector2.zero;

      generatedStrips[i] = stripRect;
    }
  }

  // ==========================================
  // --- IMMEDIATE METHODS ---
  // ==========================================

  public void ShowImmediate()
  {
    DOTween.Kill(background);
    DOTween.Kill(loadingText);

    uiContainer.gameObject.SetActive(true);
    background.alpha = 1f;
    loadingText.alpha = 1f;

    foreach (var strip in generatedStrips)
    {
      DOTween.Kill(strip);
      strip.anchoredPosition = Vector2.zero;
    }
  }

  public void HideImmediate()
  {
    uiContainer.gameObject.SetActive(false);
  }

  // ==========================================
  // --- FADE METHODS ---
  // ==========================================

  public async Task ShowFadeAsync(float duration = 0.5f)
  {
    DOTween.Kill(background);
    DOTween.Kill(loadingText);

    uiContainer.gameObject.SetActive(true);

    // Put strips in center to cover the screen
    foreach (var strip in generatedStrips)
    {
      DOTween.Kill(strip);
      strip.anchoredPosition = Vector2.zero;
    }

    // Fade the strips collective container in
    background.alpha = 0f;
    loadingText.alpha = 0f;

    await background.DOFade(1f, duration).SetUpdate(true).AsyncWaitForCompletion();

    // Fire and forget text
    loadingText.DOFade(1f, 0.3f).SetDelay(1.0f).SetUpdate(true);
  }

  public async Task HideFadeAsync(float duration = 0.5f)
  {
    DOTween.Kill(loadingText);
    if (loadingText.alpha > 0f)
    {
      await loadingText.DOFade(0f, 0.2f).SetUpdate(true).AsyncWaitForCompletion();
    }

    // Fade the strips collective container out
    await background.DOFade(0f, duration).SetUpdate(true).AsyncWaitForCompletion();
    HideImmediate();
  }

  // ==========================================
  // --- STRIPS METHODS ---
  // ==========================================

  public async Task ShowStripsAsync()
  {
    HUDOverlayUIController.Instance.SetCounterOnTop(true);

    DOTween.Kill(background);
    DOTween.Kill(loadingText);

    uiContainer.gameObject.SetActive(true);

    // Ensure strips are visible (in case they were previously faded out)
    background.alpha = 1f;
    loadingText.alpha = 0f;

    screenHeight = uiContainer.rect.height;

    foreach (var strip in generatedStrips)
    {
      DOTween.Kill(strip);
      strip.anchoredPosition = new Vector2(0, -screenHeight);
    }

    List<Task> stripTasks = new List<Task>();

    for (int i = 0; i < generatedStrips.Length; i++)
    {
      float delay = (generatedStrips.Length - 1 - i) * this.delay;

      Task t = generatedStrips[i].DOAnchorPosY(0, duration)
                                 .SetDelay(delay)
                                 .SetEase(stripEase)
                                 .SetUpdate(true) // Ignore timeScale
                                 .AsyncWaitForCompletion();
      stripTasks.Add(t);
    }

    loadingText.DOFade(1f, 0.3f).SetDelay(1.0f).SetUpdate(true);

    await Task.WhenAll(stripTasks);
  }

  public async Task HideStripsAsync()
  {
    DOTween.Kill(loadingText);

    if (loadingText.alpha > 0f)
    {
      await loadingText.DOFade(0f, 0.2f).SetUpdate(true).AsyncWaitForCompletion();
    }

    screenHeight = uiContainer.rect.height;

    List<Task> stripTasks = new List<Task>();

    for (int i = 0; i < generatedStrips.Length; i++)
    {
      float delay = (generatedStrips.Length - 1 - i) * this.delay;

      Task t = generatedStrips[i].DOAnchorPosY(screenHeight, duration)
                                 .SetDelay(delay)
                                 .SetEase(stripEase)
                                 .SetUpdate(true) // Ignore timeScale
                                 .AsyncWaitForCompletion();
      stripTasks.Add(t);
    }

    await Task.WhenAll(stripTasks);
    HideImmediate();

    HUDOverlayUIController.Instance.SetCounterOnTop(false);
  }
}