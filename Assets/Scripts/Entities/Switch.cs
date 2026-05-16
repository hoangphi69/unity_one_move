using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class Switch : MonoBehaviour
{
  [Header("Switch Settings")]
  [SerializeField] private string id;
  [SerializeField] private bool state = false;
  [SerializeField] private bool animated = true;
  [SerializeField] private bool oneTime = false;

  private Animator animator;
  private Interactable interactable;
  private bool triggered = false;

  void Awake()
  {
    interactable = GetComponent<Interactable>();
    animator = GetComponent<Animator>();
  }

  void OnEnable()
  {
    interactable.OnMainAction += OnInteract;
  }

  void OnDisable()
  {
    interactable.OnMainAction -= OnInteract;
  }

  void OnInteract()
  {
    if (oneTime && triggered) return;

    state = !state;
    triggered = true;

    HandleAnimation();

    GameEventsManager.Instance.interactEvents.Switch(id, state);
  }

  void HandleAnimation()
  {
    if (animator == null) return;

    string stateAnimation = state ? "On" : "Off";

    // Safety check in case you forgot to assign a clip in the inspector
    if (stateAnimation == null)
    {
      Debug.LogWarning($"[Switch] Missing animation clip on {gameObject.name}!");
      return;
    }

    if (animated) animator.CrossFade(stateAnimation, .1f);
    else animator.CrossFade(stateAnimation, 0);
  }
}