using UnityEngine;
using UnityEngine.Tilemaps;

public class EnvironmentManager : MonoBehaviour
{
  private Tilemap environment;

  void Awake()
  {
    environment = transform.Find("Environment").GetComponent<Tilemap>();
  }

  void OnEnable()
  {
    GameplayManager.Instance.RegisterEnvironment(environment);
  }
}