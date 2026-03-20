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

  [Header("Managers")]
  [SerializeField] private GameplayManager _gameplayManager;
  [SerializeField] private GameDialogueManager _gameDialogueManager;
  [SerializeField] private GameFlowManager _gameFlowManager;

  [Header("UI")]
  [SerializeField] private LoadingScreenUIController _loadingScreen;
  [SerializeField] private GameObject _titleScreen;
  [SerializeField] private GameObject _pauseScreen;
  [SerializeField] private GameObject _IGDialogue;


  async void Start()
  {
    Bindings();
    _loadingScreen.Show();
    await InitializeServices();
    InitializeManagers();
    StartGame();
  }

  void Bindings()
  {
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

    _gameInputManager.SetState(InputState.UI);
    await _gameDataManager.Initialize();
  }

  void InitializeManagers()
  {
    _gameplayManager = Instantiate(_gameplayManager);
    _gameDialogueManager = Instantiate(_gameDialogueManager);
    _gameFlowManager = Instantiate(_gameFlowManager);

    // UI
    _IGDialogue = Instantiate(_IGDialogue);
    _titleScreen = Instantiate(_titleScreen);
    _pauseScreen = Instantiate(_pauseScreen);

  }

  void StartGame()
  {
    _gameFlowManager.BootTitle();
  }
}