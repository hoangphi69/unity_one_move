using System;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class Interactable : MonoBehaviour, ITrigger
{
  public event Action OnDefaultAction;
  public event Action OnMainAction;
  public event Action OnLockAction;

  public bool isLocked { get; set; }
  private Outline outline;

  void Start()
  {
    outline = GetComponent<Outline>();
    outline.enabled = false;
  }

  public virtual void OnDetected()
  {
    outline.enabled = true;
  }

  public virtual void OnLost()
  {
    outline.enabled = false;
  }

  public virtual void OnInteract()
  {
    if (isLocked) OnLockAction?.Invoke();
    else OnMainAction?.Invoke();
    OnDefaultAction?.Invoke();
  }

  public Vector3 GetPosition() => transform.position;
}
