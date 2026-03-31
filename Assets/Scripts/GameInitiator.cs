using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameInitiator : MonoBehaviour
{

  [Header("Main")]
  [SerializeField] private Camera _mainCamera;
  [SerializeField] private Light _mainDirectionalLight;
  [SerializeField] private EventSystem _mainEventSystem;

  [Header("Services")]
  [SerializeField] private GameEventsManager _gameEventsManager;
  [SerializeField] private GameInputManager _gameInputManager;
  [SerializeField] private GameDataManager _gameDataManager;
  [SerializeField] private GameSettingsManager _gameSettingsManager;
  [SerializeField] private GameAudioManagger _gameAudioManager;

  [Header("Managers")]
  [SerializeField] private GameplayManager _gameplayManager;
  [SerializeField] private GameDialogueManager _gameDialogueManager;
  [SerializeField] private GameFlowManager _gameFlowManager;
  [SerializeField] private GameQuestManager _gameQuestManager;

  [Header("UI")]
  [SerializeField] private LoadingScreenUIController _loadingScreen;
  [SerializeField] private TitleScreenUIController _titleScreen;
  [SerializeField] private PauseScreenUIController _pauseScreen;
  [SerializeField] private ConfirmOverlayUIController _confirmOverlay;

  async void Start()
  {
    Bindings();
    _loadingScreen.Show();
    await InitializeServices();
    InitializeManagers();
    InitializeUI();
    StartGame();
  }

  void Bindings()
  {
    // Prioritize loading screen first
    _loadingScreen = Instantiate(_loadingScreen);
    _mainCamera = Instantiate(_mainCamera);
    _mainDirectionalLight = Instantiate(_mainDirectionalLight);
    _mainEventSystem = Instantiate(_mainEventSystem);
  }

  async Task InitializeServices()
  {
    _gameEventsManager = Instantiate(_gameEventsManager);
    _gameInputManager = Instantiate(_gameInputManager);
    _gameDataManager = Instantiate(_gameDataManager);
    _gameSettingsManager = Instantiate(_gameSettingsManager);
    _gameAudioManager = Instantiate(_gameAudioManager);

    _gameInputManager.SetState(InputState.UI);
    _gameSettingsManager.Initialize();
    await _gameDataManager.Initialize();
  }

  void InitializeManagers()
  {
    _gameplayManager = Instantiate(_gameplayManager);
    _gameDialogueManager = Instantiate(_gameDialogueManager);
    _gameFlowManager = Instantiate(_gameFlowManager);
    _gameQuestManager = Instantiate(_gameQuestManager);
  }

  void InitializeUI()
  {
    _titleScreen = Instantiate(_titleScreen);
    _pauseScreen = Instantiate(_pauseScreen);
    _confirmOverlay = Instantiate(_confirmOverlay);
  }

  void StartGame()
  {
    _gameFlowManager.BootTitle();
  }
}