using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;

public class CutsceneUIController : MonoBehaviour
{
  [Header("Main")]
  [SerializeField] private GameObject canvas;
  [SerializeField] private Image background;
  [SerializeField] private Image sprite;
  [SerializeField] private GameObject mainUIPanel;
  [SerializeField] private LogPanel logPanel;

  [Header("Bottom UI")]
  [SerializeField] private Button bottomUI;
  [SerializeField] private CanvasGroup bottomElements;
  [SerializeField] private TextMeshProUGUI dialogueLine;
  [SerializeField] private GameObject speakerBox;
  [SerializeField] private TextMeshProUGUI speaker;
  [SerializeField] private Image advanceIndicator;

  [Header("Middle UI")]
  [SerializeField] private Button middleUI;
  [SerializeField] private GameObject choicesOverlay;
  [SerializeField] private GameObject choicesContainer;
  [SerializeField] private GameObject choicePrefab;

  [Header("Top UI")]
  [SerializeField] private GameObject topUI;
  [SerializeField] private CanvasGroup topElements;
  [SerializeField] private Button showLogButton;
  [SerializeField] private Button hideLogButton;
  [SerializeField] private Button autoButton;
  [SerializeField] private Image autoIcon;
  [SerializeField] private Button hideUIButton;
  [SerializeField] private Button showUIPanel;
  [SerializeField] private Button skipButton;
  [SerializeField] private Image skipProgressRing;

  [Header("Config")]
  [SerializeField] private int typingSpeed = 50;
  [SerializeField] private float autoAdvanceDelay = 1.0f; // Seconds
  // State
  private bool isAutoMode = false;
  private CancellationTokenSource autoAdvanceCTS;
  private bool skipLine = false;
  private bool isOverlay = false;
  private CancellationTokenSource skipDialogueCTS;

  [Header("Animation")]
  [SerializeField] private float duration = 0.3f;
  [SerializeField] private Ease enterEase = Ease.OutCubic; // Smooth deceleration
  [SerializeField] private Ease exitEase = Ease.OutCubic;   // Smooth acceleration

  private Sequence sequence;

  void OnEnable()
  {
    GameEventsManager.Instance.dialogueEvents.onDialogueStarted += DialogueStart;
    GameEventsManager.Instance.dialogueEvents.onDialogueEnded += DialogueEnd;
    GameEventsManager.Instance.dialogueEvents.onDialogueDisplayed += DialogueDisplay;
    GameEventsManager.Instance.dialogueEvents.onRequestSkipLine += SkipLine;
  }

  void OnDisable()
  {
    GameEventsManager.Instance.dialogueEvents.onDialogueStarted -= DialogueStart;
    GameEventsManager.Instance.dialogueEvents.onDialogueEnded -= DialogueEnd;
    GameEventsManager.Instance.dialogueEvents.onDialogueDisplayed -= DialogueDisplay;
    GameEventsManager.Instance.dialogueEvents.onRequestSkipLine -= SkipLine;
  }

  // --- Auto Advance ---

  void ToggleAutoMode()
  {
    if (isAutoMode) DisableAutoMode();
    else EnableAutoMode();
  }

  void EnableAutoMode()
  {
    isAutoMode = true;
    autoIcon.sprite = Resources.Load<Sprite>("Sprites/Icon/auto_on");
    _ = StartAutoAdvanceTimer();
  }

  void DisableAutoMode()
  {
    isAutoMode = false;
    autoIcon.sprite = Resources.Load<Sprite>("Sprites/Icon/auto_off");
    CancelAutoAdvanceTask();
  }

  void CancelAutoAdvanceTask()
  {
    autoAdvanceCTS?.Cancel();
    autoAdvanceCTS?.Dispose();
    autoAdvanceCTS = null;
  }

  async Task StartAutoAdvanceTimer()
  {
    CancelAutoAdvanceTask();
    autoAdvanceCTS = new CancellationTokenSource();

    try
    {
      // Convert seconds to milliseconds for Task.Delay
      int delayMs = (int)(autoAdvanceDelay * 1000);
      await Task.Delay(delayMs, autoAdvanceCTS.Token);

      // Final check before advancing
      if (isAutoMode && !isOverlay) AutoAdvanceDialogue();
    }
    catch (OperationCanceledException) { }
  }

  // --- End Auto Advance ---

  // --- Skip animation ---
  void SkipDialogue(InputAction.CallbackContext _)
  {
    if (isOverlay) return;
    DialogueEnd();
  }

  void StartSkipHold(InputAction.CallbackContext context)
  {
    if (isOverlay) return;

    skipDialogueCTS?.Cancel();
    skipDialogueCTS = new CancellationTokenSource();
    var interaction = context.interaction as HoldInteraction;
    _ = UpdateSkipProgress(interaction.duration, skipDialogueCTS.Token);
  }

  void CancelSkipHold(InputAction.CallbackContext _)
  {
    skipDialogueCTS?.Cancel();
    skipProgressRing.fillAmount = 0;
  }

  async Task UpdateSkipProgress(float duration, CancellationToken token)
  {
    float elapsed = 0f;
    skipProgressRing.fillAmount = 0;

    while (elapsed < duration)
    {
      if (token.IsCancellationRequested) return;

      elapsed += Time.deltaTime;
      skipProgressRing.fillAmount = elapsed / duration;

      await Task.Yield(); // Wait for next frame
    }

    // Hold complete!
    skipProgressRing.fillAmount = 0;
    DialogueEnd();
  }
  // --- End skip animation ---

  void onShowLogClicked()
  {
    if (isOverlay) return;
    if (isAutoMode) DisableAutoMode(); // Turn off Auto when looking at history
    isOverlay = true;
    logPanel.Show();
  }

  void onHideLogClicked()
  {
    isOverlay = false;
    logPanel.Hide();
  }

  void onHideUIClicked()
  {
    if (isOverlay) return;
    if (isAutoMode) DisableAutoMode(); // Turn off Auto when hiding UI
    isOverlay = true;
    mainUIPanel.SetActive(false);
    showUIPanel.gameObject.SetActive(true);
  }

  void onShowUIClicked()
  {
    isOverlay = false;
    mainUIPanel.SetActive(true);
    showUIPanel.gameObject.SetActive(false);
  }

  void onSkipClicked()
  {
    ConfirmOverlayUIController.Instance.Show(
      "Skip cutscene",
      "Are you sure you want to skip this cutscene?",
      onConfirm: DialogueEnd
    );
  }

  async void DialogueStart(DialogueMode mode)
  {
    if (mode != DialogueMode.Cutscene) return;
    ClearDialogue();
    logPanel.ClearEntries();

    // 1. Instantly set starting states (invisible / scaled to 0) to prevent 1-frame flickering
    background.DOFade(0f, 0f);
    sprite.DOFade(0f, 0f);
    topElements.alpha = 0f;
    bottomElements.alpha = 0f;

    canvas.SetActive(true);

    // 2. Play Entrance Animation
    sequence?.Kill();
    sequence = DOTween.Sequence();
    sequence.SetUpdate(true); // Optional: Run on unscaled time if timeScale is paused

    // Scale panels and fade background/sprite at the same time
    await sequence
      .Join(topUI.GetComponent<RectTransform>().DOScaleY(1f, duration).From(0).SetEase(enterEase))
      .Join(bottomUI.GetComponent<RectTransform>().DOScaleY(1f, duration).From(0).SetEase(enterEase))
      .Insert(duration / 2, topElements.DOFade(1f, duration))
      .Join(bottomElements.DOFade(1f, duration))
      .Join(background.DOFade(1f, duration))
      .Join(sprite.DOFade(1f, duration))
      .AppendInterval(.5f)
      .AsyncWaitForCompletion();

    GameInputManager.Instance.Actions.UI.DialogueAdvance.performed += AdvanceDialogue;
    GameInputManager.Instance.Actions.UI.DialogueSkip.performed += SkipDialogue;
    GameInputManager.Instance.Actions.UI.DialogueSkip.started += StartSkipHold;
    GameInputManager.Instance.Actions.UI.DialogueSkip.canceled += CancelSkipHold;

    middleUI.onClick.AddListener(AdvanceDialogue);
    bottomUI.onClick.AddListener(AdvanceDialogue);
    showLogButton.onClick.AddListener(onShowLogClicked);
    hideLogButton.onClick.AddListener(onHideLogClicked);
    hideUIButton.onClick.AddListener(onHideUIClicked);
    showUIPanel.onClick.AddListener(onShowUIClicked);
    autoButton.onClick.AddListener(ToggleAutoMode);
    skipButton.onClick.AddListener(onSkipClicked);
  }

  void DialogueEnd()
  {
    GameEventsManager.Instance.dialogueEvents.LeaveDialogue();

    sequence?.Kill();
    sequence = DOTween.Sequence();
    sequence.SetUpdate(true);

    sequence
      .Join(topElements.DOFade(0f, duration / 2))
      .Join(bottomElements.DOFade(0f, duration / 2))
      .Join(sprite.DOFade(0f, duration / 2))
      .Insert(duration / 3, background.DOFade(0f, duration))
      .Join(topUI.GetComponent<RectTransform>().DOScaleY(0f, duration / 2).SetEase(exitEase))
      .Join(bottomUI.GetComponent<RectTransform>().DOScaleY(0f, duration / 2).SetEase(exitEase))
      .OnComplete(CleanupDialogue);
  }

  void CleanupDialogue()
  {
    canvas.SetActive(false);

    GameInputManager.Instance.Actions.UI.DialogueAdvance.performed -= AdvanceDialogue;
    GameInputManager.Instance.Actions.UI.DialogueSkip.performed -= SkipDialogue;
    GameInputManager.Instance.Actions.UI.DialogueSkip.started -= StartSkipHold;
    GameInputManager.Instance.Actions.UI.DialogueSkip.canceled -= CancelSkipHold;

    middleUI.onClick.RemoveAllListeners();
    bottomUI.onClick.RemoveAllListeners();
    showLogButton.onClick.RemoveAllListeners();
    hideLogButton.onClick.RemoveAllListeners();
    hideUIButton.onClick.RemoveAllListeners();
    showUIPanel.onClick.RemoveAllListeners();
    autoButton.onClick.RemoveAllListeners();
    skipButton.onClick.RemoveAllListeners();
  }

  void SkipLine() => skipLine = true;

  // Auto-advancing dialogue
  void AutoAdvanceDialogue() => GameEventsManager.Instance.dialogueEvents.AdvanceDialogue();
  // Manually advancing dialogue
  void AdvanceDialogue(InputAction.CallbackContext context) => AdvanceDialogue();
  void AdvanceDialogue()
  {
    if (isOverlay) return;
    if (isAutoMode) DisableAutoMode();
    GameEventsManager.Instance.dialogueEvents.AdvanceDialogue();
  }

  async void DialogueDisplay(string text, List<string> tags, List<Choice> choices, CancellationToken token)
  {
    if (sequence.active) await sequence.AsyncWaitForCompletion();
    ClearDialogue();

    try
    {
      await HandleTags(tags, token);
      LogEntry(text, tags);
      await DisplayTypingText(text, token);
      DisplayChoices(choices);
      advanceIndicator.gameObject.SetActive(true);
      if (isAutoMode) _ = StartAutoAdvanceTimer();
    }
    catch (OperationCanceledException) { }
  }

  async Task DisplayTypingText(string text, CancellationToken token)
  {
    skipLine = false;
    GameEventsManager.Instance.dialogueEvents.SetTypingState(true);

    dialogueLine.text = text;
    dialogueLine.maxVisibleCharacters = 0;

    bool isStyling = false;

    foreach (char c in text)
    {
      if (token.IsCancellationRequested) return;

      if (skipLine)
      {
        dialogueLine.maxVisibleCharacters = text.Length;
        break;
      }

      if (c == '<' || isStyling)
      {
        isStyling = true;
        if (c == '>') isStyling = false;
      }
      else
      {
        dialogueLine.maxVisibleCharacters++;
        await Task.Delay(typingSpeed, token);
      }

    }

    GameEventsManager.Instance.dialogueEvents.SetTypingState(false);
  }



  async Task HandleTags(List<string> tags, CancellationToken token)
  {
    foreach (string tag in tags)
    {
      string[] splitTag = tag.Split(":");
      if (splitTag.Length < 2) continue;

      string key = splitTag[0].Trim();
      string value = splitTag[1].Trim();

      switch (key)
      {
        case DialogueAsset.SPEAKER_TAG:
          speaker.text = value;
          speakerBox.SetActive(true);
          break;

        case DialogueAsset.SPRITE_TAG:
          Sprite charSprite = Resources.Load<Sprite>(DialogueAsset.SPRITE_DIR + value);
          if (charSprite == null) Debug.LogWarning("Character sprite not found: " + value);
          sprite.sprite = charSprite;
          sprite.gameObject.SetActive(true);
          break;

        case DialogueAsset.BG_TAG:
          Sprite bgSprite = Resources.Load<Sprite>(DialogueAsset.BG_DIR + value);
          if (bgSprite == null) Debug.LogWarning("Background sprite not found: " + value);
          background.sprite = bgSprite;
          break;

        case DialogueAsset.CG_TAG:
          await HandleCGTag(value, token);
          break;
      }
    }
  }

  async Task HandleCGTag(string tagValue, CancellationToken token)
  {
    string[] parts = tagValue.Split(',');
    string fileName = parts[0].Trim();

    float duration = 0f;
    bool isFull = false;

    if (parts.Length > 1)
    {
      float.TryParse(parts[1].Trim(), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out duration);
    }

    if (parts.Length > 2)
    {
      if (parts[2].Trim().ToLower() == "full") isFull = true;
    }

    await DisplayCG(fileName, duration, isFull, token);
  }

  async Task DisplayCG(string fileName, float duration, bool isFull, CancellationToken token)
  {
    Sprite cg = Resources.Load<Sprite>(DialogueAsset.CG_DIR + fileName);
    if (cg == null)
    {
      Debug.LogWarning("CG not found: " + fileName);
      return;
    }

    background.sprite = cg;

    if (duration <= 0f) return;

    if (isFull) mainUIPanel.SetActive(false);

    try { await Task.Delay((int)(duration * 1000), token); }
    catch (OperationCanceledException) { }
    finally { if (isFull) mainUIPanel.SetActive(true); }
  }

  void DisplayChoices(List<Choice> inkChoices)
  {
    if (inkChoices == null || inkChoices.Count == 0) return;

    choicesOverlay.SetActive(true);

    foreach (Choice choice in inkChoices)
    {
      GameObject choiceObj = Instantiate(choicePrefab, choicesContainer.transform);
      TextMeshProUGUI choiceText = choiceObj.GetComponentInChildren<TextMeshProUGUI>();
      if (choiceText != null) choiceText.text = choice.text;

      // Setup button click event
      Button button = choiceObj.GetComponent<Button>();
      if (button != null)
      {
        int index = choice.index;
        button.onClick.AddListener(() =>
        {
          LogEntry(choice.text, null, true);
          GameEventsManager.Instance.dialogueEvents.SelectChoice(index);
        });
      }
    }
  }

  void LogEntry(string text, List<string> tags, bool isChoice = false)
  {
    string speakerName = ""; // Default to empty for narrative/choices

    // Only look for speaker tags if this isn't a choice selection
    if (!isChoice && tags != null)
    {
      foreach (string tag in tags)
      {
        string[] splitTag = tag.Split(':');
        if (splitTag.Length >= 2 && splitTag[0].Trim() == DialogueAsset.SPEAKER_TAG)
        {
          speakerName = splitTag[1].Trim();
          break;
        }
      }
    }

    if (isChoice) logPanel.AddEntry(speakerName, $"<color=#D1822C>[{text}]</color>");
    else logPanel.AddEntry(speakerName, text);
  }

  void ClearDialogue()
  {
    advanceIndicator.gameObject.SetActive(false);
    sprite.gameObject.SetActive(false);
    speaker.text = "";
    speakerBox.SetActive(false);
    dialogueLine.text = "";

    // Destroy old choices
    foreach (Transform child in choicesContainer.transform) Destroy(child.gameObject);
    choicesOverlay.SetActive(false);
  }
}