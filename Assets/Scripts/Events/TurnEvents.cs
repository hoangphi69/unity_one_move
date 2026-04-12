using System;
using System.Threading.Tasks;

public class TurnEvents
{
  public event Action<Task> onPlayerTurnEnd;
  public void PlayerTurnEnd(Task action) => onPlayerTurnEnd?.Invoke(action);

  public event Action onEnemyTurnEnd;
  public void EnemyTurnEnd() => onEnemyTurnEnd?.Invoke();

  public event Action onStageRestart;
  public void RestartStage() => onStageRestart?.Invoke();
}