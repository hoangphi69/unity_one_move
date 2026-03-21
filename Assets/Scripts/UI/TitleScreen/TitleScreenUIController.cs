using UnityEngine;
using System.Collections.Generic;

public class TitleScreenUIController : MonoBehaviour
{
  public static TitleScreenUIController Instance { get; private set; }

  [Header("All Canvas Panels")]
  [SerializeField] private GameObject uiContainer;
  [SerializeField] private GameObject titlePanel;
  [SerializeField] private OptionsPanel optionsPanel;
  [SerializeField] private GameObject savesPanel;

  // The stack keeps track of our menu history
  private Stack<GameObject> menuStack = new Stack<GameObject>();

  private void Awake()
  {
    if (Instance == null) Instance = this;
    else Destroy(gameObject);
  }

  private void Start()
  {
    optionsPanel.gameObject.SetActive(false);
    optionsPanel.OnCloseRequested += CloseCurrentPanel;

    savesPanel.SetActive(false);

    if (titlePanel != null) OpenTitle();
  }

  public void Show() => uiContainer.SetActive(true);
  public void Hide() => uiContainer.SetActive(false);

  public void OpenTitle() => OpenPanel(titlePanel);
  public void OpenOptions() => OpenPanel(optionsPanel.gameObject);
  public void OpenSaves() => OpenPanel(savesPanel);

  private void OpenPanel(GameObject newPanel)
  {
    if (menuStack.Count > 0) menuStack.Peek().SetActive(false);

    menuStack.Push(newPanel);
    newPanel.SetActive(true);
  }

  public void CloseCurrentPanel()
  {
    if (menuStack.Count <= 1) return;
    menuStack.Pop().SetActive(false);
    menuStack.Peek().SetActive(true);
  }
}