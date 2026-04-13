using UnityEngine;
using NaughtyAttributes;

[RequireComponent(typeof(ITrigger))]
public class InteractPoint : MonoBehaviour
{
  [Header("Quest")]
  [SerializeField] private string targetID;

  [SerializeField] private bool bubble = true;
  [ShowIf("bubble")][SerializeField] private GameObject bubblePrefab;
  [ShowIf("bubble")][SerializeField] private Transform bubbleSpawnPoint;

  private GameObject currentBubbleInstance;
  private ITrigger trigger;

  void Awake()
  {
    trigger = GetComponent<ITrigger>();
  }

  void OnEnable()
  {
    trigger.OnDefaultAction += NotifyInteraction;
    GameEventsManager.Instance.questEvents.OnObjectiveActive += ObjectiveActive;
    GameEventsManager.Instance.questEvents.OnObjectiveComplete += ObjectiveComplete;
  }

  void OnDisable()
  {
    trigger.OnDefaultAction -= NotifyInteraction;
    GameEventsManager.Instance.questEvents.OnObjectiveActive -= ObjectiveActive;
    GameEventsManager.Instance.questEvents.OnObjectiveComplete -= ObjectiveComplete;
  }

  void Start()
  {
    GameEventsManager.Instance.questEvents.QueryObjectiveState(targetID);
  }

  void ObjectiveActive(string id)
  {
    if (id != targetID) return;
    ShowBubble(true);
  }

  void ObjectiveComplete(string id)
  {
    if (id != targetID) return;
    ShowBubble(false);
    Destroy(this);
  }

  void NotifyInteraction()
  {
    if (string.IsNullOrEmpty(targetID)) return;
    GameEventsManager.Instance.interactEvents.Interact(targetID);
  }

  public void ShowBubble(bool show)
  {
    if (!bubble) return;
    if (show && currentBubbleInstance == null && bubblePrefab != null)
    {
      currentBubbleInstance = Instantiate(bubblePrefab, bubbleSpawnPoint.position, bubbleSpawnPoint.rotation, transform);
    }
    else if (!show && currentBubbleInstance != null)
    {
      Destroy(currentBubbleInstance);
    }
  }
}