using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

public class TitleScreenManager : MonoBehaviour
{
  public static TitleScreenManager Instance { get; private set; }

  [Header("All Canvas Panels")]
  [SerializeField] private GameObject titlePanel;
  [SerializeField] private GameObject optionsPanel;
  [SerializeField] private GameObject savesPanel;
  [SerializeField] private ConfirmOverlay confirmOverlay;

  // The stack keeps track of our menu history
  private Stack<GameObject> menuStack = new Stack<GameObject>();

  private void Awake()
  {
    if (Instance == null) Instance = this;
    else Destroy(gameObject);
  }

  private void Start()
  {
    // Ensure everything is turned off initially except the title panel
    optionsPanel.SetActive(false);
    savesPanel.SetActive(false);
    confirmOverlay.Hide();

    if (titlePanel != null)
      OpenPanel(titlePanel);
  }

  // Public methods for navigation. Your panels just call these.
  public void OpenTitle() => OpenPanel(titlePanel);
  public void OpenOptions() => OpenPanel(optionsPanel);
  public void OpenSaves() => OpenPanel(savesPanel);
  public void OpenConfirm(string title, string message, Action onConfirm) => confirmOverlay.Show(title, message, onConfirm);

  private void OpenPanel(GameObject newPanel)
  {
    if (menuStack.Count > 0)
    {
      menuStack.Peek().SetActive(false);
    }

    menuStack.Push(newPanel);
    newPanel.SetActive(true);

    // EventSystem.current.SetSelectedGameObject(null);
  }

  public void CloseCurrentPanel()
  {
    if (menuStack.Count > 1)
    {
      GameObject currentPanel = menuStack.Pop();
      currentPanel.SetActive(false);

      menuStack.Peek().SetActive(true);
    }
  }
}