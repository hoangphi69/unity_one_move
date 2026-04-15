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

    GameAudioManagger.Instance.PlayMusic(FMODEvents.Instance.TitleMusic);
    TitleScreenUIController.Instance.Show();
    SetState(GameState.TitleScreen);

    await LoadGame();

    await LoadingScreenUIController.Instance.HideFadeAsync();
  }

  async Task LoadGame()
  {
    if (!GameDataManager.Instance.HasData())
    {
      string stageName = GameplayManager.Instance.newGameStage;
      await GameplayManager.Instance.LoadStageAsync(stageName);
      GameplayManager.Instance.DespawnPlayer();
    }
    else
    {
      string stageName = GameDataManager.Instance.GetProgress();
      await GameplayManager.Instance.LoadStageAsync(stageName);
      GameplayManager.Instance.SpawnPlayer();
    }
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

    PauseScreenUIController.Instance.Show();
  }

  async void ContinueGame()
  {
    if (CurrentState == GameState.Busy) return;
    SetState(GameState.Busy);

    // Execute player audio animation
    if (!GameplayManager.Instance.Stage.radioTrack.IsNull)
    {
      await GameplayManager.Instance.ActivePlayer.PlayMusic();
      GameAudioManagger.Instance.PlayMusic(GameplayManager.Instance.Stage.radioTrack);
    }
    else
    {
      GameAudioManagger.Instance.StopMusic();
    }

    SetState(GameState.Gameplay);
    GameInputManager.Instance.SetState(InputState.Gameplay);
  }

  async void RestartStage()
  {
    if (CurrentState == GameState.Busy) return;
    SetState(GameState.Busy);

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

    await LoadingScreenUIController.Instance.ShowFadeAsync();

    TitleScreenUIController.Instance.CloseEntireUI();

    GameInputManager.Instance.SetState(InputState.UI);

    GameDataManager.Instance.NewGame();

    await Task.Delay(1000);
    await LoadingScreenUIController.Instance.HideFadeAsync();

    string cutscene = "ch1_Cutscene1";
    string stageName = GameplayManager.Instance.newGameStage;
    await GameplayManager.Instance.LoadStageAsync(stageName, cutscene);
    GameEventsManager.Instance.questEvents.StartQuest("lobby1_GoOutside");

    SetState(GameState.Gameplay);
  }
}