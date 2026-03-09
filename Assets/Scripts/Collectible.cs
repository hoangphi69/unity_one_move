using UnityEngine;

public class Collectible : MonoBehaviour
{
  [SerializeField] private int levelPoint = 1;
  [SerializeField] private string cutsceneName;
  [SerializeField] private SceneField scene;

  void OnTriggerEnter(Collider collider)
  {
    if (collider.gameObject.tag == "Player")
    {
      Destroy(gameObject);
    }
  }
}