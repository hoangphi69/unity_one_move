using System;
using System.Collections.Generic;
using Ink.Runtime;

public class DialogueEvents
{
    public event Action<string> onEnterDialogue;
    public void EnterDialogue(string knotName) => onEnterDialogue?.Invoke(knotName);

    public event Action onDialogueStarted;
    public void StartDialogue() => onDialogueStarted?.Invoke();

    public event Action onDialogueEnded;
    public void EndDialogue() => onDialogueEnded?.Invoke();

    public event Action onLeaveDialogue;
    public void LeaveDialogue() => onLeaveDialogue?.Invoke();

    public event Action<string, List<string>, List<Choice>> onDialogueDisplayed;
    public void DisplayDialogue(string text, List<string> tags, List<Choice> choices) => onDialogueDisplayed?.Invoke(text, tags, choices);

    public event Action<bool> onTypingStateChanged;
    public void SetTypingState(bool isTyping) => onTypingStateChanged?.Invoke(isTyping);

    public event Action<int> onChoiceSelected;
    public void SelectChoice(int choice) => onChoiceSelected?.Invoke(choice);
}
