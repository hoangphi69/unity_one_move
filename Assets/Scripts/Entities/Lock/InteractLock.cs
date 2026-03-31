using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class InteractLock : MonoBehaviour, ILocker
{
  [Header("Locked Interaction")]
  [SerializeField] private string lockedDialogue;

  [Header("Visual Indicators")]
  [Tooltip("If true, the lock bubble won't show until the player tries to interact with it at least once.")]
  [SerializeField] private bool hideFirstTime = true;
  [SerializeField] private GameObject bubblePrefab;
  [SerializeField] private Transform bubbleSpawnPoint;

  private GameObject currentBubbleInstance;
  private Interactable interactable;

  void Awake()
  {
    interactable = GetComponent<Interactable>();
    interactable.isLocked = true;
  }

  void OnEnable()
  {
    interactable.OnLockInteract += OnInteract;
  }

  void OnDisable()
  {
    interactable.OnLockInteract -= OnInteract;
  }

  void Start()
  {
    if (hideFirstTime) ShowBubble(false);
  }

  void OnInteract()
  {
    GameEventsManager.Instance.dialogueEvents.EnterDialogue(lockedDialogue, DialogueMode.InGame);
    if (hideFirstTime) ShowBubble(true);
  }

  public void Unlock()
  {
    interactable.isLocked = false;
    ShowBubble(false);
    Destroy(this);
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