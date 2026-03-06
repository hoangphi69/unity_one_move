using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public enum Turn { Player, Enemy };

public class GameplayManager : MonoBehaviour
{
  public static GameplayManager Instance { get; private set; }

  // Configs
  [SerializeField] public LayerMask entityMask;
  [SerializeField] public float cellSize = 1f;

  // Environment
  public Tilemap environment;

  // Turn Manager
  public Turn turn { get; private set; }
  private List<EnemyController> activeEnemies = new();

  // Stage
  private string _currentStage = null;
  private bool _isLoading = false;

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

  public async void LoadStageAsync(string scene)
  {
    await Utility.LoadAdditiveAsync(scene);
    await Utility.UnloadAsync(_currentStage);
    _currentStage = scene;
  }

  public async void LoadStageAsync(string scene, string cutsceneKnot)
  {
    if (_isLoading) return;
    _isLoading = true;

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

      InputActionsManager.Instance.SetState(InputState.World);
    }
    catch (Exception e)
    {
      Debug.LogError($"Transition Error: {e}");
    }
    finally
    {
      _isLoading = false;
    }
  }

  public void RegisterEnvironment(Tilemap environment) => this.environment = environment;

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