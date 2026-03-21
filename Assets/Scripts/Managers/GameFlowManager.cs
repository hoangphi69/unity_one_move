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
    GameEventsManager.Instance.turnEvents.onStageRestart += ContinueGame;
    GameEventsManager.Instance.flowEvents.onGameNew += NewGame;
    GameEventsManager.Instance.flowEvents.onBootTitle += BootTitle;
  }

  void OnDisable()
  {
    GameEventsManager.Instance.flowEvents.onGamePaused -= PauseGame;
    GameEventsManager.Instance.flowEvents.onGameContinue -= ContinueGame;
    GameEventsManager.Instance.turnEvents.onStageRestart += ContinueGame;
    GameEventsManager.Instance.flowEvents.onGameNew -= NewGame;
    GameEventsManager.Instance.flowEvents.onBootTitle -= BootTitle;
  }

  public async void BootTitle()
  {
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

  void PauseGame()
  {
    SetState(GameState.Paused);
    GameInputManager.Instance.SetState(InputState.UI);
  }

  void ContinueGame()
  {
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