using UnityEngine;

public interface IInteractable
{
  void OnDetected();
  void OnLost();
  void OnInteract();
  Vector3 GetPosition();
}