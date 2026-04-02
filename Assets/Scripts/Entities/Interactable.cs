using System;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class Interactable : MonoBehaviour
{
  public event Action OnDefaultInteracted;
  public event Action OnInteracted;
  public event Action OnLockInteract;
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
    if (isLocked) OnLockInteract?.Invoke();
    else OnInteracted?.Invoke();
    OnDefaultInteracted?.Invoke();
  }

  public Vector3 GetPosition() => transform.position;
}