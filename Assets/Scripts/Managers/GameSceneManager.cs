using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameSceneManager : MonoBehaviour
{
  public static GameSceneManager Instance { get; private set; }

  [Header("System Scenes")]
  [SerializeField] private SceneField _loadingScene;
  [SerializeField] private SceneField _pauseScene;
  [SerializeField] private SceneField _titleScene;
  [SerializeField] private SceneField _firstLobbyScene;

  // Track state
  private string _currentOverlayScene; // Tracks Title or Pause

  public event Func<bool> OnUIBackRequested;

  private bool _isLoading;
  private float _timeScaleCache = 1f;

  // Safety cancellation for async tasks if game quits
  private CancellationTokenSource _cts;


  void Awake()
  {
    if (Instance != null) { Destroy(gameObject); return; }
    Instance = this;
    // DontDestroyOnLoad(gameObject);
    _cts = new CancellationTokenSource();
  }

  void OnDestroy()
  {
    _cts?.Cancel();
    _cts?.Dispose();
  }

  void OnEnable()
  {
    InputActionsManager.Instance.inputActions.Player.Escape.performed += TogglePause;
    InputActionsManager.Instance.inputActions.UI.Escape.performed += NavigateBack;
  }

  void OnDisable()
  {
    InputActionsManager.Instance.inputActions.Player.Escape.performed -= TogglePause;
    InputActionsManager.Instance.inputActions.UI.Escape.performed -= NavigateBack;
  }

  void Start()
  {
    // SCENARIO: First time open application
    _ = BootApplicationAsync();
  }

  #region 1. Boot & Title Logic

  private async Task BootApplicationAsync()
  {
    // Show Loading Overlay
    await Utility.LoadAdditiveAsync(_loadingScene);

    InputActionsManager.Instance.SetState(InputState.UI);

    await GameDataManager.Instance.LoadGame();
    if (!GameDataManager.Instance.HasData())
      await GameplayManager.Instance.LoadStageAsync(_firstLobbyScene);
    else
    {
      await LoadTitleGameplay();
    }

    await Utility.LoadAdditiveAsync(_titleScene);
    _currentOverlayScene = _titleScene;

    // Hide Loading
    await Task.Delay(1000);
    await Utility.UnloadAsync(_loadingScene);
  }

  public async Task LoadTitleGameplay()
  {
    string stageName = GameDataManager.Instance.GetProgress();
    await GameplayManager.Instance.LoadStageAsync(stageName);
    GameplayManager.Instance.SpawnPlayer();
  }

  // SCENARIO: Title -> Continue (Remove Title, keep Lobby)
  public async void OnTitleContinue()
  {
    if (_isLoading) return;
    _isLoading = true;

    await Utility.UnloadAsync(_titleScene);
    _currentOverlayScene = null;

    // Optional: Notify Lobby to enable player controls here
    InputActionsManager.Instance.SetState(InputState.Gameplay);

    _isLoading = false;
  }

  // SCENARIO: Title -> New Game (Show Loading, Reset Lobby, Remove Title)
  public async void OnTitleNewGame()
  {
    if (_isLoading) return;

    // Display loading
    _isLoading = true;
    await Utility.LoadAdditiveAsync(_loadingScene);

    InputActionsManager.Instance.SetState(InputState.UI);

    GameDataManager.Instance.NewGame();

    await Utility.UnloadAsync(_titleScene);
    _currentOverlayScene = null;

    // Hide loading
    await Task.Delay(1000);
    await Utility.UnloadAsync(_loadingScene);

    await GameplayManager.Instance.LoadStageAsync(_firstLobbyScene, "ch1_Cutscene1");
    GameplayManager.Instance.SpawnPlayer();

    InputActionsManager.Instance.SetState(InputState.Gameplay);

    _isLoading = false;
  }

  #endregion

  #region 3. Pause & Restart Logic

  public void NavigateBack(InputAction.CallbackContext _) => NavigateBack();

  public void NavigateBack()
  {
    bool backRequested = OnUIBackRequested?.Invoke() ?? false;
    if (!backRequested) TogglePause();
  }

  public void TogglePause(InputAction.CallbackContext _) => TogglePause();

  public void TogglePause()
  {
    if (_isLoading) return;
    if (_currentOverlayScene == _titleScene) return;
    if (GameplayManager.Instance.isCutscene()) return;

    if (_currentOverlayScene == _pauseScene)
    {
      // RESUME
      _ = Utility.UnloadAsync(_pauseScene); // Fire and forget acts as void here
      _currentOverlayScene = null;
      Time.timeScale = _timeScaleCache;

      InputActionsManager.Instance.SetState(InputState.Gameplay);
    }
    else
    {
      // PAUSE
      _timeScaleCache = Time.timeScale;
      Time.timeScale = 0f;
      _ = Utility.LoadAdditiveAsync(_pauseScene);
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
      await Utility.UnloadAsync(_pauseScene);
      _currentOverlayScene = null;
    }

    _isLoading = true;

    await GameplayManager.Instance.RestartStageAsync();

    InputActionsManager.Instance.SetState(InputState.Gameplay);

    _isLoading = false;
  }

  // SCENARIO: Return to Title (from Pause)
  public async void ReturnToTitle()
  {
    InputActionsManager.Instance.SetState(InputState.UI);
    Time.timeScale = 1f; // Reset time
    if (_isLoading) return;
    _isLoading = true;

    // Show Loading
    await Utility.LoadAdditiveAsync(_loadingScene);

    // (Optional: confirm to save game manually)
    // await GameDataManager.Instance.SaveGameAsync();

    // Clean up UI
    if (!string.IsNullOrEmpty(_currentOverlayScene))
      await Utility.UnloadAsync(_currentOverlayScene);

    // Load lobby/hallway gameplay
    await LoadTitleGameplay();

    // Load title
    await Utility.LoadAdditiveAsync(_titleScene);
    _currentOverlayScene = _titleScene;

    // Hide Loading
    await Task.Delay(1000);
    await Utility.UnloadAsync(_loadingScene);

    _isLoading = false;
  }

  #endregion
}