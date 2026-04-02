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
    quest.SetState(QuestState.ACTIVE);
    quest.StartObjective();

    GameEventsManager.Instance.questEvents.QuestStateChanged(quest);
  }

  void AdvanceQuest(string id)
  {
    Quest quest = GetQuestByID(id);
    quest.AdvanceObjective();
    bool achieved = !quest.StartObjective();

    if (achieved && !quest.requiresTurnIn) quest.SetState(QuestState.COMPLETED);
    else quest.SetState(QuestState.ACHIEVED);

    GameEventsManager.Instance.questEvents.QuestStateChanged(quest);
  }

  void CompleteQuest(string id)
  {
    Quest quest = GetQuestByID(id);
    quest.SetState(QuestState.COMPLETED);

    GameEventsManager.Instance.questEvents.QuestStateChanged(quest);
  }

  Dictionary<string, Quest> GetQuests()
  {
    Quest[] allQuests = Resources.LoadAll<Quest>("Quests");
    quests = new();
    foreach (Quest asset in allQuests)
    {
      Quest quest = Instantiate(asset);
      quests.Add(quest.id, quest);
    }
    return quests;
  }

  Quest GetQuestByID(string id)
  {
    if (quests.TryGetValue(id, out Quest quest)) return quest;
    return null;
  }

  public Dictionary<string, Quest> GetQuestDictionary() => quests;

  public QuestState GetQuestState(string id)
  {
    if (quests.TryGetValue(id, out Quest quest)) return quest.GetState();
    return QuestState.UNKNOWN;
  }
}