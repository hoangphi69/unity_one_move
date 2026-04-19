using System.Threading.Tasks;
using UnityEngine;

public enum GameState
{
  Busy,
  TitleScreen,
  Gameplay,
  Cutscene,
  Paused
}

public class GameFlowManager : MonoBehaviour
{
  public GameState CurrentState { get; private set; } = GameState.Busy;

  public void SetState(GameState newState) => CurrentState = newState;

  void OnEnable()
  {
    GameEventsManager.Instance.flowEvents.onGameLoad += () => _ = LoadGame();
    GameEventsManager.Instance.flowEvents.onGamePaused += PauseGame;
    GameEventsManager.Instance.flowEvents.onGameContinue += ContinueGame;
    GameEventsManager.Instance.turnEvents.onStageRestart += RestartStage;
    GameEventsManager.Instance.flowEvents.onGameNew += NewGame;
    GameEventsManager.Instance.flowEvents.onBootTitle += BootTitle;
  }

  void OnDisable()
  {
    GameEventsManager.Instance.flowEvents.onGamePaused -= PauseGame;
    GameEventsManager.Instance.flowEvents.onGameContinue -= ContinueGame;
    GameEventsManager.Instance.turnEvents.onStageRestart -= RestartStage;
    GameEventsManager.Instance.flowEvents.onGameNew -= NewGame;
    GameEventsManager.Instance.flowEvents.onBootTitle -= BootTitle;
  }

  public async void BootTitle()
  {
    SetState(GameState.Busy);

    if (!LoadingScreenUIController.Instance.isActive()) await LoadingScreenUIController.Instance.ShowFadeAsync();

    GameInputManager.Instance.SetState(InputState.UI);
    await GameDataManager.Instance.SaveGame();

    TitleScreenUIController.Instance.Show();
    SetState(GameState.TitleScreen);

    await LoadGame();

    // await Task.Delay(2000);

    GameAudioManagger.Instance.PlayMusic(FMODEvents.Instance.TitleMusic);

    await LoadingScreenUIController.Instance.HideFadeAsync(2f);
  }

  async Task LoadGame()
  {
    if (GameDataManager.Instance.HasData())
    {
      string stageName = GameDataManager.Instance.GetProgress();
      await GameplayManager.Instance.LoadStageAsync(stageName);
      GameplayManager.Instance.SpawnPlayer();
    }
    GameplayManager.Instance.ZoomCamera(false);
  }

  async void PauseGame()
  {
    if (CurrentState == GameState.Busy) return;
    SetState(GameState.Busy);

    // Execute player audio animation 
    if (!GameplayManager.Instance.Stage.radioTrack.IsNull)
    {
      await GameplayManager.Instance.ActivePlayer.LowerMusic();
      GameAudioManagger.Instance.LowerMusic();
    }

    GameInputManager.Instance.SetState(InputState.UI);
    SetState(GameState.Paused);

    HUDOverlayUIController.Instance.Hide();
    PauseScreenUIController.Instance.Show();
  }

  async void ContinueGame()
  {
    if (CurrentState == GameState.Busy) return;
    SetState(GameState.Busy);

    HUDOverlayUIController.Instance.Show();

    // Execute player audio animation
    if (!GameplayManager.Instance.Stage.radioTrack.IsNull)
    {
      await GameplayManager.Instance.ActivePlayer.PlayMusic();
      GameAudioManagger.Instance.PlayMusic(GameplayManager.Instance.Stage.radioTrack);
    }
    else GameAudioManagger.Instance.StopMusic();

    SetState(GameState.Gameplay);
    GameInputManager.Instance.SetState(InputState.Gameplay);
  }

  async void RestartStage()
  {
    if (!GameplayManager.Instance.Stage.isPuzzle) return;

    if (CurrentState == GameState.Busy) return;
    SetState(GameState.Busy);

    HUDOverlayUIController.Instance.Show();

    await LoadingScreenUIController.Instance.ShowStripsAsync();

    await GameplayManager.Instance.RestartStageAsync();

    if (!GameplayManager.Instance.Stage.radioTrack.IsNull)
    {
      await GameplayManager.Instance.ActivePlayer.PlayMusic();
      GameAudioManagger.Instance.PlayMusic(GameplayManager.Instance.Stage.radioTrack);
    }
    else
    {
      GameAudioManagger.Instance.StopMusic();
    }

    await LoadingScreenUIController.Instance.HideStripsAsync();

    SetState(GameState.Gameplay);
    GameInputManager.Instance.SetState(InputState.Gameplay);
  }

  async void NewGame()
  {
    if (CurrentState == GameState.Busy) return;
    SetState(GameState.Busy);

    GameInputManager.Instance.SetState(InputState.UI);

    await LoadingScreenUIController.Instance.ShowFadeAsync();

    GameAudioManagger.Instance.StopMusic();

    TitleScreenUIController.Instance.CloseEntireUI();

    GameDataManager.Instance.NewGame();

    string cutscene = "ch1_Cutscene1";
    GameEventsManager.Instance.dialogueEvents.EnterDialogue(cutscene, DialogueMode.Cutscene);

    await Task.Delay(1000);
    LoadingScreenUIController.Instance.HideImmediate();

    string stageName = GameplayManager.Instance.newGameStage;
    await GameplayManager.Instance.LoadStageAsync(stageName);
    GameplayManager.Instance.SpawnPlayer();
    GameplayManager.Instance.ZoomCamera(true);

    HUDOverlayUIController.Instance.Show();

    GameEventsManager.Instance.questEvents.StartQuest("lobby1_GoOutside");

    SetState(GameState.Gameplay);
  }
}