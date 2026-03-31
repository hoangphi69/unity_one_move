using UnityEngine;

public class Collectible : MonoBehaviour
{
  void OnTriggerEnter(Collider collider)
  {
    if (collider.gameObject.tag == "Player")
    {
      Destroy(gameObject);
    }
  }
}