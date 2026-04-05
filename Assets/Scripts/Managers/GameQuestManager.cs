using System.Collections.Generic;
using UnityEngine;

public class GameQuestManager : MonoBehaviour
{
  public static GameQuestManager Instance { get; private set; }
  private Dictionary<string, Quest> quests;

  void Awake()
  {
    if (Instance == null) Instance = this;
    LoadQuestResources();
    if (GameDataManager.Instance.HasData()) LoadQuests(GameDataManager.Instance.data);
  }

  void OnEnable()
  {
    GameEventsManager.Instance.questEvents.OnStartQuest += StartQuest;
    GameEventsManager.Instance.questEvents.OnAdvanceQuest += AdvanceQuest;
    GameEventsManager.Instance.questEvents.OnCompleteQuest += CompleteQuest;

    GameDataManager.Instance.OnLoad += LoadQuests;
    GameDataManager.Instance.OnSave += SaveQuests;
    GameDataManager.Instance.OnRefresh += LoadQuestResources;
  }

  void OnDisable()
  {
    GameEventsManager.Instance.questEvents.OnStartQuest -= StartQuest;
    GameEventsManager.Instance.questEvents.OnAdvanceQuest -= AdvanceQuest;
    GameEventsManager.Instance.questEvents.OnCompleteQuest -= CompleteQuest;

    GameDataManager.Instance.OnLoad -= LoadQuests;
    GameDataManager.Instance.OnSave -= SaveQuests;
    GameDataManager.Instance.OnRefresh -= LoadQuestResources;
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
    else if (achieved) quest.SetState(QuestState.ACHIEVED);

    GameEventsManager.Instance.questEvents.QuestStateChanged(quest);
  }

  void CompleteQuest(string id)
  {
    Quest quest = GetQuestByID(id);
    quest.SetState(QuestState.COMPLETED);

    GameEventsManager.Instance.questEvents.QuestStateChanged(quest);
  }

  void LoadQuestResources()
  {
    Quest[] allQuests = Resources.LoadAll<Quest>("Quests");
    quests = new();
    foreach (Quest asset in allQuests)
    {
      Quest quest = Instantiate(asset);
      quests.Add(quest.id, quest);
    }
  }

  public void LoadQuests(GameData data)
  {
    if (data == null) return;
    if (data.quests == null) return;

    foreach (var questData in data.quests)
    {
      Quest quest = GetQuestByID(questData.id);
      quest.Load(questData);
      GameEventsManager.Instance.questEvents.QuestStateChanged(quest);
    }
  }

  void SaveQuests(GameData data)
  {
    data.quests.Clear();
    foreach (var kvp in quests)
    {
      if (kvp.Value.GetState() == QuestState.UNKNOWN) continue;
      data.quests.Add(kvp.Value.Save());
    }
  }

  Quest GetQuestByID(string id)
  {
    if (quests.TryGetValue(id, out Quest quest)) return quest;
    return null;
  }

  public Dictionary<string, Quest> GetQuests() => quests;

  // Only grab quests the player is currently doing or ready to turn in
  public List<Quest> GetActiveQuests()
  {
    List<Quest> activeQuests = new List<Quest>();
    foreach (var kvp in quests)
    {
      QuestState state = kvp.Value.GetState();
      if (state == QuestState.ACTIVE || state == QuestState.ACHIEVED)
      {
        activeQuests.Add(kvp.Value);
      }
    }
    return activeQuests;
  }

  public QuestState GetQuestState(string id)
  {
    if (quests.TryGetValue(id, out Quest quest)) return quest.GetState();
    return QuestState.UNKNOWN;
  }
}