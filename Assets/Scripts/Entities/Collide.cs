using System;
using UnityEngine;

public class Collide : MonoBehaviour
{
  public event Action<Vector3> OnCollided;
  public event Action OnLockedCollided;

  public bool isLocked { get; set; }

  public void OnCollide(Vector3 direction)
  {
    if (isLocked) OnLockedCollided?.Invoke();
    else OnCollided?.Invoke(direction);
  }
}