using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Transform pivot;

    private float cellSize = 1f;

    private IInteractable nearbyInteractable;

    [SerializeField] private float speed = 10f;
    [SerializeField] private LayerMask collectibleMask;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private LayerMask interactableMask;

    void Awake()
    {
        GameObject point = new("MovePoint");
        pivot = point.transform;
        pivot.position = transform.position;
    }

    void OnEnable()
    {
        InputActionsManager.Instance.inputActions.Player.Move.performed += MovePivot;
        InputActionsManager.Instance.inputActions.Player.Interact.started += Interact;
    }

    void OnDisable()
    {
        InputActionsManager.Instance.inputActions.Player.Move.performed -= MovePivot;
        InputActionsManager.Instance.inputActions.Player.Interact.started -= Interact;
    }

    void MovePivot(InputAction.CallbackContext ctx)
    {
        Vector2 input = ctx.ReadValue<Vector2>();
        // Busy check
        if (Vector3.Distance(transform.position, pivot.position) > 0.05f) return;
        if (input.sqrMagnitude < 0.1f) return;

        // Pre-process input (exclude diagonal movement)
        Vector3 direction;
        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            direction = new Vector3(Mathf.Sign(input.x), 0, 0);
        else
            direction = new Vector3(0, 0, Mathf.Sign(input.y));

        transform.rotation = Quaternion.LookRotation(direction);

        // Interact with objects
        if (!CanMove(direction)) return;

        // Move pivot
        pivot.position += direction;

        // Look for interactibles
        ScanSurroundings();
    }

    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, pivot.position, speed * Time.fixedDeltaTime);
    }

    bool CanMove(Vector3 direction)
    {
        Vector3 position = transform.position;

        if (Physics.Raycast(position, direction, cellSize, collectibleMask)) return true;
        else if (Physics.Raycast(position, direction, cellSize, interactableMask)) return false;
        else if (Physics.Raycast(position, direction, cellSize, obstacleMask)) return false;

        return true;
    }

    void ScanSurroundings()
    {
        IInteractable found = null;

        // Look for interactibles in 4 directions
        Vector3[] directions = { Vector3.forward, Vector3.left, Vector3.right, Vector3.back };
        foreach (Vector3 direction in directions)
        {
            if (Physics.Raycast(pivot.position, direction, out RaycastHit hit, cellSize))
            {
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
    }

    void Interact(InputAction.CallbackContext context)
    {
        if (!context.started || nearbyInteractable == null) return;

        // Face the object
        Vector3 dir = nearbyInteractable.GetPosition() - transform.position;
        dir.y = 0; // Keep the player upright
        transform.rotation = Quaternion.LookRotation(dir);

        nearbyInteractable.OnInteract();
    }
}