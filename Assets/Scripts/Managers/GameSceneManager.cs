using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
  public static GameSceneManager Instance { get; private set; }

  [Header("System Scenes")]
  [SerializeField] private SceneField _loadingScene;
  [SerializeField] private SceneField _cutsceneScene;
  [SerializeField] private SceneField _pauseScene;
  [SerializeField] private SceneField _titleScene;
  [SerializeField] private SceneField _firstLobbyScene;

  // Track state
  private string _currentWorldScene; // Tracks Lobby or Gameplay
  private string _currentOverlayScene; // Tracks Title or Pause

  private TaskCompletionSource<bool> _cutsceneSignal;
  private bool _isLoading;
  private float _timeScaleCache = 1f;

  // Safety cancellation for async tasks if game quits
  private CancellationTokenSource _cts;


  private void Awake()
  {
    if (Instance != null) { Destroy(gameObject); return; }
    Instance = this;
    // DontDestroyOnLoad(gameObject);
    _cts = new CancellationTokenSource();
  }

  private void OnDestroy()
  {
    _cts?.Cancel();
    _cts?.Dispose();
  }

  private void Start()
  {
    // SCENARIO: First time open application
    _ = BootApplicationAsync();
  }

  #region 1. Boot & Title Logic

  private async Task BootApplicationAsync()
  {
    // Show Loading Overlay
    await LoadAdditiveAsync(_loadingScene);

    InputActionsManager.Instance.SetState(InputState.UI);

    await GameDataManager.Instance.LoadGameAsync();
    if (!GameDataManager.Instance.HasData())
      GameplayManager.Instance.LoadStageAsync(_firstLobbyScene);
    else
    {
      string stageName = GameDataManager.Instance.GetProgressScene();
      GameplayManager.Instance.LoadStageAsync(stageName);
    }

    await LoadAdditiveAsync(_titleScene);
    _currentOverlayScene = _titleScene;

    // Hide Loading
    await Task.Delay(1000);
    await UnloadAsync(_loadingScene);
  }

  // SCENARIO: Title -> Continue (Remove Title, keep Lobby)
  public async void OnTitleContinue()
  {
    if (_isLoading) return;
    _isLoading = true;

    await UnloadAsync(_titleScene);
    _currentOverlayScene = null;

    // Optional: Notify Lobby to enable player controls here
    InputActionsManager.Instance.SetState(InputState.World);

    _isLoading = false;
  }

  // SCENARIO: Title -> New Game (Show Loading, Reset Lobby, Remove Title)
  public async void OnTitleNewGame()
  {
    if (_isLoading) return;
    _isLoading = true;

    GameDataManager.Instance.NewGame();

    bool isNewGame = SceneManager.GetSceneByName(_firstLobbyScene).isLoaded;

    if (isNewGame)
    {
      await UnloadAsync(_titleScene);
      _currentOverlayScene = null;
    }
    else
    {
      await LoadAdditiveAsync(_loadingScene);
      InputActionsManager.Instance.SetState(InputState.UI);

      await UnloadAsync(_currentWorldScene);
      await UnloadAsync(_titleScene);
      _currentOverlayScene = null;
      await LoadAdditiveAsync(_firstLobbyScene);
      _currentWorldScene = _firstLobbyScene;

      await Task.Delay(1000);
      await UnloadAsync(_loadingScene);
    }

    InputActionsManager.Instance.SetState(InputState.World);

    _isLoading = false;
  }

  #endregion

  #region 2. Transition Logic (Lobby <-> Gameplay)

  // SCENARIO: Lobby <-> Gameplay with Cutscene sandwich
  public async void TravelToScene(string nextScene, string cutsceneId)
  {
    if (_isLoading) return;
    _isLoading = true;

    try
    {
      InputActionsManager.Instance.SetState(InputState.UI);

      _cutsceneSignal = new TaskCompletionSource<bool>();

      // 1. Load Cutscene Overlay (User watches this)
      await LoadAdditiveAsync(_cutsceneScene);

      // Trigger your Dialogue/Cutscene system here
      // GameEvents.TriggerCutscene(cutsceneId); 
      // Debug.Log($"Playing Cutscene: {cutsceneId}");
      GameEventsManager.Instance.dialogueEvents.EnterDialogue(cutsceneId, DialogueMode.Cutscene);

      // 2. Unload Old World (Behind the cutscene)
      if (!string.IsNullOrEmpty(_currentWorldScene))
        await UnloadAsync(_currentWorldScene);

      // 3. Load New World (Background) - Wait for 90% load
      var loadOp = SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);
      loadOp.allowSceneActivation = false; // Hold it at 90%

      // 4. Wait for TWO conditions: Scene is ready AND Cutscene is done
      // We wait for the cutscene signal (Triggered by FinishCutscene)
      await _cutsceneSignal.Task;

      // 5. Finish Scene Load
      loadOp.allowSceneActivation = true;
      while (!loadOp.isDone) await Task.Yield();

      _currentWorldScene = nextScene;

      // 6. Remove Cutscene Overlay
      await UnloadAsync(_cutsceneScene);

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

  // Call this from your Dialogue System/Timeline signal
  public void FinishCutscene()
  {
    _cutsceneSignal?.TrySetResult(true);
  }

  #endregion

  #region 3. Pause & Restart Logic

  public void TogglePause()
  {
    if (_isLoading || _currentOverlayScene == _titleScene) return;

    if (_currentOverlayScene == _pauseScene)
    {
      // RESUME
      _ = UnloadAsync(_pauseScene); // Fire and forget acts as void here
      _currentOverlayScene = null;
      Time.timeScale = _timeScaleCache;

      InputActionsManager.Instance.SetState(InputState.World);
    }
    else
    {
      // PAUSE
      _timeScaleCache = Time.timeScale;
      Time.timeScale = 0f;
      _ = LoadAdditiveAsync(_pauseScene);
      _currentOverlayScene = _pauseScene;

      InputActionsManager.Instance.SetState(InputState.UI);
    }
  }

  // SCENARIO: Restart (Resume button or Death)
  public async void RestartCurrentLevel()
  {
    if (_isLoading) return;

    // Ensure unpaused before reloading
    if (Time.timeScale == 0) Time.timeScale = 1;
    if (_currentOverlayScene == _pauseScene)
    {
      await UnloadAsync(_pauseScene);
      _currentOverlayScene = null;
    }

    _isLoading = true;

    // Simple fade out/in (reusing loading screen for restart)
    // await LoadAdditiveAsync(_loadingScene);

    await UnloadAsync(_currentWorldScene);
    await LoadAdditiveAsync(_currentWorldScene);

    InputActionsManager.Instance.SetState(InputState.World);

    _isLoading = false;
  }

  // SCENARIO: Return to Title (from Pause)
  public async void ReturnToTitle()
  {
    InputActionsManager.Instance.SetState(InputState.UI);
    Time.timeScale = 1f; // Reset time
    if (_isLoading) return;
    _isLoading = true;

    // 1. Show Loading
    await LoadAdditiveAsync(_loadingScene);

    await GameDataManager.Instance.SaveGameAsync();

    // 2. Clean up UI
    if (!string.IsNullOrEmpty(_currentOverlayScene))
      await UnloadAsync(_currentOverlayScene);

    // 3. Clean up World
    if (!string.IsNullOrEmpty(_currentWorldScene))
      await UnloadAsync(_currentWorldScene);

    // 4. Load Title + Lobby (Progress Saved state)
    string worldScene = GameDataManager.Instance.GetProgressScene();
    await LoadAdditiveAsync(worldScene);
    await LoadAdditiveAsync(_titleScene);

    _currentWorldScene = worldScene;
    _currentOverlayScene = _titleScene;

    // 5. Hide Loading
    await Task.Delay(1000);
    await UnloadAsync(_loadingScene);

    _isLoading = false;
  }

  #endregion

  #region Internal Helpers

  private async Task LoadAdditiveAsync(string sceneName)
  {
    if (!SceneManager.GetSceneByName(sceneName).isLoaded)
    {
      await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }
  }

  private async Task UnloadAsync(string sceneName)
  {
    if (SceneManager.GetSceneByName(sceneName).isLoaded)
    {
      await SceneManager.UnloadSceneAsync(sceneName);
    }
  }

  #endregion
}