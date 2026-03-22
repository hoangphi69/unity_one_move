using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class OptionsPanel : NavigationPanel
{
  [Header("Settings UI")]
  [SerializeField] private InputField qualityField;
  [SerializeField] private HorizontalSelector qualitySelector;

  [SerializeField] private InputField resolutionField;
  [SerializeField] private HorizontalSelector resolutionSelector;

  [SerializeField] private InputField fullscreenField;
  [SerializeField] private SwitchToggle fullscreenToggle;

  [Header("Footer Buttons")]
  [SerializeField] private Button saveButton;
  [SerializeField] private Button resetButton;
  [SerializeField] private Button backButton;

  private GraphicsSettings currentGraphics;
  private GraphicsSettings defaultGraphics;

  private bool HasUnsavedChanges => !currentGraphics.Equals(GameSettingsManager.Instance.Graphics);
  private bool IsNotDefault => !currentGraphics.Equals(defaultGraphics);

  void OnEnable()
  {
    currentGraphics = GameSettingsManager.Instance.Graphics.Clone();

    defaultGraphics = new GraphicsSettings();
    defaultGraphics.InitDefaults();

    InitializeSelectors();

    // Listeners
    qualitySelector.OnValueChanged.AddListener(val => { currentGraphics.QualityIndex = val; HandleSettingChange(); });
    resolutionSelector.OnValueChanged.AddListener(val => { currentGraphics.ResolutionIndex = val; HandleSettingChange(); });
    fullscreenToggle.OnValueChanged.AddListener(val => { currentGraphics.IsFullscreen = val; HandleSettingChange(); });

    saveButton.onClick.AddListener(SaveSettings);
    resetButton.onClick.AddListener(OnResetClicked);
    backButton.onClick.AddListener(OnBackClicked);

    UpdateUI();
  }

  void OnDisable()
  {
    qualitySelector.OnValueChanged.RemoveAllListeners();
    resolutionSelector.OnValueChanged.RemoveAllListeners();
    fullscreenToggle.OnValueChanged.RemoveAllListeners();

    saveButton.onClick.RemoveListener(SaveSettings);
    resetButton.onClick.RemoveListener(OnResetClicked);
    backButton.onClick.RemoveListener(OnBackClicked);
  }

  private void InitializeSelectors()
  {
    qualitySelector.Setup(QualitySettings.names.ToList(), currentGraphics.QualityIndex);

    var resolutions = GameSettingsManager.Instance.Graphics.AvailableResolutions;
    List<string> resStrings = new List<string>();
    for (int i = 0; i < resolutions.Length; i++)
    {
      resStrings.Add($"{resolutions[i].width} x {resolutions[i].height}");
    }

    resolutionSelector.Setup(resStrings, currentGraphics.ResolutionIndex);
    fullscreenToggle.SetValueWithoutNotify(currentGraphics.IsFullscreen);
  }

  private void HandleSettingChange()
  {
    // Preview the draft on the screen instantly
    GameSettingsManager.Instance.PreviewSettings(currentGraphics);
    UpdateUI();
  }

  private void SaveSettings()
  {
    // Copy the draft values into the Manager's live master copy
    GameSettingsManager.Instance.Graphics.QualityIndex = currentGraphics.QualityIndex;
    GameSettingsManager.Instance.Graphics.ResolutionIndex = currentGraphics.ResolutionIndex;
    GameSettingsManager.Instance.Graphics.IsFullscreen = currentGraphics.IsFullscreen;

    // Tell the Manager to save and apply the master copy
    GameSettingsManager.Instance.SaveSettings(GameSettingsManager.Instance.Graphics);
    UpdateUI();
  }

  private void OnResetClicked()
  {
    // Calling your newly separated Confirm Overlay
    ConfirmOverlayUIController.Instance.Show(
        "Reset Settings",
        "Are you sure you want to reset all settings to their default values?",
        onConfirm: RevertToDefault
    );
  }

  private void RevertToDefault()
  {
    currentGraphics = defaultGraphics.Clone();

    qualitySelector.SetValueWithoutNotify(currentGraphics.QualityIndex);
    resolutionSelector.SetValueWithoutNotify(currentGraphics.ResolutionIndex);
    fullscreenToggle.SetValueWithoutNotify(currentGraphics.IsFullscreen);

    HandleSettingChange();
  }

  private void UpdateUI()
  {
    qualityField.SetHighlight(currentGraphics.QualityIndex != defaultGraphics.QualityIndex);
    resolutionField.SetHighlight(currentGraphics.ResolutionIndex != defaultGraphics.ResolutionIndex);
    fullscreenField.SetHighlight(currentGraphics.IsFullscreen != defaultGraphics.IsFullscreen);

    saveButton.gameObject.SetActive(HasUnsavedChanges);
    resetButton.gameObject.SetActive(IsNotDefault);
  }

  private void OnBackClicked()
  {
    if (!HasUnsavedChanges) ClosePanel();
    else
    {
      ConfirmOverlayUIController.Instance.Show(
          "Unsaved Changes",
          "You have unsaved changes. Would you like to save before exiting?",
          onConfirm: () =>
          {
            SaveSettings();
            ClosePanel();
          },
          onCancel: () =>
          {
            GameSettingsManager.Instance.RevertSettings(GameSettingsManager.Instance.Graphics);
            ClosePanel();
          }
      );
    }
  }
}