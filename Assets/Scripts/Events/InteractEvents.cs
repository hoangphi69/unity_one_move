using System;

public class InteractEvents
{
  public event Action<string> OnInteract;
  public void Interact(string id) => OnInteract?.Invoke(id);
}