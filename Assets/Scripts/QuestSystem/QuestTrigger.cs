using UnityEngine;

public class QuestTrigger : MonoBehaviour
{
  [SerializeField] private Quest quest;

  private ITrigger trigger;

  void Awake()
  {
    trigger = GetComponent<ITrigger>();
  }

  void OnEnable()
  {
    if (trigger != null)
    {
      trigger.OnMainAction += StartQuest;
    }
  }

  void OnDisable()
  {
    if (trigger != null)
    {
      trigger.OnMainAction -= StartQuest;
    }
  }

  void StartQuest()
  {
    GameEventsManager.Instance.questEvents.StartQuest(quest.id);
  }
}