using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class InteractPoint : MonoBehaviour
{
  [Header("Quest Configuration")]
  [SerializeField] private string targetID;

  [Header("Visual Indicators")]
  [SerializeField] private GameObject bubblePrefab; // The "!" or "?" prefab
  [SerializeField] private Transform bubbleSpawnPoint; // Empty GameObject above the NPC's head

  private GameObject currentBubbleInstance;
  private Interactable interactable;

  private void Awake()
  {
    interactable = GetComponent<Interactable>();
  }

  private void OnEnable()
  {
    interactable.OnDefaultAction += NotifyInteraction;

    // Future: Subscribe to Quest Manager state changes here
    // GameEventsManager.Instance.questEvents.OnQuestStateChange += UpdateBubbleState;
  }

  private void OnDisable()
  {
    interactable.OnDefaultAction -= NotifyInteraction;
    // GameEventsManager.Instance.questEvents.OnQuestStateChange -= UpdateBubbleState;
  }

  private void Start()
  {
    // For testing: immediately show the bubble. 
    // Later, this will be driven by the current Quest State.
    ShowBubble(true);
  }

  private void NotifyInteraction()
  {
    if (string.IsNullOrEmpty(targetID)) return;
    GameEventsManager.Instance.interactEvents.Interact(targetID);

    // Hide the bubble once interacted with
    ShowBubble(false);
  }

  // Modular function to handle the UI indicator
  public void ShowBubble(bool show)
  {
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