using System;
using UnityEngine;

public interface IPanel
{
  event Action OnCloseRequested;
  event Action OnCloseAllRequested;
  event Action<string> OnNavigateRequested;

  void Show();
  void Hide();
}

public abstract class NavigationPanel : MonoBehaviour, IPanel
{
  public event Action OnCloseRequested;
  public event Action OnCloseAllRequested;
  public event Action<string> OnNavigateRequested;

  public virtual void Show() => gameObject.SetActive(true);
  public virtual void Hide() => gameObject.SetActive(false);

  // Helper methods for the inherited panels to call
  protected void ClosePanel() => OnCloseRequested?.Invoke();
  protected void CloseAllPanels() => OnCloseAllRequested?.Invoke();
  protected void Navigate(string panelName) => OnNavigateRequested?.Invoke(panelName);
}