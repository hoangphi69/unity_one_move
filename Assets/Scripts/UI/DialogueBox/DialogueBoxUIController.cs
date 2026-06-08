using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueBoxUIController : MonoBehaviour
{
  [Header("UI")]
  [SerializeField] private GameObject uiContainer;
  [SerializeField] private Button background;
  [SerializeField] private GameObject dialogueBox;
  [SerializeField] private CanvasGroup elements;
  [SerializeField] private Image sprite;
  [SerializeField] private TextMeshProUGUI speaker;
  [SerializeField] private GameObject speakerBox;
  [SerializeField] private TextMeshProUGUI dialogueLine;
  [SerializeField] private Image advanceIndicator;
  [SerializeField] private GameObject choicesContainer;
  [SerializeField] private GameObject choicePrefab;

  [Header("Config")]
  [SerializeField] private int typingSpeed = 50;
  [SerializeField] private float animationDuration = .15f;
  private bool skipLine = false;
  private Sequence animSequence;

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

  void DialogueStart(DialogueMode mode)
  {
    if (mode != DialogueMode.InGame) return;
    ClearDialogue();
    uiContainer.SetActive(true);

    animSequence?.Kill();
    animSequence = DOTween.Sequence();
    elements.DOFade(0f, 0f);
    animSequence
      .Join(dialogueBox.transform.DOScaleY(1f, animationDuration).From(0).SetEase(Ease.OutSine))
      .Append(elements.DOFade(1f, animationDuration).From(0));

    GameplayManager.Instance.ZoomCamera(4f, animationDuration * 2);
    GameAudioManager.Instance.PlaySFX(AudioTag.DialogueOn);
    GameInputManager.Instance.Actions.UI.DialogueAdvance.performed += AdvanceDialogue;
    background.onClick.AddListener(AdvanceDialogue);
  }

  async void DialogueEnd()
  {
    animSequence?.Kill();
    animSequence = DOTween.Sequence();
    GameplayManager.Instance.ZoomCamera(4.5f, animationDuration * 2);
    await animSequence
      .Join(elements.DOFade(0f, animationDuration / 2))
      .Append(dialogueBox.transform.DOScaleY(0f, animationDuration / 2).SetEase(Ease.InSine))
      .AsyncWaitForCompletion();

    uiContainer.SetActive(false);
    background.onClick.RemoveAllListeners();
    GameAudioManager.Instance.PlaySFX(AudioTag.DialogueOff);
    GameInputManager.Instance.Actions.UI.DialogueAdvance.performed -= AdvanceDialogue;
    GameEventsManager.Instance.dialogueEvents.LeaveDialogue();
  }

  void SkipLine() => skipLine = true;

  void AdvanceDialogue(InputAction.CallbackContext context) => AdvanceDialogue();
  void AdvanceDialogue()
  {
    GameEventsManager.Instance.dialogueEvents.AdvanceDialogue();
  }

  async void DialogueDisplay(string text, List<string> tags, List<Choice> inkChoices, CancellationToken token)
  {
    ClearDialogue();
    HandleTags(tags);

    try
    {
      await DisplayTypingText(text, token);
      DisplayChoices(inkChoices);
      advanceIndicator.gameObject.SetActive(true);
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

  void HandleTags(List<string> tags)
  {
    foreach (string tag in tags)
    {
      string[] splitTag = tag.Split(":");
      if (splitTag.Length < 2) continue;

      string key = splitTag[0].Trim();
      string value = splitTag[1].Trim();

      if (Enum.TryParse(key, true, out InkTag parsedTag))
      {
        switch (parsedTag)
        {
          case InkTag.Speaker: DisplaySpeaker(value); break;
          case InkTag.Sprite: DisplaySprite(value); break;
          case InkTag.Sfx: PlaySFX(value); break;
        }
      }
    }
  }

  void DisplaySpeaker(string name)
  {
    speaker.text = name;
    speakerBox.SetActive(true);
  }

  void DisplaySprite(string fileName)
  {
    Sprite charSprite = Resources.Load<Sprite>(ResourcePath.Sprites + fileName);
    if (charSprite == null) Debug.LogWarning("Character sprite not found: " + fileName);
    sprite.sprite = charSprite;
    sprite.gameObject.SetActive(true);
  }

  void PlaySFX(string tag)
  {
    Enum.TryParse(tag, true, out AudioTag audio);
    GameAudioManager.Instance.PlaySFX(audio);
  }

  void DisplayChoices(List<Choice> inkChoices)
  {
    if (inkChoices == null || inkChoices.Count == 0) return;

    foreach (Choice choice in inkChoices)
    {
      GameObject choiceObj = Instantiate(choicePrefab, choicesContainer.transform);
      TextMeshProUGUI choiceText = choiceObj.GetComponentInChildren<TextMeshProUGUI>();
      if (choiceText != null) choiceText.text = choice.text;

      // Setup button click event
      Button button = choiceObj.GetComponent<Button>();
      if (button != null)
        button.onClick.AddListener(() => GameEventsManager.Instance.dialogueEvents.SelectChoice(choice.index));
    }
  }

  void ClearDialogue()
  {
    advanceIndicator.gameObject.SetActive(false);
    sprite.gameObject.SetActive(false);
    speaker.text = "";
    speakerBox.SetActive(false);
    dialogueLine.text = "";
    foreach (Transform child in choicesContainer.transform) Destroy(child.gameObject);
  }
}