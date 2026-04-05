using System.Collections.Generic;
using System.Threading;
using Ink.Runtime;
using UnityEngine;

public enum DialogueMode { Cutscene, InGame }

public class GameDialogueManager : MonoBehaviour
{
  [Header("Ink Story")]
  [SerializeField] private TextAsset inkJSON;

  [Header("UI prefabs")]
  [SerializeField] private CutsceneUIController _cutscenePrefab;
  [SerializeField] private DialogueBoxUIController _dialogueBoxPrefab;

  private Story story;
  private Dictionary<DialogueMode, MonoBehaviour> _uiInstances = new();
  private bool dialogueActive = false;
  private bool isTyping = false;
  private CancellationTokenSource _displaying;

  void Awake()
  {
    story = new Story(inkJSON.text);
    if (GameDataManager.Instance.HasData()) LoadInkState(GameDataManager.Instance.data);
  }

  void OnEnable()
  {
    GameEventsManager.Instance.dialogueEvents.onEnterDialogue += EnterDialogue;
    GameEventsManager.Instance.dialogueEvents.onChoiceSelected += SelectChoice;
    GameEventsManager.Instance.dialogueEvents.onTypingStateChanged += SetTypingState;
    GameEventsManager.Instance.dialogueEvents.onAdvanceDialogue += ContinueDialogue;
    GameEventsManager.Instance.dialogueEvents.onLeaveDialogue += LeaveDialogue;

    GameDataManager.Instance.OnLoad += LoadInkState;
    GameDataManager.Instance.OnSave += SaveInkState;
    GameDataManager.Instance.OnRefresh += RefreshInkState;
  }

  void OnDisable()
  {
    GameEventsManager.Instance.dialogueEvents.onEnterDialogue -= EnterDialogue;
    GameEventsManager.Instance.dialogueEvents.onChoiceSelected -= SelectChoice;
    GameEventsManager.Instance.dialogueEvents.onTypingStateChanged -= SetTypingState;
    GameEventsManager.Instance.dialogueEvents.onAdvanceDialogue -= ContinueDialogue;
    GameEventsManager.Instance.dialogueEvents.onLeaveDialogue -= LeaveDialogue;

    GameDataManager.Instance.OnLoad -= LoadInkState;
    GameDataManager.Instance.OnSave -= SaveInkState;
    GameDataManager.Instance.OnRefresh -= RefreshInkState;
  }

  void SetTypingState(bool isTyping) => this.isTyping = isTyping;

  void EnterDialogue(string knotName, DialogueMode mode)
  {
    if (dialogueActive || string.IsNullOrEmpty(knotName)) return;

    GameObject activeUI = GetOrCreateUI(mode);
    if (activeUI == null) return;
    activeUI.SetActive(true);

    GameInputManager.Instance.SetState(InputState.UI);
    GameEventsManager.Instance.dialogueEvents.StartDialogue(mode);

    dialogueActive = true;
    story.ChoosePathString(knotName);
    ContinueDialogue();
  }

  GameObject GetOrCreateUI(DialogueMode mode)
  {
    if (_uiInstances.TryGetValue(mode, out var instance)) return instance.gameObject;

    MonoBehaviour newInstance = mode switch
    {
      DialogueMode.Cutscene => Instantiate(_cutscenePrefab),
      DialogueMode.InGame => Instantiate(_dialogueBoxPrefab),
      _ => null
    };

    if (newInstance != null) _uiInstances.Add(mode, newInstance);
    return newInstance?.gameObject;
  }

  // void ContinueDialogue(InputAction.CallbackContext context) => ContinueDialogue();
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
    else GameEventsManager.Instance.dialogueEvents.EndDialogue();
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

  void LeaveDialogue()
  {
    CancelDisplaying();
    SetTypingState(false);
    dialogueActive = false;
    GameInputManager.Instance.SetState(InputState.Gameplay);
  }

  void SelectChoice(int choiceIndex)
  {
    story.ChooseChoiceIndex(choiceIndex);
    ContinueDialogue();
  }

  void SaveInkState(GameData data)
  {
    if (story == null) return;
    data.dialogueState = story.state.ToJson();
  }

  void LoadInkState(GameData data)
  {
    if (data == null) return;
    if (story == null) return;
    if (string.IsNullOrEmpty(data.dialogueState)) return;
    story.state.LoadJson(data.dialogueState);
  }

  void RefreshInkState()
  {
    if (inkJSON == null) return;
    story = new(inkJSON.text);
  }
}