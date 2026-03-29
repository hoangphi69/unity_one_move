using System;

public class QuestEvents
{
  public event Action<string> OnStartQuest;
  public void StartQuest(string id) => OnStartQuest?.Invoke(id);

  public event Action<Quest> OnQuestStateChanged;
  public void QuestStateChanged(Quest quest) => OnQuestStateChanged?.Invoke(quest);

  public event Action<string> OnAdvanceQuest;
  public void AdvanceQuest(string id) => OnAdvanceQuest?.Invoke(id);

  public event Action<string> OnCompleteQuest;
  public void CompleteQuest(string id) => OnCompleteQuest?.Invoke(id);
}