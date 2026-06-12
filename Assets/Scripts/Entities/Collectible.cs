using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Outline))]
[RequireComponent(typeof(Collide))]
public class Collectible : MonoBehaviour
{
  public string id;

  // Idle animation
  private float floatOffset = 0.3f;
  private float floatDuration = 3f;

  // Collect animation
  private float collectOffset = 2f;
  private Ease collectEase = Ease.InOutBack;
  private float collectAnimDuration = 0.2f;

  // Flag to prevent multiple collisions while the destroy animation plays
  private bool isCollected = false;
  private Collide collide;


  void Awake()
  {
    collide = GetComponent<Collide>();
  }

  void Start()
  {
    transform.DOMove(transform.position + Vector3.up * floatOffset, floatDuration)
        .SetLoops(-1, LoopType.Yoyo)
        .SetEase(Ease.InOutSine);
  }

  void OnEnable()
  {
    collide.OnCollided += Collect;
  }

  void OnDisable()
  {
    collide.OnCollided -= Collect;
  }

  void Collect(Vector3 direction)
  {
    if (isCollected) return;
    isCollected = true;

    GameAudioManager.Instance.PlaySFX(AudioTag.collect);

    transform.DOKill();

    Sequence animation = DOTween.Sequence();
    animation
        .Join(transform.DOMove(transform.position + Vector3.up * collectOffset, collectAnimDuration).SetEase(collectEase))
        .Append(transform.DOScale(Vector3.zero, collectAnimDuration / 2).SetEase(Ease.InBack))
        .OnComplete(() => Destroy(gameObject));
  }

  void OnDestroy()
  {
    transform.DOKill();
  }
}