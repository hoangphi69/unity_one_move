using UnityEngine;
using System.Threading.Tasks;

[RequireComponent(typeof(Collide))]
public class Pushable : MonoBehaviour
{
    [SerializeField] private float moveDuration = .2f;
    private Collide collide;
    private bool isSliding = false;

    private string tileAsset = "Sprites/Icon/tile_pushable";
    private float heightOffset = 0.02f;

    void Awake()
    {
        collide = GetComponent<Collide>();
        CreateIndicatorTile();
    }

    void OnEnable()
    {
        collide.OnCollided += TryMove;
    }

    void OnDisable()
    {
        collide.OnCollided -= TryMove;
    }

    void CreateIndicatorTile()
    {
        GameObject indicator = new("indicator");

        indicator.transform.SetParent(transform);
        indicator.transform.localPosition = new Vector3(0, heightOffset, 0);
        indicator.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        indicator.transform.localScale = new Vector3(.9f, .9f, 1f);

        SpriteRenderer sr = indicator.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>(tileAsset);
    }

    async void TryMove(Vector3 direction)
    {
        if (isSliding)
        {
            await BlockPlayer();
            return;
        }

        Vector3 location = transform.position + direction;

        if (!GameplayManager.Instance.Stage.IsGround(location))
        {
            await BlockPlayer();
            return;
        }

        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, GameplayManager.Instance.cellSize, GameplayManager.Instance.entityMask))
        {
            if (hit.collider.TryGetComponent(out Obstacle obstacle))
            {
                if (obstacle.BlockPlayer)
                {
                    await BlockPlayer();
                    return;
                }
            }
        }

        await Move(location);
    }

    async Task BlockPlayer()
    {
        collide.isLocked = true;
        await Task.Yield();
        collide.isLocked = false;
    }

    async Task Move(Vector3 location)
    {
        isSliding = true;

        float elapsedTime = 0;
        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(transform.position, location, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            await Task.Yield();
        }

        transform.position = location;
        isSliding = false;
    }
}