using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(Collide))]
public class CollideLock : MonoBehaviour, ILocker
{
  [Header("Locked Interaction")]
  [SerializeField] private string lockedDialogue;

  [SerializeField] private bool bubble = false;
  [Tooltip("If true, the lock bubble won't show until the player tries to collide it at least once.")]
  [ShowIf("bubble")][SerializeField] private bool hideFirstTime = true;
  [ShowIf("bubble")][SerializeField] private GameObject bubblePrefab;
  [ShowIf("bubble")][SerializeField] private Transform bubbleSpawnPoint;

  private GameObject currentBubbleInstance;
  private Collide collide;

  void Awake()
  {
    collide = GetComponent<Collide>();
  }

  void OnEnable()
  {
    collide.isLocked = true;
    collide.OnLockAction += OnCollided;
  }

  void OnDisable()
  {
    collide.OnLockAction -= OnCollided;
  }

  void Start()
  {
    if (hideFirstTime) ShowBubble(false);
  }

  void OnCollided()
  {
    GameEventsManager.Instance.dialogueEvents.EnterDialogue(lockedDialogue, DialogueMode.InGame);
    if (hideFirstTime) ShowBubble(true);
  }

  public void Unlock()
  {
    collide.isLocked = false;
    ShowBubble(false);
    Destroy(this);
  }

  private void ShowBubble(bool show)
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