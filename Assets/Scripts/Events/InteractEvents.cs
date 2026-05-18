using System;

public class InteractEvents
{
  public event Action<string> OnInteract;
  public void Interact(string id) => OnInteract?.Invoke(id);

  public event Action<string, bool> OnSwitch;
  public void Switch(string id, bool state) => OnSwitch?.Invoke(id, state);
}