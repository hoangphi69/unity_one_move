using UnityEngine;
using UnityEngine.Tilemaps;

public class StageManager : MonoBehaviour
{
  // Configs
  public Vector3 defaultPlayerPosition;
  public bool isPuzzle = false;

  public Tilemap environment { get; private set; }

  void Awake()
  {
    environment = transform.Find("Environment").GetComponent<Tilemap>();
  }

  void OnEnable()
  {
    GameplayManager.Instance.RegisterStage(this);
  }
}