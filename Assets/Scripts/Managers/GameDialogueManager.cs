using System.Threading;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

public enum DialogueMode { Cutscene, InGame }

public class GameDialogueManager : MonoBehaviour
{
  [Header("Ink Story")]
  [SerializeField] private TextAsset inkJSON;
  private Story story;

  private DialogueMode currentMode;
  private bool dialogueActive = false;
  private bool isTyping = false;
  private CancellationTokenSource _displaying;

  void Awake()
  {
    story = new Story(inkJSON.text);
  }

  void OnEnable()
  {
    GameEventsManager.Instance.dialogueEvents.onEnterDialogue += EnterDialogue;
    GameEventsManager.Instance.dialogueEvents.onChoiceSelected += SelectChoice;
    GameEventsManager.Instance.dialogueEvents.onTypingStateChanged += SetTypingState;
    GameInputManager.Instance.Actions.UI.DialogueAdvance.performed += ContinueDialogue;
    GameInputManager.Instance.Actions.UI.DialogueSkip.performed += SkipCutsceneDialogue;
  }

  void OnDisable()
  {
    GameEventsManager.Instance.dialogueEvents.onEnterDialogue -= EnterDialogue;
    GameEventsManager.Instance.dialogueEvents.onChoiceSelected -= SelectChoice;
    GameEventsManager.Instance.dialogueEvents.onTypingStateChanged -= SetTypingState;
    GameInputManager.Instance.Actions.UI.DialogueAdvance.performed -= ContinueDialogue;
    GameInputManager.Instance.Actions.UI.DialogueSkip.performed -= SkipCutsceneDialogue;
  }

  void SetTypingState(bool isTyping) => this.isTyping = isTyping;

  void EnterDialogue(string knotName, DialogueMode mode)
  {
    if (dialogueActive) return;
    if (knotName.Equals("")) return;

    dialogueActive = true;
    currentMode = mode;
    GameInputManager.Instance.SetState(InputState.UI);
    GameEventsManager.Instance.dialogueEvents.StartDialogue(mode);
    story.ChoosePathString(knotName);
    ContinueDialogue();
  }

  void ContinueDialogue(InputAction.CallbackContext context) => ContinueDialogue();
  void ContinueDialogue()
  {
    if (!dialogueActive) return;

    if (isTyping)
    {
      GameEventsManager.Instance.dialogueEvents.RequestSkipLine();
      return;
    }

    if (story.currentChoices.Count > 0) return;

    if (story.canContinue)
    {
      CancelDisplaying();
      _displaying = new();

      GameEventsManager.Instance.dialogueEvents.DisplayDialogue(
        story.Continue(),
        story.currentTags,
        story.currentChoices,
        _displaying.Token
      );
    }
    else ExitDialogue();
  }

  void CancelDisplaying()
  {
    if (_displaying != null)
    {
      _displaying.Cancel();
      _displaying.Dispose();
      _displaying = null;
    }
  }

  void SkipCutsceneDialogue(InputAction.CallbackContext context)
  {
    if (currentMode != DialogueMode.Cutscene) return;
    ExitDialogue();
  }

  void ExitDialogue()
  {
    CancelDisplaying();
    SetTypingState(false);
    dialogueActive = false;
    GameEventsManager.Instance.dialogueEvents.EndDialogue();
    GameInputManager.Instance.SetState(InputState.Gameplay);
  }

  void SelectChoice(int choiceIndex)
  {
    story.ChooseChoiceIndex(choiceIndex);
    ContinueDialogue();
  }
}