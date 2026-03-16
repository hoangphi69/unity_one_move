using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pushable : MonoBehaviour, IObstacle, IPushable
{
    [Header("Player")] 
    [SerializeField] bool blockPlayer = true;
    [SerializeField] bool isPushable = false;

    [Header("Enemy")]
    [SerializeField] bool blockEnemy = true;
    [SerializeField] bool blockEnemySight = true;

    [Header("Movement")]
    [SerializeField] float moveDuration = 0.1f;

    private bool isMoving = false;

    
    //IObstacle
    public bool IsPlayerBlocking() => blockPlayer;
    public bool IsEnemyBlocking() => blockEnemy;
    public bool IsEnemySightBlocking() => blockEnemySight;

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