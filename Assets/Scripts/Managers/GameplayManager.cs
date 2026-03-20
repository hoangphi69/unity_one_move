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
  [SerializeField] private CinemachineCamera cameraPrefab;

  public PlayerController activePlayer { get; private set; }
  public CinemachineCamera playerCam { get; private set; }

  private void Awake()
  {
    Instance = this;
    turn = Turn.Player;
  }

  void OnEnable()
  {
    GameEventsManager.Instance.turnEvents.onPlayerTurnEnd += EnemyTurnStart;
    GameEventsManager.Instance.turnEvents.onEnemyTurnEnd += PlayerTurnStart;
    GameEventsManager.Instance.turnEvents.onStageRestart += () => _ = RestartStageAsync();
    GameInputManager.Instance.Actions.Player.Escape.performed += PauseGame;
  }

  void OnDisable()
  {
    GameEventsManager.Instance.turnEvents.onPlayerTurnEnd -= EnemyTurnStart;
    GameEventsManager.Instance.turnEvents.onEnemyTurnEnd -= PlayerTurnStart;
    GameInputManager.Instance.Actions.Player.Escape.performed -= PauseGame;
  }

  void PauseGame(InputAction.CallbackContext context)
  {
    GameEventsManager.Instance.flowEvents.PauseGame();
  }

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
    }
    catch (Exception e)
    {
      GameEventsManager.Instance.dialogueEvents.onLeaveDialogue -= cutSceneEnd;
      Debug.LogError($"Transition Error: {e}");
    }
    finally { _isCutscene = false; }
  }

  async Task RestartStageAsync()
  {
    if (!isPuzzleStage()) return;

    if (playerCam != null) playerCam.gameObject.SetActive(false);

    await LoadStageAsync(_currentStage);
    SpawnPlayer();

    if (playerCam != null) playerCam.gameObject.SetActive(true);
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

  void InitializeCamera()
  {
    if (cameraPrefab != null && playerCam == null)
    {
      playerCam = Instantiate(cameraPrefab, transform);
    }
  }

  public void SetCameraTarget(Transform target)
  {
    if (playerCam == null) return;
    playerCam.Follow = target;
    playerCam.LookAt = target;
  }

  public void DespawnPlayer()
  {
    if (activePlayer == null) return;
    SetCameraTarget(null);
    Destroy(activePlayer.gameObject);
    activePlayer = null;
  }

  public void SpawnPlayer()
  {
    if (playerPrefab == null)
    {
      Debug.LogError("Player prefab missing");
      return;
    }

    if (activePlayer != null) DespawnPlayer();

    InitializeCamera();

    Vector3 position = stageManager.defaultPlayerPosition;
    Quaternion rotation = Quaternion.identity;

    GameObject playerObj = Instantiate(playerPrefab, position, rotation);
    activePlayer = playerObj.GetComponent<PlayerController>();

    SetCameraTarget(playerObj.transform);
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

    await Task.WhenAll(enemyTasks);
    GameEventsManager.Instance.turnEvents.EnemyTurnEnd();
  }
}