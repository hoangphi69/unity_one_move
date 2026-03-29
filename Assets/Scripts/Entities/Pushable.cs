using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pushable : MonoBehaviour, IObstacle, IPushable, IInteractable
{
    [Header("Player")]
    [SerializeField] bool blockPlayer = true;
    [SerializeField] bool isPushable = false;

    [Header("Enemy")]
    [SerializeField] bool blockEnemy = true;
    [SerializeField] bool blockEnemySight = true;

    [Header("Movement")]
    [SerializeField] float moveDuration = 0.2f;

    //IObstacle
    public bool IsPlayerBlocking() => blockPlayer;
    public bool IsEnemyBlocking() => blockEnemy;
    public bool IsEnemySightBlocking() => blockEnemySight;


    private bool isMoving = false;
    private Outline outline;

    void Awake()
    {
        outline = GetComponent<Outline>();
        if (outline != null) outline.enabled = false;
    }

    public async Task Push(Vector3 direction)
    {
        Vector3 target = transform.position + direction * GameplayManager.Instance.cellSize;
        await SmoothMoveAsync(target, destroyCancellationToken);
    }

    public bool CanPush(Vector3 direction)
    {
        if (!isPushable) return false;
        if (isMoving) return false;
        Vector3 target = transform.position + direction * GameplayManager.Instance.cellSize;
        return CanPushTo(direction, target);
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

    // IInteractable
    public void OnDetected()
    {
        if (outline != null) outline.enabled = true;
    }
    public void OnLost()
    {
        if (outline != null) outline.enabled = false;
    }
    public void OnInteract() { }
    public Vector3 GetPosition() => transform.position;
}