using System.Collections.Generic;
using System.Threading.Tasks;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class GameplayDialogueUIController : MonoBehaviour
{
  private UIDocument UIDocument;
  private VisualElement root;
  private Label dialogueText;
  private Label characterName;
  private Image sprite;
  private VisualElement choicesContainer;

  private bool skipLine = false;

  private const string SPEAKER_TAG = "speaker";
  private const string SPRITE_TAG = "sprite";
  private const string SPRITE_DIR = "Assets/Sprites/";

  [SerializeField] private int typingSpeed = 5;

  void Awake()
  {
    UIDocument = GetComponent<UIDocument>();
    root = UIDocument.rootVisualElement;
    dialogueText = root.Q<Label>("dialogue");
    characterName = root.Q<Label>("speaker");
    sprite = root.Q<Image>("sprite");
    choicesContainer = root.Q<VisualElement>("choices");
  }

  void Start()
  {
    root.style.display = DisplayStyle.None;
    dialogueText.text = "";
  }

  void OnEnable()
  {
    GameEventsManager.Instance.dialogueEvents.onDialogueStarted += DialogueStart;
    GameEventsManager.Instance.dialogueEvents.onDialogueEnded += DialogueEnd;
    GameEventsManager.Instance.dialogueEvents.onDialogueDisplayed += DialogueDisplay;
    InputActionsManager.Instance.inputActions.UI.Submit.performed += SkipLine;
  }

  void OnDisable()
  {
    GameEventsManager.Instance.dialogueEvents.onDialogueStarted -= DialogueStart;
    GameEventsManager.Instance.dialogueEvents.onDialogueEnded -= DialogueEnd;
    GameEventsManager.Instance.dialogueEvents.onDialogueDisplayed -= DialogueDisplay;
    InputActionsManager.Instance.inputActions.UI.Submit.performed -= SkipLine;
  }

  void DialogueStart()
  {
    root.style.display = DisplayStyle.Flex;
    ClearDialogue();
  }

  void DialogueEnd()
  {
    root.style.display = DisplayStyle.None;
  }

  void SkipLine(InputAction.CallbackContext ctx) => skipLine = true;

  async void DialogueDisplay(string text, List<string> tags, List<Choice> choices)
  {
    ClearDialogue();
    DisplayTags(tags);
    await DisplayTypingText(text);
    DisplayChoices(choices);
  }

  async Task DisplayTypingText(string text)
  {
    skipLine = false;
    GameEventsManager.Instance.dialogueEvents.SetTypingState(true);
    foreach (char c in text)
    {
      if (skipLine)
      {
        dialogueText.text = text;
        break;
      }
      dialogueText.text += c;
      await Task.Delay(typingSpeed);
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
          characterName.text = value;
          characterName.style.display = DisplayStyle.Flex;
          break;
        case SPRITE_TAG:
          // StyleBackground image = new(Resources.Load<Sprite>(SPRITE_DIR + value));
          // sprite.style.backgroundImage = image;
          sprite.sprite = Resources.Load<Sprite>(SPRITE_DIR + value);
          sprite.style.display = DisplayStyle.Flex;
          break;
      }
    }
  }

  void DisplayChoices(List<Choice> choices)
  {
    foreach (Choice choice in choices)
    {
      Button button = new() { text = choice.text };
      button.AddToClassList("choice");
      button.clicked += () => GameEventsManager.Instance.dialogueEvents.SelectChoice(choice.index);
      choicesContainer.Add(button);
    }

    if (choices.Count > 0)
    {
      choicesContainer.style.display = DisplayStyle.Flex;
      choicesContainer.schedule.Execute(() => choicesContainer.ElementAt(0).Focus());
    }
  }

  void ClearDialogue()
  {
    dialogueText.text = "";
    characterName.style.display = DisplayStyle.None;
    sprite.style.display = DisplayStyle.None;
    choicesContainer.Clear();
    choicesContainer.style.display = DisplayStyle.None;
  }
}