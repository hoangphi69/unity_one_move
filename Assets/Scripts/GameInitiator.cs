using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameInitiator : MonoBehaviour
{
  [SerializeField] private Canvas _loadingScreen;

  [Header("Main")]
  [SerializeField] private Camera _mainCamera;
  [SerializeField] private Light _mainDirectionalLight;
  [SerializeField] private EventSystem _mainEventSystem;

  [Header("Services")]
  [SerializeField] private GameEventsManager _gameEventsManager;
  [SerializeField] private GameInputManager _gameInputManager;
  [SerializeField] private GameDataManager _gameDataManager;

  [Header("Managers")]
  [SerializeField] private GameSceneManager _gameSceneManager;
  [SerializeField] private GameplayManager _gameplayManager;
  [SerializeField] private GameDialogueManager _gameDialogueManager;

  [SerializeField] private GameObject _IGDialogue;


  async void Start()
  {
    Bindings();
    _loadingScreen.gameObject.SetActive(true);
    await InitializeServices();
    InitializeManagers();
    await Creation();
    await Task.Delay(1000);
    _loadingScreen.gameObject.SetActive(false);
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
    _gameSceneManager = Instantiate(_gameSceneManager);
    _gameplayManager = Instantiate(_gameplayManager);
    _gameDialogueManager = Instantiate(_gameDialogueManager);

    _IGDialogue = Instantiate(_IGDialogue);
  }

  async Task Creation()
  {
    await _gameSceneManager.LoadTitleScene();
    await _gameSceneManager.LoadTitleGameplay();
  }
}