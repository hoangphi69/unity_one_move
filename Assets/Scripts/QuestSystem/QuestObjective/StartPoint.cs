using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class StartPoint : MonoBehaviour
{
  [Header("Quest Configuration")]
  [SerializeField] private string questID;

  [Header("Visual Indicators")]
  [SerializeField] private GameObject bubblePrefab;
  [SerializeField] private Transform bubbleSpawnPoint;

  private GameObject currentBubbleInstance;
  private Interactable interactable;

  private void Awake()
  {
    interactable = GetComponent<Interactable>();
  }

  private void OnEnable()
  {
    // 1. Listen for the physical click from the Interactable script
    interactable.OnInteracted += HandleInteraction;

    // 2. Listen to global quest state changes
    // (Uncomment and adjust to match your actual Event Manager syntax)
    // GameEventsManager.Instance.questEvents.OnQuestStateChange += QuestStateChange;
  }

  private void OnDisable()
  {
    interactable.OnInteracted -= HandleInteraction;

    // GameEventsManager.Instance.questEvents.OnQuestStateChange -= QuestStateChange;
  }

  private void Start()
  {
    // Check the initial state of the quest as soon as this object loads in the scene.
    // (Adjust the manager call to match your actual GameQuestManager)

    // Example:
    // QuestState currentState = GameQuestManager.Instance.GetQuestState(questIDToStart);
    // UpdateBasedOnState(currentState);
  }

  private void HandleInteraction()
  {
    // When clicked, tell the global manager to start this quest!
    // The GameQuestManager should hear this, change the state to ACTIVE, 
    // and broadcast the OnQuestStateChange event.

    // Example:
    // GameEventsManager.Instance.questEvents.StartQuest(questIDToStart);
  }

  private void QuestStateChange(string changedQuestID, /* QuestState */ int newState) // Replace 'int' with your QuestState enum
  {
    // Only react if the quest that just changed is the one THIS point cares about
    if (changedQuestID == questID)
    {
      UpdateBasedOnState(newState);
    }
  }

  private void UpdateBasedOnState(/* QuestState */ int state) // Replace 'int' with your QuestState enum
  {
    // Assuming your enum has a state like 'Available'
    // if (state == QuestState.Available)
    if (state == 0) // Placeholder for 'Available'
    {
      ShowBubble(true);
    }
    else
    {
      // If the state is Active, Completed, or anything else, this StartPoint is obsolete.
      ShowBubble(false);

      // Destroy THIS script component, leaving the NPC and its Interactable script intact.
      Destroy(this);
    }
  }

  private void ShowBubble(bool show)
  {
    if (show && currentBubbleInstance == null && bubblePrefab != null)
    {
      currentBubbleInstance = Instantiate(bubblePrefab, bubbleSpawnPoint.position, Quaternion.identity, transform);
    }
    else if (!show && currentBubbleInstance != null)
    {
      Destroy(currentBubbleInstance);
    }
  }
}