using System;
using System.Collections.Generic;
using System.Threading;
using Ink.Runtime;

public class DialogueEvents
{
    public event Action<string, DialogueMode> onEnterDialogue;
    public void EnterDialogue(string knotName, DialogueMode mode) => onEnterDialogue?.Invoke(knotName, mode);

    public event Action<DialogueMode> onDialogueStarted;
    public void StartDialogue(DialogueMode mode) => onDialogueStarted?.Invoke(mode);

    public event Action onDialogueEnded;
    public void EndDialogue() => onDialogueEnded?.Invoke();

    public event Action onLeaveDialogue;
    public void LeaveDialogue() => onLeaveDialogue?.Invoke();

    public event Action onAdvanceDialogue;
    public void AdvanceDialogue() => onAdvanceDialogue?.Invoke();

    public event Action<string, List<string>, List<Choice>, CancellationToken> onDialogueDisplayed;
    public void DisplayDialogue(string text, List<string> tags, List<Choice> choices, CancellationToken token) => onDialogueDisplayed?.Invoke(text, tags, choices, token);

    public event Action<bool> onTypingStateChanged;
    public void SetTypingState(bool isTyping) => onTypingStateChanged?.Invoke(isTyping);

    public event Action<int> onChoiceSelected;
    public void SelectChoice(int choice) => onChoiceSelected?.Invoke(choice);

    public event Action onRequestSkipLine;
    public void RequestSkipLine() => onRequestSkipLine?.Invoke();
}
