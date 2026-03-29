using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class LockedPoint : MonoBehaviour
{
  [Header("Quest Configuration")]
  [SerializeField] private string questID;

  [Header("Locked Interaction")]
  [SerializeField] private string lockedDialogue;

  [Header("Visual Indicators")]
  [Tooltip("If true, the lock bubble won't show until the player tries to interact with it at least once.")]
  [SerializeField] private bool hideFirstTime = true;
  [SerializeField] private GameObject bubblePrefab; // Lock icon canvas prefab with Billboard
  [SerializeField] private Transform bubbleSpawnPoint;

  private GameObject currentBubbleInstance;
  private Interactable interactable;
  private bool hasInteracted = false;

  private void Awake()
  {
    interactable = GetComponent<Interactable>();
  }

  private void OnEnable()
  {
    interactable.OnLockInteract += OnInteract;
    GameEventsManager.Instance.questEvents.OnQuestStateChanged += QuestStateChange;
  }

  private void OnDisable()
  {
    interactable.OnLockInteract -= OnInteract;
    GameEventsManager.Instance.questEvents.OnQuestStateChanged -= QuestStateChange;
  }

  private void Start()
  {
    QuestState currentState = GameQuestManager.Instance.GetQuestState(questID);
    UpdateLockState(currentState);
  }

  void OnInteract()
  {
    hasInteracted = true;
    GameEventsManager.Instance.dialogueEvents.EnterDialogue(lockedDialogue, DialogueMode.InGame);

    // Re-evaluate the bubble. If it was hidden by 'hideFirstTime', it should pop up now.
    QuestState currentState = GameQuestManager.Instance.GetQuestState(questID);
    EvaluateBubbleVisibility(currentState);
  }

  private void QuestStateChange(Quest quest)
  {
    if (quest.info.id != questID) return;
    UpdateLockState(quest.state);
  }

  void UpdateLockState(QuestState state)
  {
    if (state == QuestState.COMPLETED)
    {
      interactable.isLocked = false;
      ShowBubble(false);
      Destroy(this);
    }
    else
    {
      interactable.isLocked = true;
      EvaluateBubbleVisibility(state);
    }
  }

  private void EvaluateBubbleVisibility(QuestState state)
  {
    bool isInProgress = state == QuestState.IN_PROGRESS;

    if (isInProgress)
    {
      // Show the bubble UNLESS 'hideFirstTime' is active and we haven't clicked it yet
      if (hideFirstTime && !hasInteracted) ShowBubble(false);
      else ShowBubble(true);
    }
    // Hide the lock bubble if the quest hasn't reached InProgress yet
    else ShowBubble(false);
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