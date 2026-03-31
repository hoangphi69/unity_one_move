using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyController : MonoBehaviour
{
  // Configs
  [SerializeField] float moveDuration = .2f;
  [SerializeField] float sightDistance = 10f;

  private bool isMoving = false;
  private Vector3[] sightDirections = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };

  void OnEnable()
  {
    GameplayManager.Instance.RegisterEnemy(this);
  }

  void OnDisable()
  {
    GameplayManager.Instance.UnregisterEnemy(this);
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
        transform.position,
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

  void Rotate(Vector3 direction)
  {
    transform.rotation = Quaternion.LookRotation(direction);
  }

  async Task TryMove(Vector3 direction)
  {
    if (isMoving) return;
    if (!CanMove(direction)) return;

    Vector3 location = transform.position + (direction * GameplayManager.Instance.cellSize);
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

    if (!GameplayManager.Instance.stageManager.IsGround(position + direction)) return false;

    if (Physics.Raycast(position, direction, out RaycastHit hit, GameplayManager.Instance.cellSize, GameplayManager.Instance.entityMask))
    {
      if (hit.collider.TryGetComponent(out Obstacle obstacle))
      {
        return !obstacle.BlockEnemy;
      }

      if (hit.collider.TryGetComponent(out PlayerController player))
      {
        _ = player.Die();
      }
    }

    return true;
  }
}