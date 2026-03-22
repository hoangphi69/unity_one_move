using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class NavigationUIController : MonoBehaviour
{
  [SerializeField] protected GameObject uiContainer;

  // The Registry: Maps string names to panel instances
  private Dictionary<string, IPanel> panelRegistry = new Dictionary<string, IPanel>();
  private Stack<IPanel> menuStack = new Stack<IPanel>();

  public virtual void Show() => uiContainer.SetActive(true);
  public virtual void Hide() => uiContainer.SetActive(false);

  protected void RegisterPanel(string panelName, IPanel panel)
  {
    if (panelRegistry.ContainsKey(panelName))
    {
      Debug.LogWarning($"[UIController] Panel '{panelName}' is already registered.");
      return;
    }

    panelRegistry.Add(panelName, panel);

    // The Controller strictly ONLY registers to these three events
    panel.OnCloseRequested += CloseCurrentPanel;
    panel.OnCloseAllRequested += CloseEntireUI;
    panel.OnNavigateRequested += NavigateToPanel;

    panel.Hide();
  }

  protected void NavigateToPanel(string targetPanelName)
  {
    if (!panelRegistry.TryGetValue(targetPanelName, out IPanel targetPanel))
    {
      throw new Exception($"[UIController] Navigation Error: Target panel '{targetPanelName}' does not exist in the registry.");
    }

    if (menuStack.Count > 0)
      menuStack.Peek().Hide();

    menuStack.Push(targetPanel);
    targetPanel.Show();
  }

  protected void ClearStack()
  {
    while (menuStack.Count > 0)
    {
      menuStack.Pop().Hide();
    }
  }

  private void CloseCurrentPanel()
  {
    if (menuStack.Count <= 1) return;
    menuStack.Pop().Hide();
    menuStack.Peek().Show();
  }

  private void CloseEntireUI()
  {
    ClearStack();
    Hide();
  }

  protected void UnregisterAllPanels()
  {
    foreach (var panel in panelRegistry.Values)
    {
      panel.OnCloseRequested -= CloseCurrentPanel;
      panel.OnCloseAllRequested -= CloseEntireUI;
      panel.OnNavigateRequested -= NavigateToPanel;
    }
    panelRegistry.Clear();
  }
}