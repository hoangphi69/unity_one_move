using System;
using UnityEngine;

public class Collide : MonoBehaviour, ITrigger
{
  public event Action OnDefaultAction;
  public event Action OnMainAction;
  public event Action OnLockAction;
  public event Action<Vector3> OnCollided;

  public bool isLocked { get; set; }

  public void OnCollide(Vector3 direction)
  {
    if (isLocked)
    {
      OnLockAction?.Invoke();
    }
    else
    {
      OnCollided?.Invoke(direction);
      OnMainAction?.Invoke();
    }
    OnDefaultAction?.Invoke();
  }
}