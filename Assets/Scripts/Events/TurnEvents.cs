using System;

public class TurnEvents
{
  public event Action onPlayerTurnEnd;
  public void PlayerTurnEnd() => onPlayerTurnEnd?.Invoke();

  public event Action onEnemyTurnEnd;
  public void EnemyTurnEnd() => onEnemyTurnEnd?.Invoke();
}