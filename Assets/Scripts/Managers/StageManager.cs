using FMODUnity;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StageManager : MonoBehaviour
{
  // Configs
  public string stageName;
  public Vector3 defaultPlayerPosition;
  public bool isPuzzle = false;
  public int maxStep;
  public EventReference ambienceTrack;
  public EventReference radioTrack;

  public Tilemap environment { get; private set; }

  void Awake()
  {
    environment = transform.Find("Environment").GetComponent<Tilemap>();
  }

  void OnEnable()
  {
    GameplayManager.Instance.RegisterStage(this);
    HUDOverlayUIController.Instance.SetStageName(stageName);
    HUDOverlayUIController.Instance.ToggleBottom(isPuzzle);
    HUDOverlayUIController.Instance.SetStepLeft(maxStep);
    if (!ambienceTrack.IsNull) GameAudioManagger.Instance.PlayAmbience(ambienceTrack);
  }

  public bool IsGround(Vector3 position)
  {
    if (environment == null) return true;
    Vector3Int cell = environment.WorldToCell(position);
    return environment.HasTile(cell);
  }
}