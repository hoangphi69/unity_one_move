using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public enum Turn { Player, Enemy };

public class GameplayManager : MonoBehaviour
{
  public static GameplayManager Instance { get; private set; }

  // Configs
  [SerializeField] public LayerMask entityMask;
  public SceneField newGameStage;
  public float cellSize { get; private set; } = 1f;

  // Turn Manager
  public Turn turn { get; private set; }
  private List<EnemyController> activeEnemies = new();

  // Stage
  public StageManager stageManager { get; private set; } = null;
  private string _currentStage = null;
  private bool _isCutscene = false;

  // Player
  [SerializeField] private GameObject playerPrefab;
  public PlayerController ActivePlayer { get; private set; }

  [SerializeField] private CinemachineCamera playerCameraPrefab;
  public CinemachineCamera PlayerCam { get; private set; }
  public CameraMode cameraMode = CameraMode.A;
  public Action<CameraMode> OnCameraSwitch;

  private void Awake()
  {
    Instance = this;
    turn = Turn.Player;
  }

  void OnEnable()
  {
    GameEventsManager.Instance.turnEvents.onPlayerTurnEnd += EnemyTurnStart;
    GameEventsManager.Instance.turnEvents.onEnemyTurnEnd += PlayerTurnStart;

    GameInputManager.Instance.Actions.Player.Escape.performed += PauseGame;
    GameInputManager.Instance.Actions.Player.Restart.performed += RestartGame;
    GameInputManager.Instance.Actions.Player.SwitchCamera.performed += SwitchCameraMode;
  }

  void OnDisable()
  {
    GameEventsManager.Instance.turnEvents.onPlayerTurnEnd -= EnemyTurnStart;
    GameEventsManager.Instance.turnEvents.onEnemyTurnEnd -= PlayerTurnStart;

    GameInputManager.Instance.Actions.Player.Escape.performed -= PauseGame;
    GameInputManager.Instance.Actions.Player.Restart.performed -= RestartGame;
    GameInputManager.Instance.Actions.Player.SwitchCamera.performed -= SwitchCameraMode;
  }

  void PauseGame(InputAction.CallbackContext context) => GameEventsManager.Instance.flowEvents.PauseGame();

  void RestartGame(InputAction.CallbackContext context) => GameEventsManager.Instance.turnEvents.RestartStage();

  void SwitchCameraMode(InputAction.CallbackContext context) => SwitchCamera();

  public async Task LoadStageAsync(string scene)
  {
    await Utility.UnloadAsync(_currentStage);
    await Utility.LoadAdditiveAsync(scene);
    _currentStage = scene;
  }

  public async Task LoadStageAsync(string scene, string cutsceneKnot)
  {
    if (_isCutscene) return;
    _isCutscene = true;

    var tcs = new TaskCompletionSource<bool>();
    Action cutSceneEnd = null;
    cutSceneEnd = () =>
    {
      GameEventsManager.Instance.dialogueEvents.onLeaveDialogue -= cutSceneEnd;
      tcs.TrySetResult(true);
    };

    GameEventsManager.Instance.dialogueEvents.onLeaveDialogue += cutSceneEnd;
    GameEventsManager.Instance.dialogueEvents.EnterDialogue(cutsceneKnot, DialogueMode.Cutscene);
    GameAudioManagger.Instance.StopMusic();

    try
    {
      await Utility.UnloadAsync(_currentStage);

      var loadOp = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
      loadOp.allowSceneActivation = false;

      await tcs.Task;

      loadOp.allowSceneActivation = true;
      while (!loadOp.isDone) await Task.Yield();

      _currentStage = scene;
      SpawnPlayer();

      // Audio
      if (!stageManager.radioTrack.IsNull)
      {
        await ActivePlayer.PlayMusic();
        GameAudioManagger.Instance.PlayMusic(stageManager.radioTrack);
      }
      else
      {
        GameAudioManagger.Instance.StopMusic();
      }

      GameInputManager.Instance.SetState(InputState.Gameplay);
    }
    catch (Exception e)
    {
      GameEventsManager.Instance.dialogueEvents.onLeaveDialogue -= cutSceneEnd;
      Debug.LogError($"Transition Error: {e}");
    }
    finally { _isCutscene = false; }
  }

  public async Task RestartStageAsync()
  {
    if (!isPuzzleStage()) return;

    if (PlayerCam != null) PlayerCam.gameObject.SetActive(false);

    await LoadStageAsync(_currentStage);
    SpawnPlayer();

    if (PlayerCam != null) PlayerCam.gameObject.SetActive(true);
  }

  public bool isCutscene()
  {
    return _isCutscene;
  }

  public bool isPuzzleStage()
  {
    return stageManager.isPuzzle;
  }

  public void RegisterStage(StageManager stage) => stageManager = stage;

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

  void InitializePlayerCam()
  {
    if (playerCameraPrefab != null && PlayerCam == null)
    {
      PlayerCam = Instantiate(playerCameraPrefab, transform);
    }
  }

  public void SetPlayerCameraTarget(Transform target)
  {
    if (PlayerCam == null) return;
    PlayerCam.Follow = target;
    PlayerCam.LookAt = target;
  }

  void SwitchCamera()
  {
    cameraMode = cameraMode switch
    {
      CameraMode.A => CameraMode.B,
      CameraMode.B => CameraMode.A,
      _ => CameraMode.A
    };

    OnCameraSwitch?.Invoke(cameraMode);
  }

  public void DespawnPlayer()
  {
    if (ActivePlayer == null) return;
    SetPlayerCameraTarget(null);
    Destroy(ActivePlayer.gameObject);
    ActivePlayer = null;
  }

  public void SpawnPlayer()
  {
    if (playerPrefab == null)
    {
      Debug.LogError("Player prefab missing");
      return;
    }

    if (ActivePlayer != null) DespawnPlayer();

    InitializePlayerCam();

    Vector3 position = stageManager.defaultPlayerPosition;
    Quaternion rotation = Quaternion.identity;

    GameObject playerObj = Instantiate(playerPrefab, position, rotation);
    ActivePlayer = playerObj.GetComponent<PlayerController>();

    SetPlayerCameraTarget(playerObj.transform);
  }

  void PlayerTurnStart()
  {
    turn = Turn.Player;
  }

  async void EnemyTurnStart()
  {
    turn = Turn.Enemy;

    List<Task> enemyTasks = new();

    foreach (EnemyController enemy in activeEnemies)
    {
      if (enemy == null) continue;
      enemyTasks.Add(enemy.TakeTurnAsync());
    }

    GameEventsManager.Instance.turnEvents.EnemyTurnEnd();
    await Task.WhenAll(enemyTasks);
  }
}