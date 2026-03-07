using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.UIElements;

public class CutsceneDialogueUIController : MonoBehaviour
{
  private VisualElement root;
  private Image background;
  private Image character;

  private Label speaker;
  private Label line;
  private VisualElement choices;

  private bool skipLine = false;

  private const string SPEAKER_TAG = "speaker";
  private const string SPRITE_TAG = "sprite";
  private const string BG_TAG = "bg";
  private const string SPRITE_DIR = "Assets/Sprites/";

  [SerializeField] private int typingSpeed = 5;

  [SerializeField] private float fadeDuration = 0.5f;
  private Tween activeFadeTween;

  void Awake()
  {
    root = GetComponent<UIDocument>().rootVisualElement;
    background = root.Q<Image>("background");
    character = root.Q<Image>("character");

    speaker = root.Q<Label>("speaker");
    line = root.Q<Label>("line");
    choices = root.Q<VisualElement>("choices");

    root.style.opacity = 0;
    root.style.display = DisplayStyle.None;
  }

  void OnEnable()
  {
    GameEventsManager.Instance.dialogueEvents.onDialogueStarted += DialogueStart;
    GameEventsManager.Instance.dialogueEvents.onDialogueEnded += DialogueEnd;
    GameEventsManager.Instance.dialogueEvents.onDialogueDisplayed += DialogueDisplay;
    GameEventsManager.Instance.dialogueEvents.onRequestSkipLine += SkipLine;
  }

  void OnDisable()
  {
    // Kill any active tweens to prevent memory leaks or errors
    activeFadeTween?.Kill();

    GameEventsManager.Instance.dialogueEvents.onDialogueStarted -= DialogueStart;
    GameEventsManager.Instance.dialogueEvents.onDialogueEnded -= DialogueEnd;
    GameEventsManager.Instance.dialogueEvents.onDialogueDisplayed -= DialogueDisplay;
    GameEventsManager.Instance.dialogueEvents.onRequestSkipLine -= SkipLine;
  }

  // Marked as 'async void' to be compatible with event delegates
  async void DialogueStart(DialogueMode mode)
  {
    if (mode != DialogueMode.Cutscene) return;
    // Kill existing animation if user triggers start while fading out
    activeFadeTween?.Kill();

    ClearDialogue();

    // Enable layout immediately so it is visible during fade
    root.style.display = DisplayStyle.Flex;

    // Animate Opacity 0 -> 1
    activeFadeTween = DOTween.To(
        () => root.style.opacity.value,     // Getter
        x => root.style.opacity = x,        // Setter
        1f,                                 // Target
        fadeDuration                        // Duration
    ).SetEase(Ease.OutQuad);

    try
    {
      // Await the tween completion safely
      await activeFadeTween.AsyncWaitForCompletion();
    }
    catch (OperationCanceledException)
    {
      // Handle case where object is destroyed mid-tween
      return;
    }
  }

  // Marked as 'async void' to be compatible with event delegates
  async void DialogueEnd()
  {
    activeFadeTween?.Kill();

    ClearDialogue();

    // Animate Opacity Current -> 0
    activeFadeTween = DOTween.To(
        () => root.style.opacity.value,
        x => root.style.opacity = x,
        0f,
        fadeDuration
    ).SetEase(Ease.InQuad);

    try
    {
      await activeFadeTween.AsyncWaitForCompletion();

      // Disable layout strictly AFTER fade out completes
      root.style.display = DisplayStyle.None;
      CutsceneManager.Instance.EndCutscene();
    }
    catch (OperationCanceledException)
    {
      return;
    }
  }

  void SkipLine() => skipLine = true;

  async void DialogueDisplay(string text, List<string> tags, List<Choice> choices, CancellationToken token)
  {
    ClearDialogue();
    DisplayTags(tags);

    try
    {
      await DisplayTypingText(text, token);
      DisplayChoices(choices);
    }
    catch (OperationCanceledException) { }
  }

  async Task DisplayTypingText(string text, CancellationToken token)
  {
    skipLine = false;
    GameEventsManager.Instance.dialogueEvents.SetTypingState(true);

    foreach (char c in text)
    {
      if (token.IsCancellationRequested) return;
      if (skipLine)
      {
        line.text = text;
        break;
      }
      line.text += c;
      await Task.Delay(typingSpeed, token);
    }

    GameEventsManager.Instance.dialogueEvents.SetTypingState(false);
  }

  void DisplayTags(List<string> tags)
  {
    foreach (string tag in tags)
    {
      string key = tag.Split(":")[0];
      string value = tag.Split(":")[1];

      switch (key)
      {
        case SPEAKER_TAG:
          speaker.text = value;
          speaker.style.display = DisplayStyle.Flex;
          break;
        case SPRITE_TAG:
          character.sprite = Resources.Load<Sprite>(SPRITE_DIR + value);
          character.style.display = DisplayStyle.Flex;
          break;
        case BG_TAG:
          background.sprite = Resources.Load<Sprite>(SPRITE_DIR + value);
          background.style.display = DisplayStyle.Flex;
          break;
      }
    }
  }

  void DisplayChoices(List<Choice> choices)
  {
    foreach (Choice choice in choices)
    {
      Button button = new() { text = choice.text };
      button.AddToClassList("button");
      button.clicked += () => GameEventsManager.Instance.dialogueEvents.SelectChoice(choice.index);
      this.choices.Add(button);
    }

    if (choices.Count > 0)
    {
      this.choices.style.display = DisplayStyle.Flex;
      this.choices.schedule.Execute(() => this.choices.ElementAt(0).Focus());
    }
  }

  void ClearDialogue()
  {
    character.style.display = DisplayStyle.None;
    line.text = "";
    speaker.style.display = DisplayStyle.None;
    choices.Clear();
    choices.style.display = DisplayStyle.None;
  }
}