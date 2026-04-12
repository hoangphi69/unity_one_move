using System;
using System.Threading;
using System.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Configs
    [SerializeField] float moveDuration = .2f;

    private bool isMoving = false;
    private Interactable nearbyInteractable;
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

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
        // Compute input -> direction
        Vector2 input = ctx.ReadValue<Vector2>();
        if (input.sqrMagnitude < 0.1f) return;
        // Exclude diagonal movement
        Vector3 direction;
        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            direction = new Vector3(Mathf.Sign(input.x), 0, 0);
        else
            direction = new Vector3(0, 0, Mathf.Sign(input.y));

        // Busy check
        if (GameplayManager.Instance.Stage.turn != Turn.Player) return;
        if (GameplayManager.Instance.Stage.isPuzzle && GameplayManager.Instance.Stage.stepLeft == 0)
        {
            await Die(direction, false);
            return;
        }

        Rotate(direction);
        await TryMove(direction);
        ScanSurroundings();
    }

    void Rotate(Vector3 direction)
    {
        transform.rotation = Quaternion.LookRotation(direction);
    }

    async Task TryMove(Vector3 direction)
    {
        if (isMoving) return;

        animator.CrossFade("move", .1f, 0, 0f);

        if (!CanMove(direction)) return;

        GameAudioManagger.Instance.PlaySFX(FMODEvents.Instance.Footstep, transform.position);

        Vector3 location = transform.position + (direction * GameplayManager.Instance.cellSize);
        Task move = SmoothMoveAsync(location, destroyCancellationToken);

        GameEventsManager.Instance.turnEvents.PlayerTurnEnd(move);

        await move;
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

        if (Physics.Raycast(position, direction, out RaycastHit hit, GameplayManager.Instance.cellSize, GameplayManager.Instance.entityMask))
        {
            if (hit.collider.TryGetComponent(out Collide collide))
            {
                collide.OnCollide(direction);
                if (collide.isLocked) return false;
                else return true;
            }

            if (hit.collider.TryGetComponent(out Obstacle obstacle))
            {
                return !obstacle.BlockPlayer;
            }
        }

        return true;
    }

    void ScanSurroundings()
    {
        Interactable found = null;

        // Look for interactibles in 4 directions
        Vector3[] directions = { Vector3.forward, Vector3.left, Vector3.right, Vector3.back };
        foreach (Vector3 direction in directions)
        {
            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, GameplayManager.Instance.cellSize, GameplayManager.Instance.entityMask))
            {
                if (hit.transform.TryGetComponent(out Interactable interactable))
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

    public async Task Die(Vector3 direction, bool shake = true)
    {
        GameInputManager.Instance.SetState(InputState.None);

        // Shake camera
        if (shake)
        {
            var bumped = GetComponent<CinemachineImpulseSource>();
            float bumpedDirection = direction.z != 0 ? direction.z : -direction.x;
            bumped.DefaultVelocity = new Vector3(bumpedDirection, 1f, 0f);
            bumped.GenerateImpulse(.1f);
        }

        // Collapse animation
        Rotate(-direction);
        animator.CrossFade("fall_back", .1f, 0);
        await Task.Delay(1000);

        GameEventsManager.Instance.turnEvents.RestartStage();
    }

    public async Task PlayMusic()
    {
        GameInputManager.Instance.SetState(InputState.None);

        animator.CrossFade("wear_headphone", .1f, 1);
        await Task.Delay(300);
        GameAudioManagger.Instance.PlaySFX(FMODEvents.Instance.RadioToggle, transform.position);
    }

    public async Task LowerMusic()
    {
        GameInputManager.Instance.SetState(InputState.None);

        animator.CrossFade("remove_headphone", .1f, 1);
        GameAudioManagger.Instance.PlaySFX(FMODEvents.Instance.RadioToggle, transform.position);
        await Task.Delay(300);
    }

    public async Task StopMusic()
    {
        GameInputManager.Instance.SetState(InputState.None);

        animator.CrossFade("remove_headphone", .1f, 1);
        GameAudioManagger.Instance.PlaySFX(FMODEvents.Instance.RadioToggle, transform.position);
        await Task.Delay(300);
    }
}