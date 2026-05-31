using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class GlitchRemover : MonoBehaviour
{
  [SerializeField] private string id;

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
    if (triggered) return;
    triggered = true;

    HandleAnimation();

    GameEventsManager.Instance.interactEvents.Switch(id, true);
  }

  void HandleAnimation()
  {
    if (animator == null) return;
    animator.CrossFade("On", .1f);
  }
}