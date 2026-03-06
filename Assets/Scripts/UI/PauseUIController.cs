using System.Collections.Generic;
using System.Linq;
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
  private HorizontalSelector _graphicsQualitySelector;
  private HorizontalSelector _graphicsResolutionSelector;
  private SwitchToggle _graphicsFullscreenToggle;
  private IntSlider _graphicsBrightnessSlider;
  private Button _graphicsBackButton;

  private Resolution[] _resolutions;

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
    _graphicsQualitySelector = _graphicsPanel.Q<HorizontalSelector>("quality-selector");
    _graphicsResolutionSelector = _graphicsPanel.Q<HorizontalSelector>("resolution-selector");
    _graphicsFullscreenToggle = _graphicsPanel.Q<SwitchToggle>("fullscreen-toggle");
    _graphicsBrightnessSlider = _graphicsPanel.Q<IntSlider>("brightness-slider");
    _graphicsBackButton = _graphicsPanel.Q<Button>("back");

    InitializeQuality();
    InitializeResolutions();
    InitializeFullScreen();
    InitializeBrightness();
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

    _graphicsQualitySelector.RegisterValueChangedCallback(ChangeQuality);
    _graphicsResolutionSelector.RegisterValueChangedCallback(ChangeResolution);
    _graphicsFullscreenToggle.RegisterValueChangedCallback(ChangeFullScreen);
    _graphicsBrightnessSlider.RegisterValueChangedCallback(ChangeBrightness);
    _graphicsBackButton.clicked += GoBack;

    InputActionsManager.Instance.OnUIBackRequested += HandleEscapeAction;
  }

  void OnDisable()
  {
    _resumeButton.clicked -= GameSceneManager.Instance.TogglePause;
    _restartButton.clicked -= GameSceneManager.Instance.RestartCurrentLevel;
    _returnToTitleButton.clicked -= GameSceneManager.Instance.ReturnToTitle;

    _optionsBackButton.clicked -= GoBack;

    _graphicsResolutionSelector.UnregisterValueChangedCallback(ChangeResolution);
    _graphicsQualitySelector.UnregisterValueChangedCallback(ChangeQuality);
    _graphicsFullscreenToggle.UnregisterValueChangedCallback(ChangeFullScreen);
    _graphicsBrightnessSlider.UnregisterValueChangedCallback(ChangeBrightness);
    _graphicsBackButton.clicked -= GoBack;

    InputActionsManager.Instance.OnUIBackRequested -= HandleEscapeAction;
  }

  void ChangeQuality(ChangeEvent<string> evt)
  {
    QualitySettings.SetQualityLevel(_graphicsQualitySelector.selectedIndex);
  }

  void ChangeResolution(ChangeEvent<string> evt)
  {
    Resolution r = _resolutions[_graphicsResolutionSelector.selectedIndex];
    Screen.SetResolution(r.width, r.height, Screen.fullScreen);
  }

  void ChangeFullScreen(ChangeEvent<bool> evt)
  {
    Screen.fullScreen = evt.newValue;
  }

  void ChangeBrightness(ChangeEvent<int> evt)
  {
    Screen.brightness = evt.newValue * 0.01f;
  }

  void InitializeBrightness()
  {
    _graphicsBrightnessSlider.minValue = 0;
    _graphicsBrightnessSlider.maxValue = 100;
    _graphicsBrightnessSlider.fill = true;
    _graphicsBrightnessSlider.SetValueWithoutNotify((int)(Screen.brightness * 100));
  }

  void InitializeFullScreen()
  {
    _graphicsFullscreenToggle.SetValueWithoutNotify(Screen.fullScreen);
  }

  void InitializeQuality()
  {
    _graphicsQualitySelector.choices = QualitySettings.names.ToList();
    _graphicsQualitySelector.selectedIndex = QualitySettings.GetQualityLevel();
  }

  void InitializeResolutions()
  {
    _resolutions = Screen.resolutions
            .Select(r => new Resolution { width = r.width, height = r.height })
            .Distinct()
            .ToArray();

    // Convert the array of structs into a list of strings for the UI
    _graphicsResolutionSelector.choices = _resolutions
        .Select(r => $"{r.width}x{r.height}")
        .ToList();

    // Find current resolution
    for (int i = 0; i < _resolutions.Length; i++)
    {
      if (_resolutions[i].width == Screen.width && _resolutions[i].height == Screen.height)
      {
        _graphicsResolutionSelector.selectedIndex = i;
        break;
      }
    }
  }

  void ShowRootPanel()
  {
    _navigationHistory.Clear();

    // Reset: Hide all, Show Main
    _optionsPanel.AddToClassList("hidden");
    _graphicsPanel.AddToClassList("hidden");

    _mainPanel.RemoveFromClassList("hidden");
    _currentPanel = _mainPanel;
  }

  // --- Navigation Logic ---

  void SwitchPanel(VisualElement targetPanel)
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

  void GoBack()
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

  bool HandleEscapeAction()
  {
    if (_navigationHistory.Count > 0)
    {
      GoBack();
      return true;
    }
    return false;
  }
}