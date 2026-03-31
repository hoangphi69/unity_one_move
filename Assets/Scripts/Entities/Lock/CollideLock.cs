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
    collide.isLocked = true;
  }

  void OnEnable()
  {
    collide.OnLockedCollided += OnCollided;
  }

  void OnDisable()
  {
    collide.OnLockedCollided -= OnCollided;
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