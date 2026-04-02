using UnityEngine;

[RequireComponent(typeof(ILocker))]
public class QuestUnlocker : MonoBehaviour
{
  [SerializeField] private Quest quest;

  private ILocker locker;

  void Awake()
  {
    locker = GetComponent<ILocker>();
  }

  void OnEnable()
  {
    GameEventsManager.Instance.questEvents.OnQuestStateChanged += CheckQuest;
  }

  void OnDisable()
  {
    GameEventsManager.Instance.questEvents.OnQuestStateChanged -= CheckQuest;
  }

  void Start()
  {
    // Check if the player already finished the quest before this scene loaded
    QuestState currentState = GameQuestManager.Instance.GetQuestState(quest.id);
    if (currentState == QuestState.COMPLETED) Unlock();
  }

  void CheckQuest(Quest quest)
  {
    if (quest.id != this.quest.id) return;
    if (quest.GetState() == QuestState.COMPLETED) Unlock();
  }

  void Unlock()
  {
    // Do not change the order as the key script requires lock script exists.
    Destroy(this);
    locker.Unlock();
  }
}