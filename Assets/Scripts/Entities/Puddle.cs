using System.Threading;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(SpriteRenderer), typeof(TrailRenderer))]
public class Puddle : MonoBehaviour
{
  [Header("Settings")]
  [SerializeField] private float fadeDuration = 1.5f;
  [SerializeField] private float heightOffset = 0.02f;

  private SpriteRenderer spriteRenderer;
  private Collider col;
  private TrailRenderer trail;

  void Awake()
  {
    spriteRenderer = GetComponent<SpriteRenderer>();
    col = GetComponent<Collider>();
    trail = GetComponent<TrailRenderer>();

    // Ensure trail doesn't start drawing before the player touches it
    trail.emitting = false;
  }

  public void AttachTrailTo(Transform parent)
  {
    spriteRenderer.enabled = false;
    col.enabled = false;

    transform.SetParent(parent);
    transform.localPosition = new Vector3(0, heightOffset, 0);

    trail.emitting = true;
  }

  public void Evaporate()
  {
    transform.SetParent(null);
    trail.emitting = false;

    Tween fadeTween = trail.material
      .DOFade(0f, fadeDuration)
      .OnComplete(() => { if (gameObject != null) Destroy(gameObject); });
  }
}