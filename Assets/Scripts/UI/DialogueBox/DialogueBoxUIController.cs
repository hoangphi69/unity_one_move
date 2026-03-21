using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
  [SerializeField] private Image sprite;
  [SerializeField] private TextMeshProUGUI speaker;
  [SerializeField] private GameObject speakerBox;
  [SerializeField] private TextMeshProUGUI dialogueLine;
  [SerializeField] private Image advanceIndicator;
  [SerializeField] private GameObject choicesContainer;
  [SerializeField] private GameObject choicePrefab;

  [Header("Config")]
  [SerializeField] private int typingSpeed = 50;
  private bool skipLine = false;

  private const string SPEAKER_TAG = "speaker";
  private const string SPRITE_TAG = "sprite";
  private const string SPRITE_DIR = "Sprites/InGameDialogue/";

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

    GameInputManager.Instance.Actions.UI.DialogueAdvance.performed += AdvanceDialogue;
    background.onClick.AddListener(AdvanceDialogue);
  }

  void DialogueEnd()
  {
    uiContainer.SetActive(false);

    GameInputManager.Instance.Actions.UI.DialogueAdvance.performed -= AdvanceDialogue;
    background.onClick.RemoveAllListeners();
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
    DisplayTags(tags);

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

  void DisplayTags(List<string> tags)
  {
    foreach (string tag in tags)
    {
      string[] splitTag = tag.Split(":");
      if (splitTag.Length < 2) continue;

      string key = splitTag[0].Trim();
      string value = splitTag[1].Trim();

      switch (key)
      {
        case SPEAKER_TAG:
          speaker.text = value;
          speakerBox.SetActive(true);
          break;
        case SPRITE_TAG:
          Sprite charSprite = Resources.Load<Sprite>(SPRITE_DIR + value);
          if (charSprite == null) Debug.LogWarning("Character sprite not found: " + value);
          sprite.sprite = charSprite;
          sprite.gameObject.SetActive(true);
          break;
      }
    }
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