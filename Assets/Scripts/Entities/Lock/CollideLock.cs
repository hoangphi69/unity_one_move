using UnityEngine;

[RequireComponent(typeof(Collide))]
public class CollideLock : MonoBehaviour, ILocker
{
  [Header("Locked Interaction")]
  [SerializeField] private string lockedDialogue;

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

  void OnCollided()
  {
    GameEventsManager.Instance.dialogueEvents.EnterDialogue(lockedDialogue, DialogueMode.InGame);
  }

  public void Unlock()
  {
    collide.isLocked = false;
    Destroy(this);
  }
}