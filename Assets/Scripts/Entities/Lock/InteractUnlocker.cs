using UnityEngine;

[RequireComponent(typeof(ILocker))]
public class InteractUnlocker : MonoBehaviour
{
  [SerializeField] private string targetID;
  private ILocker locker;

  void Awake()
  {
    locker = GetComponent<ILocker>();
  }

  void OnEnable()
  {
    GameEventsManager.Instance.interactEvents.OnInteract += Unlock;
  }

  void OnDisable()
  {
    GameEventsManager.Instance.interactEvents.OnInteract -= Unlock;
  }

  void Unlock(string id)
  {
    if (id != targetID) return;

    Destroy(this);
    locker.Unlock();
  }
}