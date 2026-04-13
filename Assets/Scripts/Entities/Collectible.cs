using UnityEngine;

[RequireComponent(typeof(Collide))]
public class Collectible : MonoBehaviour
{
  private Collide collide;

  void Awake()
  {
    collide = GetComponent<Collide>();
  }

  void OnEnable()
  {
    collide.OnCollided += Collect;
  }

  void OnDisable()
  {
    collide.OnCollided -= Collect;
  }

  void Collect(Vector3 direction)
  {
    Destroy(gameObject);
  }
}