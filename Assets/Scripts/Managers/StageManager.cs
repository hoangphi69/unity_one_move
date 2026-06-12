using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FMODUnity;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum CameraMode { A, B }

public enum Turn { Player, Enemy };

public class StageManager : MonoBehaviour
{
  [Header("Generals")]
  public string stageName;
  public bool isPuzzle = false;
  public int maxStep;

  [Header("Map")]
  [SerializeField] private Tilemap tileMap;
  [field: SerializeField] public Transform SpawnPoint { get; private set; }

  [Header("Camera")]
  [SerializeField] private CinemachineCamera cameraA;
  [SerializeField] private CinemachineCamera cameraB;

  [Header("Audio")]
  public AudioTag ambienceTrack;
  public AudioTag musicTrack = AudioTag.None;

  public float musicTrackParameter;

  public Turn turn { get; private set; }
  private List<EnemyController> activeEnemies = new();
  public int stepLeft { get; private set; }

  void Awake()
  {
    stepLeft = maxStep;
    turn = Turn.Player;
    GameplayManager.Instance.RegisterStage(this);
  }

  void OnEnable()
  {
    SwitchCamera(GameplayManager.Instance.cameraMode);
    GameplayManager.Instance.OnCameraSwitch += SwitchCamera;

    HUDOverlayUIController.Instance.SetStageName(stageName);
    HUDOverlayUIController.Instance.ToggleBottom(isPuzzle);
    HUDOverlayUIController.Instance.SetStepLeft(maxStep);

    if (ambienceTrack != AudioTag.None) GameAudioManager.Instance.PlayAmbience(ambienceTrack);
    else GameAudioManager.Instance.StopAmbience();

    GameEventsManager.Instance.turnEvents.onPlayerTurnEnd += PlayerTurnEnd;
    GameEventsManager.Instance.turnEvents.onEnemyTurnEnd += EnemyTurnEnd;
    GameEventsManager.Instance.turnEvents.onStageFinished += StageFinished;
  }

  void OnDisable()
  {
    GameplayManager.Instance.OnCameraSwitch -= SwitchCamera;

    GameEventsManager.Instance.turnEvents.onPlayerTurnEnd -= PlayerTurnEnd;
    GameEventsManager.Instance.turnEvents.onEnemyTurnEnd -= EnemyTurnEnd;
    GameEventsManager.Instance.turnEvents.onStageFinished -= StageFinished;
  }

  public void RegisterEnemy(EnemyController enemy)
  {
    if (activeEnemies.Contains(enemy)) return;
    activeEnemies.Add(enemy);
  }

  public void UnregisterEnemy(EnemyController enemy)
  {
    if (!activeEnemies.Contains(enemy)) return;
    activeEnemies.Remove(enemy);
  }

  public bool IsGround(Vector3 position)
  {
    if (tileMap == null) return true;
    Vector3Int cell = tileMap.WorldToCell(position);
    return tileMap.HasTile(cell);
  }

  void EnemyTurnEnd()
  {
    turn = Turn.Player;
  }

  async void PlayerTurnEnd(Task playerAction)
  {
    turn = Turn.Enemy;

    if (isPuzzle)
    {
      stepLeft--;
      HUDOverlayUIController.Instance.SetStepLeft(stepLeft);
    }

    await playerAction;

    List<Task> enemyTasks = new();

    foreach (EnemyController enemy in activeEnemies)
    {
      if (enemy == null) continue;
      enemyTasks.Add(enemy.TakeTurnAsync());
    }

    GameEventsManager.Instance.turnEvents.EnemyTurnEnd();
    await Task.WhenAll(enemyTasks);
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

  void StageFinished()
  {
    if (cameraA == null || cameraB == null) return;
    cameraA.Priority = -1;
    cameraB.Priority = -1;
  }
}