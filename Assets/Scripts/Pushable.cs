using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PushBox : MonoBehaviour, IObstacle, IPushable
{
    [SerializeField] float moveDuration = .2f;
    [SerializeField] bool isPushable = true;

    private bool isMoving = false;

    public bool IsSolid() => !isPushable;

    public async Task Push(Vector3 direction)
    {
        if (!isPushable) return;
        if (isMoving) return;

        Vector3 target = transform.position + direction * GameplayManager.Instance.cellSize;

        if (!CanPushTo(direction, target)) return;

        await SmoothMoveAsync(target, destroyCancellationToken);
    }

    bool CanPushTo(Vector3 direction, Vector3 target)
    {
        Tilemap ground = GameplayManager.Instance.stageManager.environment;
        if (ground != null)
        {
            Vector3Int cell = ground.WorldToCell(target);
            if (!ground.HasTile(cell)) return false;
        }

        if (Physics.Raycast(transform.position, direction,
            GameplayManager.Instance.cellSize,
            GameplayManager.Instance.entityMask))
        {
            return false;
        }

        return true;
    }

    async Task SmoothMoveAsync(Vector3 target, CancellationToken token)
    {
        isMoving = true;
        float elapsed = 0f;
        Vector3 start = transform.position;

        while (elapsed < moveDuration)
        {
            if (token.IsCancellationRequested) return;
            transform.position = Vector3.Lerp(start, target, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            await Task.Yield();
        }

        if (token.IsCancellationRequested) return;
        transform.position = target;
        isMoving = false;
    }
}