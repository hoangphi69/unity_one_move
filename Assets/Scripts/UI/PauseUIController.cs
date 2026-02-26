using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PauseUIController : MonoBehaviour
{
  private VisualElement _root;

  // -- Main panel --
  private VisualElement _mainPanel;
  private Button _resumeButton;
  private Button _restartButton;
  private Button _optionsButton;
  private Button _returnToTitleButton;

  // -- Options panel --
  private VisualElement _optionsPanel;
  private Button _graphicsButton;
  private Button _optionsBackButton;

  // -- Graphics panel --
  private VisualElement _graphicsPanel;
  private Button _graphicsBackButton;

  // -- Navigation --
  private Stack<VisualElement> _navigationHistory = new Stack<VisualElement>();
  private VisualElement _currentPanel;

  void Awake()
  {
    _root = GetComponent<UIDocument>().rootVisualElement;


    // -- Main Panel --
    _mainPanel = _root.Q("panel-main");
    _resumeButton = _mainPanel.Q<Button>("resume");
    _restartButton = _mainPanel.Q<Button>("restart");
    _returnToTitleButton = _mainPanel.Q<Button>("return_to_title");
    _optionsButton = _mainPanel.Q<Button>("options");

    // -- Options Panel --
    _optionsPanel = _root.Q("panel-options");
    _graphicsButton = _optionsPanel.Q<Button>("graphics");
    _optionsBackButton = _optionsPanel.Q<Button>("back");

    // -- Graphics Panel --
    _graphicsPanel = _root.Q("panel-graphics");
    _graphicsBackButton = _graphicsPanel.Q<Button>("back");

    // _mainPanel.AddToClassList("menu-panel");
    // _optionsPanel.AddToClassList("menu-panel");
    // _graphicsPanel.AddToClassList("menu-panel");
    ShowRootPanel();
  }

  void OnEnable()
  {
    _resumeButton.clicked += GameSceneManager.Instance.TogglePause;
    _restartButton.clicked += GameSceneManager.Instance.RestartCurrentLevel;
    _returnToTitleButton.clicked += GameSceneManager.Instance.ReturnToTitle;
    _optionsButton.clicked += () => SwitchPanel(_optionsPanel);

    _graphicsButton.clicked += () => SwitchPanel(_graphicsPanel);
    _optionsBackButton.clicked += GoBack;

    _graphicsBackButton.clicked += GoBack;

    InputActionsManager.Instance.OnUIBackRequested += HandleEscapeAction;
  }

  void OnDisable()
  {
    _resumeButton.clicked -= GameSceneManager.Instance.TogglePause;
    _restartButton.clicked -= GameSceneManager.Instance.RestartCurrentLevel;
    _returnToTitleButton.clicked -= GameSceneManager.Instance.ReturnToTitle;

    _optionsBackButton.clicked -= GoBack;

    _graphicsBackButton.clicked -= GoBack;

    InputActionsManager.Instance.OnUIBackRequested -= HandleEscapeAction;
  }

  private void ShowRootPanel()
  {
    _navigationHistory.Clear();

    // Reset: Hide all, Show Main
    _optionsPanel.AddToClassList("hidden");
    _graphicsPanel.AddToClassList("hidden");

    _mainPanel.RemoveFromClassList("hidden");
    _currentPanel = _mainPanel;
  }

  // --- Navigation Logic ---

  private void SwitchPanel(VisualElement targetPanel)
  {
    if (_currentPanel == targetPanel) return;

    // Save where we came from
    _navigationHistory.Push(_currentPanel);

    // Fade Out Current
    if (_currentPanel != null)
      _currentPanel.AddToClassList("hidden");

    // Fade In Target
    targetPanel.RemoveFromClassList("hidden");
    _currentPanel = targetPanel;
  }

  private void GoBack()
  {
    if (_navigationHistory.Count > 0)
    {
      // Fade Out Current
      _currentPanel.AddToClassList("hidden");

      // Get previous panel
      VisualElement previousPanel = _navigationHistory.Pop();

      // Fade In Previous
      previousPanel.RemoveFromClassList("hidden");
      _currentPanel = previousPanel;
    }
  }

  private bool HandleEscapeAction()
  {
    if (_navigationHistory.Count > 0)
    {
      print("go back");
      GoBack();
      return true;
    }
    return false;
  }
}