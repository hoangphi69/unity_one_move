using UnityEngine;

public class QuestStartPoint : MonoBehaviour
{
  [SerializeField] private Quest quest;

  void Awake()
  {
    GameEventsManager.Instance.questEvents.StartQuest(quest.id);
  }
}