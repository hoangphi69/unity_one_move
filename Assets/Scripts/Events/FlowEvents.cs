using System;

public class FlowEvents
{
  public event Action onGamePaused;
  public void PauseGame() => onGamePaused?.Invoke();

  public event Action onGameContinue;
  public void ContinueGame() => onGameContinue?.Invoke();

  public event Action onGameNew;
  public void NewGame() => onGameNew?.Invoke();

  public event Action onBootTitle;
  public void ReturnToTitle() => onBootTitle?.Invoke();

  public event Action onGameLoad;
  public void LoadGame() => onGameLoad?.Invoke();
}