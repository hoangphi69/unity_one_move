using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
  // Configs
  [SerializeField] float moveDuration = .2f;
  [SerializeField] float sightDistance = 10f;
  private float raycastHeight = .3f;

  [Header("Grid")]
  private string tileAsset = "Sprites/Icon/tile_enemy";
  [SerializeField] private float heightOffset = 0.03f;
  private GameObject indicatorTile;

  private bool isMoving = false;
  private Vector3[] sightDirections = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };

  void Awake()
  {
    CreateIndicatorTile();
  }

  void Start()
  {
    GameplayManager.Instance.Stage.RegisterEnemy(this);
  }

  void OnDestroy()
  {
    GameplayManager.Instance.Stage.UnregisterEnemy(this);
  }

  public async Task TakeTurnAsync()
  {
    Vector3? direction = ScanForPlayer();
    if (!direction.HasValue) return;
    Rotate(direction.Value);
    await TryMove(direction.Value);
  }

  Vector3? ScanForPlayer()
  {
    foreach (Vector3 direction in sightDirections)
    {
      RaycastHit[] hits = Physics.RaycastAll
      (
        transform.position + Vector3.up * raycastHeight,
        direction,
        sightDistance,
        GameplayManager.Instance.entityMask
      );

      System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
      foreach (RaycastHit hit in hits)
      {
        if (hit.collider.TryGetComponent(out EnemyController _)) break;
        if (hit.collider.TryGetComponent(out Obstacle obstacle))
        {
          if (obstacle.BlockEnemySight) break;
          else continue;
        }

        if (hit.collider.TryGetComponent(out PlayerController _))
        {
          return direction;
        }
      }
    }
    return null;
  }

  void CreateIndicatorTile()
  {
    GameObject indicator = new("indicator");

    indicator.transform.SetParent(transform);
    indicator.transform.localPosition = new Vector3(0, heightOffset, 0);
    indicator.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    indicator.transform.localScale = new Vector3(.9f, .9f, 1f);

    indicatorTile = indicator;

    SpriteRenderer sr = indicator.AddComponent<SpriteRenderer>();
    sr.sprite = Resources.Load<Sprite>(tileAsset);
  }

  void Rotate(Vector3 direction)
  {
    transform.rotation = Quaternion.LookRotation(direction);
    indicatorTile.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
  }

  async Task TryMove(Vector3 direction)
  {
    if (isMoving) return;
    if (!CanMove(direction)) return;

    Vector3 location = transform.position + (direction * GameplayManager.Instance.cellSize);
    GameAudioManager.Instance.PlaySFX(AudioTag.step);
    await SmoothMoveAsync(location, destroyCancellationToken);
  }

  async Task SmoothMoveAsync(Vector3 location, CancellationToken token)
  {
    isMoving = true;

    float elapsedTime = 0f;
    while (elapsedTime < moveDuration)
    {
      if (token.IsCancellationRequested) return;
      transform.position = Vector3.Lerp(transform.position, location, elapsedTime / moveDuration);
      elapsedTime += Time.deltaTime;
      await Task.Yield();
    }

    if (token.IsCancellationRequested) return;
    transform.position = location;
    isMoving = false;
  }

  bool CanMove(Vector3 direction)
  {
    Vector3 position = transform.position;

    if (!GameplayManager.Instance.Stage.IsGround(position + direction)) return false;

    if (Physics.Raycast(position + Vector3.up * raycastHeight, direction, out RaycastHit hit, GameplayManager.Instance.cellSize, GameplayManager.Instance.entityMask))
    {
      if (hit.collider.TryGetComponent(out Obstacle obstacle))
      {
        return !obstacle.BlockEnemy;
      }

      if (hit.collider.TryGetComponent(out PlayerController player))
      {
        _ = player.Die(direction);
      }
    }

    return true;
  }
}