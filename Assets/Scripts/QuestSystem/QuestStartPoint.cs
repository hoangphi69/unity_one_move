using UnityEngine;

public class QuestStartPoint : MonoBehaviour
{
  void Awake()
  {
    GameEventsManager.Instance.questEvents.StartQuest("QuestCheckPhone");
  }
}