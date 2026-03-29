using System.Collections.Generic;
using UnityEngine;

public class GameQuestManager : MonoBehaviour
{
  public static GameQuestManager Instance { get; private set; }
  private Dictionary<string, Quest> quests;

  void Awake()
  {
    if (Instance == null) Instance = this;
    quests = GetQuests();
  }

  void OnEnable()
  {
    GameEventsManager.Instance.questEvents.OnStartQuest += StartQuest;
    GameEventsManager.Instance.questEvents.OnAdvanceQuest += AdvanceQuest;
    GameEventsManager.Instance.questEvents.OnCompleteQuest += CompleteQuest;
  }

  void OnDisable()
  {
    GameEventsManager.Instance.questEvents.OnStartQuest -= StartQuest;
    GameEventsManager.Instance.questEvents.OnAdvanceQuest -= AdvanceQuest;
    GameEventsManager.Instance.questEvents.OnCompleteQuest -= CompleteQuest;
  }

  void StartQuest(string id)
  {
    Quest quest = GetQuestByID(id);
    quest.InitializeCurrentStep(transform);
    ChangeQuestState(quest.info.id, QuestState.IN_PROGRESS);
  }

  void AdvanceQuest(string id)
  {
    Quest quest = GetQuestByID(id);
    quest.MoveToNextStep();
    if (quest.CurrentStepExists()) quest.InitializeCurrentStep(transform);
    else ChangeQuestState(quest.info.id, QuestState.COMPLETED); // IMPORTANT - change this to DONE later
  }

  void CompleteQuest(string id)
  {
    Quest quest = GetQuestByID(id);
    ChangeQuestState(quest.info.id, QuestState.COMPLETED);
  }

  void ChangeQuestState(string id, QuestState state)
  {
    Quest quest = GetQuestByID(id);
    quest.state = state;
    GameEventsManager.Instance.questEvents.QuestStateChanged(quest);
  }

  Dictionary<string, Quest> GetQuests()
  {
    QuestInfoSO[] allQuestInfoSO = Resources.LoadAll<QuestInfoSO>("Quests");
    quests = new();
    foreach (QuestInfoSO questInfo in allQuestInfoSO)
    {
      quests.Add(questInfo.id, new Quest(questInfo));
    }
    return quests;
  }

  Quest GetQuestByID(string id)
  {
    if (quests.TryGetValue(id, out Quest quest)) return quest;
    return null;
  }

  public QuestState GetQuestState(string id)
  {
    if (quests.TryGetValue(id, out Quest quest)) return quest.state;
    return QuestState.UNAVAILABLE;
  }

  public bool MeetRequirements(Quest quest)
  {
    bool result = true;

    foreach (QuestInfoSO requiredQuest in quest.info.questPrerequisites)
    {
      if (GetQuestByID(requiredQuest.id).state != QuestState.COMPLETED) result = false;
    }

    return result;
  }
}