using System.Threading.Tasks;
using UnityEngine;

public enum GameState
{
  Booting,
  TitleScreen,
  Gameplay,
  Cutscene,
  Paused
}

public class GameFlowManager : MonoBehaviour
{
  public GameState CurrentState { get; private set; } = GameState.Booting;

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
    GameAudioManagger.Instance.PlayMusic(FMODEvents.Instance.TitleMusic);
    LoadingScreenUIController.Instance.Show();
    SetState(GameState.Booting);
    GameInputManager.Instance.SetState(InputState.UI);

    TitleScreenUIController.Instance.Show();
    SetState(GameState.TitleScreen);

    await LoadGame();

    LoadingScreenUIController.Instance.Hide();
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
    SetState(GameState.Booting);

    // Execute player audio animation 
    if (!GameplayManager.Instance.stageManager.radioTrack.IsNull)
    {
      await GameplayManager.Instance.activePlayer.LowerMusic();
      GameAudioManagger.Instance.LowerMusic();
    }

    GameInputManager.Instance.SetState(InputState.UI);
    SetState(GameState.Paused);

    PauseScreenUIController.Instance.Show();
  }

  async void ContinueGame()
  {
    SetState(GameState.Booting);

    // Execute player audio animation
    if (!GameplayManager.Instance.stageManager.radioTrack.IsNull)
    {
      await GameplayManager.Instance.activePlayer.PlayMusic();
      GameAudioManagger.Instance.PlayMusic(GameplayManager.Instance.stageManager.radioTrack);
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
    if(CurrentState == GameState.Booting) return;
    SetState(GameState.Booting);

    await GameplayManager.Instance.RestartStageAsync();

    if (!GameplayManager.Instance.stageManager.radioTrack.IsNull)
    {
      await GameplayManager.Instance.activePlayer.PlayMusic();
      GameAudioManagger.Instance.PlayMusic(GameplayManager.Instance.stageManager.radioTrack);
    }
    else
    {
      GameAudioManagger.Instance.StopMusic();
    }

    SetState(GameState.Gameplay);
    GameInputManager.Instance.SetState(InputState.Gameplay);
  }

  async void NewGame()
  {
    LoadingScreenUIController.Instance.Show();
    SetState(GameState.Booting);

    GameInputManager.Instance.SetState(InputState.UI);

    GameDataManager.Instance.NewGame();

    LoadingScreenUIController.Instance.Hide();

    string cutscene = "ch1_Cutscene1";
    string stageName = GameplayManager.Instance.newGameStage;
    await GameplayManager.Instance.LoadStageAsync(stageName, cutscene);

    SetState(GameState.Gameplay);
  }
}