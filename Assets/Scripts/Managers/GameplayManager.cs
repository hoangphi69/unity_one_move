using System;
using System.Threading.Tasks;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
  public static GameplayManager Instance { get; private set; }

  // Configs
  [SerializeField] public LayerMask entityMask;
  public SceneField newGameStage;
  public float cellSize { get; private set; } = 1f;

  // Stage
  public StageManager Stage { get; private set; } = null;
  private string _currentStage = null;
  private bool _isCutscene = false;

  // Player
  [SerializeField] private GameObject playerPrefab;
  public PlayerController ActivePlayer { get; private set; }

  [SerializeField] private CinemachineCamera playerCameraPrefab;
  [SerializeField] private float titleFOV = 6.5f;
  [SerializeField] private float gameplayFOV = 4.5f;
  public CinemachineCamera PlayerCam { get; private set; }
  public CameraMode cameraMode = CameraMode.A;
  public Action<CameraMode> OnCameraSwitch;

  void Awake()
  {
    if (Instance == null) Instance = this;
  }

  void OnEnable()
  {
    GameInputManager.Instance.Actions.Player.Escape.performed += PauseGame;
    GameInputManager.Instance.Actions.Player.Restart.performed += RestartGame;
    GameInputManager.Instance.Actions.Player.SwitchCamera.performed += SwitchCameraMode;
  }

  void OnDisable()
  {
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

    HUDOverlayUIController.Instance.Hide();

    GameEventsManager.Instance.dialogueEvents.EnterDialogue(cutsceneKnot, DialogueMode.Cutscene);

    GameAudioManagger.Instance.StopMusic();

    try
    {
      await Task.Delay(1000); // Simulate cutscene opening animation

      await Utility.UnloadAsync(_currentStage);

      var loadOp = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
      loadOp.allowSceneActivation = false;

      await tcs.Task;

      loadOp.allowSceneActivation = true;
      while (!loadOp.isDone) await Task.Yield();

      _currentStage = scene;
      SpawnPlayer();

      // Audio
      if (!Stage.radioTrack.IsNull)
      {
        await ActivePlayer.PlayMusic();
        GameAudioManagger.Instance.PlayMusic(Stage.radioTrack);
      }
      else GameAudioManagger.Instance.StopMusic();

      HUDOverlayUIController.Instance.Show();

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
    if (!Stage.isPuzzle) return;

    if (PlayerCam != null) PlayerCam.gameObject.SetActive(false);

    await LoadStageAsync(_currentStage);
    SpawnPlayer();

    if (PlayerCam != null) PlayerCam.gameObject.SetActive(true);
  }

  public void RegisterStage(StageManager stage) => Stage = stage;

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

  public void ZoomCamera(bool isZoomed)
  {
    if (PlayerCam == null) return;
    float targetFOV = isZoomed ? gameplayFOV : titleFOV;
    DOTween.To(() => PlayerCam.Lens.OrthographicSize,
               x => PlayerCam.Lens.OrthographicSize = x,
               targetFOV,
               .5f)
            .SetEase(Ease.InOutSine);
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
    if (playerPrefab == null) return;

    if (ActivePlayer != null) DespawnPlayer();

    InitializePlayerCam();

    GameObject player = Instantiate(playerPrefab, Stage.SpawnPoint.position, Stage.SpawnPoint.rotation);
    ActivePlayer = player.GetComponent<PlayerController>();

    SetPlayerCameraTarget(player.transform);
  }
}