using System;

public interface ITrigger
{
  public event Action OnDefaultAction;
  public event Action OnMainAction;
  public event Action OnLockAction;
}