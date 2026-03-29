using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
  private bool isDone = false;
  private string questID;

  public void Initialize(string questID)
  {
    this.questID = questID;
  }

  protected void Done()
  {
    if (isDone) return;
    isDone = true;
    GameEventsManager.Instance.questEvents.AdvanceQuest(questID);
    Destroy(gameObject);
  }
}