using System.Threading;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

public enum DialogueMode { Cutscene, InGame }

public class DialogueManager : MonoBehaviour
{
  [Header("Ink Story")]
  [SerializeField] private TextAsset inkJSON;
  private Story story;

  private DialogueMode currentMode;
  private bool dialogueActive = false;
  private bool isTyping = false;
  private CancellationTokenSource _typing;

  void Awake()
  {
    story = new Story(inkJSON.text);
  }

  void OnEnable()
  {
    GameEventsManager.Instance.dialogueEvents.onEnterDialogue += EnterDialogue;
    GameEventsManager.Instance.dialogueEvents.onChoiceSelected += SelectChoice;
    GameEventsManager.Instance.dialogueEvents.onTypingStateChanged += SetTypingState;
    InputActionsManager.Instance.inputActions.UI.DialogueAdvance.performed += ContinueDialogue;
    InputActionsManager.Instance.inputActions.UI.DialogueSkip.performed += SkipCutsceneDialogue;
  }

  void OnDisable()
  {
    GameEventsManager.Instance.dialogueEvents.onEnterDialogue -= EnterDialogue;
    GameEventsManager.Instance.dialogueEvents.onChoiceSelected -= SelectChoice;
    GameEventsManager.Instance.dialogueEvents.onTypingStateChanged -= SetTypingState;
    InputActionsManager.Instance.inputActions.UI.DialogueAdvance.performed -= ContinueDialogue;
    InputActionsManager.Instance.inputActions.UI.DialogueSkip.performed -= SkipCutsceneDialogue;
  }

  void SetTypingState(bool isTyping) => this.isTyping = isTyping;

  void EnterDialogue(string knotName, DialogueMode mode)
  {
    if (dialogueActive) return;
    if (knotName.Equals("")) return;

    dialogueActive = true;
    currentMode = mode;
    InputActionsManager.Instance.SetState(InputState.UI);
    GameEventsManager.Instance.dialogueEvents.StartDialogue(mode);
    story.ChoosePathString(knotName);
    ContinueDialogue();
  }

  void ContinueDialogue(InputAction.CallbackContext context) => ContinueDialogue();
  void ContinueDialogue()
  {
    if (!dialogueActive) return;
    if (story.currentChoices.Count > 0) return;

    if (isTyping)
    {
      GameEventsManager.Instance.dialogueEvents.RequestSkipLine();
      return;
    }

    if (story.canContinue)
    {
      CancelTyping();
      _typing = new();

      GameEventsManager.Instance.dialogueEvents.DisplayDialogue(
        story.Continue(),
        story.currentTags,
        story.currentChoices,
        _typing.Token
      );
    }
    else ExitDialogue();
  }

  void CancelTyping()
  {
    if (_typing != null)
    {
      _typing.Cancel();
      _typing.Dispose();
      _typing = null;
    }
  }

  void SkipCutsceneDialogue(InputAction.CallbackContext context)
  {
    if (currentMode != DialogueMode.Cutscene) return;
    ExitDialogue();
  }

  void ExitDialogue()
  {
    CancelTyping();
    dialogueActive = false;
    GameEventsManager.Instance.dialogueEvents.EndDialogue();
    InputActionsManager.Instance.SetState(InputState.Gameplay);
  }

  void SelectChoice(int choiceIndex)
  {
    story.ChooseChoiceIndex(choiceIndex);
    ContinueDialogue();
  }
}