using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NaughtyAttributes;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Grid")]
    [SerializeField] private Sprite playerTile;
    [SerializeField] private float heightOffset = 0.02f;

    // Configs
    [Header("Debug")]
    [SerializeField] private bool godMode = false;
    [SerializeField] private bool showRaycast = false;
    [SerializeField] private bool showTrail = false;
    [ShowIf("showTrail")][SerializeField] private int maxTrails = 1; // Configurable history limit

    private float moveDuration = .2f;
    private bool isMoving = false;

    private float raycastHeight = .3f;

    private Interactable nearbyInteractable;

    private Animator animator;

    private GameObject indicatorTile;

    [SerializeField] private bool onSkipTile = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
        CreateIndicatorTile();
    }

    void OnEnable()
    {
        GameInputManager.Instance.Actions.Player.Move.performed += TakeTurn;
        GameInputManager.Instance.Actions.Player.Interact.started += Interact;
        GameEventsManager.Instance.turnEvents.onStageRestart += ArchiveTrail;
    }

    void OnDisable()
    {
        GameInputManager.Instance.Actions.Player.Move.performed -= TakeTurn;
        GameInputManager.Instance.Actions.Player.Interact.started -= Interact;
        GameEventsManager.Instance.turnEvents.onStageRestart -= ArchiveTrail;
    }

    void CreateIndicatorTile()
    {
        GameObject indicator = new("indicator");

        indicator.transform.SetParent(transform);
        indicator.transform.localPosition = new Vector3(0, heightOffset, 0);
        indicator.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        indicator.transform.localScale = new Vector3(.9f, .9f, 1f);

        indicatorTile = indicator;

        if (playerTile == null) return;
        SpriteRenderer sr = indicator.AddComponent<SpriteRenderer>();
        sr.sprite = playerTile;
    }

    async void TakeTurn(InputAction.CallbackContext ctx)
    {
        // Compute input -> direction
        Vector2 input = ctx.ReadValue<Vector2>();
        if (input.sqrMagnitude < 0.1f) return;

        // Exclude diagonal movement
        Vector3 direction;
        bool x = Mathf.Abs(input.x) > Mathf.Abs(input.y);
        if (x) direction = new Vector3(Mathf.Sign(input.x), 0, 0);
        else direction = new Vector3(0, 0, Mathf.Sign(input.y));

        // Busy check
        var stage = GameplayManager.Instance.Stage;
        if (stage.turn != Turn.Player) return;
        if (isMoving) return;

        // Check steps left
        if (stage is { isPuzzle: true, stepLeft: 0 })
        { await Die(direction, shake: false); return; }

        // Actions
        Rotate(direction);

        if (!CanMove(direction))
        { animator.CrossFade("move", .1f, 0, 0f); return; }

        await ProcessMove(direction);

        ScanSurroundings();
    }

    async Task ProcessMove(Vector3 direction)
    {
        animator.CrossFade("move", .1f, 0, 0f);

        // Calculate distance
        Vector3 location;
        if (OnPuddle(out Puddle puddle)) location = GetSlideDestination(direction);
        else location = transform.position + (direction * GameplayManager.Instance.cellSize);

        PathTrailManager.AddStep(transform.position, location, raycastHeight, showTrail); // DEBUG TRAIL

        Task move = Move(location, destroyCancellationToken);
        GameEventsManager.Instance.turnEvents.PlayerTurnEnd(move);
        await move;

        if (puddle != null) puddle.DetachAndFade(destroyCancellationToken);
    }

    private bool OnPuddle(out Puddle activePuddle)
    {
        activePuddle = null;

        Vector3 origin = transform.position + (Vector3.up * raycastHeight);

        // Raycast straight down
        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, raycastHeight + 0.5f))
        {
            if (hit.collider.TryGetComponent(out Puddle puddle))
            {
                puddle.AttachTrailTo(transform);
                activePuddle = puddle;
                return true;
            }
        }

        return false;
    }

    Vector3 GetSlideDestination(Vector3 direction)
    {
        Vector3 currentSimulatedPos = transform.position;
        float cellSize = GameplayManager.Instance.cellSize;

        // Failsafe to prevent infinite loops in case of level design errors
        int maxSlideDistance = 20;

        for (int i = 0; i < maxSlideDistance; i++)
        {
            Vector3 nextSimulatedPos = currentSimulatedPos + (direction * cellSize);

            // 1. Check if the next tile is actually ground
            if (!GameplayManager.Instance.Stage.IsGround(nextSimulatedPos))
            {
                break; // Stop at the current position
            }

            // 2. Check for blocking entities ahead
            Vector3 rayOrigin = currentSimulatedPos + Vector3.up * raycastHeight;
            if (Physics.Raycast(rayOrigin, direction, out RaycastHit hit, cellSize, GameplayManager.Instance.entityMask))
            {
                if (hit.collider.TryGetComponent(out Collide collide))
                    break; // Blocked by a locked entity

                if (hit.collider.TryGetComponent(out Obstacle obstacle) && obstacle.BlockPlayer)
                    break; // Blocked by an obstacle
            }

            // If we didn't break, the next cell is safe. Advance our simulated position.
            currentSimulatedPos = nextSimulatedPos;
        }

        return currentSimulatedPos;
    }

    bool CanMove(Vector3 direction)
    {
        if (!GameplayManager.Instance.Stage.IsGround(transform.position + direction)) return false;

        // --- DEBUG RAYCAST ---
        Vector3 origins = transform.position + Vector3.up * raycastHeight;
        DrawGameLine(origins, direction, Color.yellow, 2f);

        if (Physics.Raycast(origins, direction, out RaycastHit hit, GameplayManager.Instance.cellSize, GameplayManager.Instance.entityMask))
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

    async Task Move(Vector3 location, CancellationToken token)
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
        GameAudioManagger.Instance.PlaySFX(FMODEvents.Instance.Footstep, location);
        transform.position = location;

        isMoving = false;
    }

    void Rotate(Vector3 direction)
    {
        transform.rotation = Quaternion.LookRotation(direction);
        indicatorTile.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }

    void ScanSurroundings()
    {
        Interactable found = null;

        // Look for interactibles in 4 directions
        Vector3 origins = transform.position + Vector3.up * raycastHeight;
        Vector3[] directions = { Vector3.forward, Vector3.left, Vector3.right, Vector3.back };
        foreach (Vector3 direction in directions)
        {
            // --- DEBUG RAYCAST ---
            DrawGameLine(origins, direction, Color.cyan, 0.5f);

            if (Physics.Raycast(origins, direction, out RaycastHit hit, GameplayManager.Instance.cellSize, GameplayManager.Instance.entityMask))
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
        if (godMode) return;
        GameInputManager.Instance.SetState(InputState.None);

        HUDOverlayUIController.Instance.SetStepLeft(-1);

        // Shake camera
        if (shake)
        {
            var bumped = GetComponent<CinemachineImpulseSource>();
            float bumpedDirection = direction.z != 0 ? direction.z : -direction.x;
            bumped.DefaultVelocity = new Vector3(bumpedDirection, 1f, 0f);
            bumped.GenerateImpulse(.1f);
        }

        // --- SAVE TRAIL BEFORE RESTART ---
        PathTrailManager.ArchiveCurrentTrail(maxTrails);

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

    // --- HELPER METHOD TO DRAW TEMPORARY LINES ---
    void DrawGameLine(Vector3 start, Vector3 direction, Color color, float duration)
    {
        if (!showRaycast) return;

        Vector3 end = start + (direction * GameplayManager.Instance.cellSize);

        GameObject myLine = new GameObject("DebugLine");
        myLine.transform.position = start;

        LineRenderer lr = myLine.AddComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.sortingLayerName = "Default";
        lr.sortingOrder = 1;
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = 0.05f;
        lr.endWidth = 0.05f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);

        Destroy(myLine, duration);
    }

    void ArchiveTrail()
    {
        PathTrailManager.ArchiveCurrentTrail(maxTrails);
    }
}

// ====================================================================
// --- SEPARATE LOGIC: Manages persistent debug trails across restarts ---
// ====================================================================
public static class PathTrailManager
{
    private static Queue<GameObject> historyQueue = new Queue<GameObject>();
    private static LineRenderer currentTrail;

    private static int stepCount = 0;
    private static int colorIndex = 0;

    // Cycle through distinct colors for each new attempt
    private static readonly Color[] trailColors = {
        Color.magenta,
        Color.cyan,
        Color.green,
        new Color(1f, 0.5f, 0f), // Orange
        Color.red,
    };

    public static void AddStep(Vector3 startPosition, Vector3 newLocation, float heightOffset, bool showLine)
    {
        if (!showLine) return;

        // Initialize a new trail if one doesn't exist
        if (currentTrail == null)
        {
            GameObject trailObj = new GameObject("Current trail");

            currentTrail = trailObj.AddComponent<LineRenderer>();
            currentTrail.material = new Material(Shader.Find("Sprites/Default"));

            currentTrail.sortingLayerName = "Default";
            currentTrail.sortingOrder = 1;

            Color currentColor = trailColors[colorIndex % trailColors.Length];
            currentTrail.startColor = currentColor;
            currentTrail.endColor = currentColor;
            currentTrail.startWidth = 0.05f;
            currentTrail.endWidth = 0.05f;

            currentTrail.positionCount = 1;
            currentTrail.SetPosition(0, startPosition + (Vector3.up * heightOffset));
            stepCount = 0;
        }

        // Extend the trail and increment steps
        stepCount++;
        currentTrail.positionCount++;
        currentTrail.SetPosition(currentTrail.positionCount - 1, newLocation + (Vector3.up * heightOffset));
    }

    public static void ArchiveCurrentTrail(int maxHistory)
    {
        if (currentTrail != null)
        {
            // Rename the trail to store the step count visually in the hierarchy
            currentTrail.gameObject.name = $"Previous trail ({stepCount} steps)";

            // Add to history
            historyQueue.Enqueue(currentTrail.gameObject);

            // Clean up the oldest trail if we exceed the allowed maximum
            while (historyQueue.Count > maxHistory)
            {
                GameObject oldestTrail = historyQueue.Dequeue();
                if (oldestTrail != null)
                {
                    Object.Destroy(oldestTrail);
                }
            }

            // Reset variables for the next run
            currentTrail = null;
            colorIndex++;
        }
    }
}