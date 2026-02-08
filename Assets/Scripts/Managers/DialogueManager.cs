using Ink.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
  [Header("Ink Story")]
  [SerializeField] private TextAsset inkJSON;

  private Story story;
  private bool dialogueActive = false;
  private bool isTyping = false;

  void Awake()
  {
    story = new Story(inkJSON.text);
  }

  void OnEnable()
  {
    GameEventsManager.Instance.dialogueEvents.onEnterDialogue += EnterDialogue;
    GameEventsManager.Instance.dialogueEvents.onChoiceSelected += SelectChoice;
    GameEventsManager.Instance.dialogueEvents.onTypingStateChanged += SetTypingState;
    InputActionsManager.Instance.inputActions.UI.Submit.performed += ContinueDialogue;
  }

  void OnDisable()
  {
    GameEventsManager.Instance.dialogueEvents.onEnterDialogue -= EnterDialogue;
    GameEventsManager.Instance.dialogueEvents.onChoiceSelected -= SelectChoice;
    GameEventsManager.Instance.dialogueEvents.onTypingStateChanged -= SetTypingState;
    InputActionsManager.Instance.inputActions.UI.Submit.performed -= ContinueDialogue;
  }

  void SetTypingState(bool isTyping) => this.isTyping = isTyping;

  void EnterDialogue(string knotName)
  {
    if (dialogueActive) return;
    if (knotName.Equals("")) return;

    dialogueActive = true;
    InputActionsManager.Instance.SetState(InputState.UI);
    GameEventsManager.Instance.dialogueEvents.StartDialogue();
    story.ChoosePathString(knotName);
    ContinueDialogue();
  }

  void ContinueDialogue(InputAction.CallbackContext context) => ContinueDialogue();
  void ContinueDialogue()
  {
    if (story.currentChoices.Count > 0) return;
    if (isTyping) return;
    if (story.canContinue)
    {
      GameEventsManager.Instance.dialogueEvents.DisplayDialogue(
        story.Continue(),
        story.currentTags,
        story.currentChoices
      );
    }
    else ExitDialogue();
  }

  void ExitDialogue()
  {
    dialogueActive = false;
    GameEventsManager.Instance.dialogueEvents.EndDialogue();
    InputActionsManager.Instance.SetState(InputState.World);
  }

  void SelectChoice(int choiceIndex)
  {
    story.ChooseChoiceIndex(choiceIndex);
    ContinueDialogue();
  }
}