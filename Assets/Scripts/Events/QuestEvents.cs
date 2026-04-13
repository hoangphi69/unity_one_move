using System;

public class QuestEvents
{
  // Quest
  public event Action<string> OnStartQuest;
  public void StartQuest(string id) => OnStartQuest?.Invoke(id);

  public event Action<Quest> OnQuestStateChanged;
  public void QuestStateChanged(Quest quest) => OnQuestStateChanged?.Invoke(quest);

  public event Action<string> OnAdvanceQuest;
  public void AdvanceQuest(string id) => OnAdvanceQuest?.Invoke(id);

  public event Action<string> OnCompleteQuest;
  public void CompleteQuest(string id) => OnCompleteQuest?.Invoke(id);

  // Quest objective
  public event Action<string> OnObjectiveActive;
  public void ObjectiveActive(string id) => OnObjectiveActive?.Invoke(id);

  public event Action<string> OnQueryObjectiveState;
  public void QueryObjectiveState(string id) => OnQueryObjectiveState?.Invoke(id);

  public event Action<string> OnObjectiveComplete;
  public void ObjectiveComplete(string id) => OnObjectiveComplete?.Invoke(id);
}