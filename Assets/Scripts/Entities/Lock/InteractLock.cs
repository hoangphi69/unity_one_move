using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class InteractLock : MonoBehaviour, ILocker
{
  [Header("Locked Interaction")]
  [SerializeField] private string lockedDialogue;

  [SerializeField] private bool bubble = true;
  [Tooltip("If true, the lock bubble won't show until the player tries to interact with it at least once.")]
  [ShowIf("bubble")][SerializeField] private bool hideFirstTime = true;
  [ShowIf("bubble")][SerializeField] private GameObject bubblePrefab;
  [ShowIf("bubble")][SerializeField] private Transform bubbleSpawnPoint;

  private GameObject currentBubbleInstance;
  private Interactable interactable;

  void Awake()
  {
    interactable = GetComponent<Interactable>();
  }

  void OnEnable()
  {
    interactable.isLocked = true;
    interactable.OnLockAction += OnInteract;
  }

  void OnDisable()
  {
    interactable.OnLockAction -= OnInteract;
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
    if (!bubble) return;
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