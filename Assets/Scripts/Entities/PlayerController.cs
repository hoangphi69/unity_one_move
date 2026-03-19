using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Configs
    [SerializeField] float moveDuration = .2f;

    private bool isMoving = false;
    private IInteractable nearbyInteractable;
    private OutlineHighlight currentHighlight;

    void OnEnable()
    {
        GameInputManager.Instance.Actions.Player.Move.performed += TakeTurn;
        GameInputManager.Instance.Actions.Player.Interact.started += Interact;
    }

    void OnDisable()
    {
        GameInputManager.Instance.Actions.Player.Move.performed -= TakeTurn;
        GameInputManager.Instance.Actions.Player.Interact.started -= Interact;
    }

    async void TakeTurn(InputAction.CallbackContext ctx)
    {
        Vector2 input = ctx.ReadValue<Vector2>();

        // Busy check
        if (GameplayManager.Instance.turn != Turn.Player) return;
        if (input.sqrMagnitude < 0.1f) return;

        // Pre-process input (exclude diagonal movement)
        Vector3 direction;
        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            direction = new Vector3(Mathf.Sign(input.x), 0, 0);
        else
            direction = new Vector3(0, 0, Mathf.Sign(input.y));

        Rotate(direction);
        await TryMove(direction);
        ScanSurroundings();
        GameEventsManager.Instance.turnEvents.PlayerTurnEnd();
    }

    void Rotate(Vector3 direction)
    {
        transform.rotation = Quaternion.LookRotation(direction);
    }

    async Task TryMove(Vector3 direction)
    {
        // Interact with objects
        if (isMoving) return;
        if (!await CanMove(direction)) return;

        // Move
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

    async Task<bool> CanMove(Vector3 direction)
    {
        Vector3 position = transform.position;

        if (!IsGround(position + direction)) return false;

        if (Physics.Raycast(position, direction, out RaycastHit hit, GameplayManager.Instance.cellSize, GameplayManager.Instance.entityMask))
        {
            if (hit.collider.TryGetComponent(out IPushable pushable))
            {
                return await pushable.Push(direction);
            }

            if (hit.collider.TryGetComponent(out IObstacle obstacle))
            {
                return !obstacle.IsPlayerBlocking();
            }

            if (hit.collider.TryGetComponent(out ICollectible collectible))
            {
                collectible.Collect();
                return true;
            }

            if (hit.collider.TryGetComponent(out IGateway gateway))
            {
                gateway.Transition();
                return true;
            }
        }

        return true;
    }

    bool IsGround(Vector3 position)
    {
        Tilemap ground = GameplayManager.Instance.stageManager.environment;
        if (ground == null) return true;
        Vector3Int cell = ground.WorldToCell(position);
        return ground.HasTile(cell);
    }

    void ScanSurroundings()
    {
        IInteractable found = null;
        // OutlineHighlight foundHighlight = null;

        // Look for interactibles in 4 directions
        Vector3[] directions = { Vector3.forward, Vector3.left, Vector3.right, Vector3.back };
        foreach (Vector3 direction in directions)
        {
            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, GameplayManager.Instance.cellSize, GameplayManager.Instance.entityMask))
            {
                // if (hit.transform.TryGetComponent(out OutlineHighlight highlight))
                // {
                //     foundHighlight = highlight;
                // }

                if (hit.transform.TryGetComponent(out IInteractable interactable))
                {
                    found = interactable;
                    break;
                }
            }
        }

        if (found != nearbyInteractable)
        {
            nearbyInteractable?.OnLost();
            nearbyInteractable = found;
            nearbyInteractable?.OnDetected();
        }

        // if (foundHighlight != currentHighlight)
        // {
        //     currentHighlight?.Hide(); 
        //     currentHighlight = foundHighlight;
        //     currentHighlight?.Show();
        // }
    }

    void Interact(InputAction.CallbackContext context)
    {
        if (!context.started || nearbyInteractable == null) return;

        // Face the object
        Vector3 direction = nearbyInteractable.GetPosition() - transform.position;
        direction.y = 0; // Keep the player upright
        Rotate(direction);

        nearbyInteractable.OnInteract();
    }

    public async Task Die()
    {
        print("Failed");
        await Task.Delay(300);
        await GameplayManager.Instance.RestartStageAsync();
    }
}