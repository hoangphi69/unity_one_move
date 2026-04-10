using FMODUnity;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum CameraMode { A, B }

public class StageManager : MonoBehaviour
{
  [SerializeField] private CinemachineCamera cameraA;
  [SerializeField] private CinemachineCamera cameraB;
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
    SwitchCamera(GameplayManager.Instance.cameraMode);
    GameplayManager.Instance.OnCameraSwitch += SwitchCamera;

    HUDOverlayUIController.Instance.SetStageName(stageName);
    HUDOverlayUIController.Instance.ToggleBottom(isPuzzle);
    HUDOverlayUIController.Instance.SetStepLeft(maxStep);

    if (!ambienceTrack.IsNull) GameAudioManagger.Instance.PlayAmbience(ambienceTrack);
  }

  void OnDisable()
  {
    GameplayManager.Instance.OnCameraSwitch -= SwitchCamera;
    GameAudioManagger.Instance.StopAmbience();
  }

  public bool IsGround(Vector3 position)
  {
    if (environment == null) return true;
    Vector3Int cell = environment.WorldToCell(position);
    return environment.HasTile(cell);
  }

  void SwitchCamera(CameraMode mode)
  {
    if (cameraA == null || cameraB == null) return;
    switch (mode)
    {
      case CameraMode.A:
        cameraA.Priority = 2;
        cameraB.Priority = 1;
        break;
      case CameraMode.B:
        cameraA.Priority = 1;
        cameraB.Priority = 2;
        break;
    }
  }
}