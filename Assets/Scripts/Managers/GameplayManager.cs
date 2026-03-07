using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Turn { Player, Enemy };

public class GameplayManager : MonoBehaviour
{
  public static GameplayManager Instance { get; private set; }

  // Configs
  [SerializeField] public LayerMask entityMask;
  [SerializeField] public float cellSize = 1f;

  // Turn Manager
  public Turn turn { get; private set; }
  private List<EnemyController> activeEnemies = new();

  // Stage
  public StageManager stageManager { get; private set; } = null;
  private string _currentStage = null;
  private bool _isCutscene = false;

  // Player
  [SerializeField] private GameObject playerPrefab;
  public PlayerController activePlayer { get; private set; }

  private void Awake()
  {
    Instance = this;
    turn = Turn.Player;
  }

  void OnEnable()
  {
    GameEventsManager.Instance.turnEvents.onPlayerTurnEnd += EnemyTurnStart;
    GameEventsManager.Instance.turnEvents.onEnemyTurnEnd += PlayerTurnStart;
  }

  void OnDisable()
  {
    GameEventsManager.Instance.turnEvents.onPlayerTurnEnd -= EnemyTurnStart;
    GameEventsManager.Instance.turnEvents.onEnemyTurnEnd -= PlayerTurnStart;
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

    try
    {
      InputActionsManager.Instance.SetState(InputState.UI);

      Task cutsceneFinished = CutsceneManager.Instance.StartCutscene(cutsceneKnot);

      await Utility.UnloadAsync(_currentStage);

      var loadOp = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
      loadOp.allowSceneActivation = false;

      await cutsceneFinished;

      loadOp.allowSceneActivation = true;
      while (!loadOp.isDone) await Task.Yield();

      _currentStage = scene;

      await CutsceneManager.Instance.HideCutscene();

      InputActionsManager.Instance.SetState(InputState.Gameplay);
    }
    catch (Exception e)
    {
      Debug.LogError($"Transition Error: {e}");
    }
    finally
    {
      _isCutscene = false;
    }
  }

  public async Task RestartStageAsync()
  {
    if (!isPuzzleStage()) return;
    await LoadStageAsync(_currentStage);
    SpawnPlayer();
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

  public void SpawnPlayer()
  {
    if (playerPrefab == null)
    {
      Debug.LogError("Player prefab missing");
      return;
    }

    if (activePlayer != null)
    {
      Destroy(activePlayer.gameObject);
      activePlayer = null;
    }

    Vector3 position = stageManager.defaultPlayerPosition;
    Quaternion rotation = Quaternion.identity;
    GameObject playerObj = Instantiate(playerPrefab, position, rotation);
    activePlayer = playerObj.GetComponent<PlayerController>();
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