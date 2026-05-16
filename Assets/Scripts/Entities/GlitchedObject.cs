using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class GlitchedObject : MonoBehaviour
{
  [SerializeField] private string id;
  [SerializeField] private GameObject doorPrefab;
  [SerializeField] private float animationDuration = 1.5f;

  private Animator animator;
  private bool isTransforming = false;

  void Awake()
  {
    animator = GetComponent<Animator>();
  }

  void OnEnable()
  {
    GameEventsManager.Instance.interactEvents.OnSwitch += HandleSwitchEvent;
  }

  void OnDisable()
  {
    GameEventsManager.Instance.interactEvents.OnSwitch -= HandleSwitchEvent;
  }

  private async void HandleSwitchEvent(string switchId, bool state)
  {
    // Ignore if it's the wrong switch, if the switch was turned off, or if we are already transforming
    if (switchId != id || !state || isTransforming) return;

    await TransformIntoDoor();
  }

  private async Task TransformIntoDoor()
  {
    isTransforming = true;

    // 1. Play the glitch fixing animation
    animator.Play("Fix");

    // 2. Wait for the animation to finish. Task.Delay takes milliseconds.
    await Task.Delay(Mathf.RoundToInt(animationDuration * 1000));

    // Safety check: Make sure this object wasn't destroyed while we were waiting (e.g., player quit the level)
    if (this == null || !gameObject) return;

    // 3. Spawn the Gateway/Door prefab at exactly the same position and rotation
    if (doorPrefab != null)
    {
      Instantiate(doorPrefab, transform.position, transform.rotation);
    }
    else
    {
      Debug.LogWarning($"[GlitchedObject] No Door Prefab assigned on {gameObject.name}!");
    }

    // 4. Destroy this glitched object so only the door remains
    Destroy(gameObject);
  }
}