using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class OptionsPanel : NavigationPanel
{
  [Header("Graphics Settings UI")]
  [SerializeField] private InputField qualityField;
  [SerializeField] private HorizontalSelector qualitySelector;

  [SerializeField] private InputField resolutionField;
  [SerializeField] private HorizontalSelector resolutionSelector;

  [SerializeField] private InputField fullscreenField;
  [SerializeField] private SwitchToggle fullscreenToggle;

  [Header("Audio Settings UI")]
  [SerializeField] private InputField masterField;
  [SerializeField] private CustomSlider masterSlider;

  [SerializeField] private InputField musicField;
  [SerializeField] private CustomSlider musicSlider;

  [SerializeField] private InputField ambienceField;
  [SerializeField] private CustomSlider ambienceSlider;

  [SerializeField] private InputField sfxField;
  [SerializeField] private CustomSlider sfxSlider;

  [Header("Footer Buttons")]
  [SerializeField] private Button saveButton;
  [SerializeField] private Button resetButton;
  [SerializeField] private Button backButton;

  private GraphicsSettings currentGraphics;
  private GraphicsSettings defaultGraphics;

  private AudioSettings currentAudio;
  private AudioSettings defaultAudio;

  private bool HasUnsavedChanges => !currentGraphics.Equals(GameSettingsManager.Instance.Graphics) || !currentAudio.Equals(GameSettingsManager.Instance.Audio);
  private bool IsNotDefault => !currentGraphics.Equals(defaultGraphics) || !currentAudio.Equals(defaultAudio);

  void OnEnable()
  {
    currentGraphics = GameSettingsManager.Instance.Graphics.Clone();
    defaultGraphics = new();
    defaultGraphics.InitDefaults();

    currentAudio = GameSettingsManager.Instance.Audio.Clone();
    defaultAudio = new();
    defaultAudio.InitDefaults();

    InitializeInputs();

    // Listeners
    qualitySelector.OnValueChanged.AddListener(val => { currentGraphics.QualityIndex = val; HandleGraphicsChange(); });
    resolutionSelector.OnValueChanged.AddListener(val => { currentGraphics.ResolutionIndex = val; HandleGraphicsChange(); });
    fullscreenToggle.OnValueChanged.AddListener(val => { currentGraphics.IsFullscreen = val; HandleGraphicsChange(); });

    masterSlider.OnValueChanged.AddListener(val => { currentAudio.MasterVolume = val; HandleAudioChange(); });
    musicSlider.OnValueChanged.AddListener(val => { currentAudio.MusicVolume = val; HandleAudioChange(); });
    ambienceSlider.OnValueChanged.AddListener(val => { currentAudio.AmbienceVolume = val; HandleAudioChange(); });
    sfxSlider.OnValueChanged.AddListener(val => { currentAudio.SFXVolume = val; HandleAudioChange(); });

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

    masterSlider.OnValueChanged.RemoveAllListeners();
    musicSlider.OnValueChanged.RemoveAllListeners();
    ambienceSlider.OnValueChanged.RemoveAllListeners();
    sfxSlider.OnValueChanged.RemoveAllListeners();

    saveButton.onClick.RemoveListener(SaveSettings);
    resetButton.onClick.RemoveListener(OnResetClicked);
    backButton.onClick.RemoveListener(OnBackClicked);
  }

  private void InitializeInputs()
  {
    qualitySelector.Setup(QualitySettings.names.ToList(), currentGraphics.QualityIndex);

    var resolutions = GameSettingsManager.Instance.Graphics.AvailableResolutions;
    List<string> resStrings = new List<string>();
    for (int i = 0; i < resolutions.Length; i++) resStrings.Add($"{resolutions[i].width} x {resolutions[i].height}");

    resolutionSelector.Setup(resStrings, currentGraphics.ResolutionIndex);
    fullscreenToggle.SetValueWithoutNotify(currentGraphics.IsFullscreen);

    masterSlider.Setup(0f, 1f, currentAudio.MasterVolume);
    musicSlider.Setup(0f, 1f, currentAudio.MusicVolume);
    ambienceSlider.Setup(0f, 1f, currentAudio.AmbienceVolume);
    sfxSlider.Setup(0f, 1f, currentAudio.SFXVolume);
  }

  private void HandleGraphicsChange()
  {
    // Preview the draft on the screen instantly
    GameSettingsManager.Instance.PreviewSettings(currentGraphics);
    UpdateUI();
  }

  private void HandleAudioChange()
  {
    GameSettingsManager.Instance.PreviewSettings(currentAudio);
    UpdateUI();
  }

  private void SaveSettings()
  {
    // Copy the draft values into the Manager's live master copy
    GameSettingsManager.Instance.Graphics.QualityIndex = currentGraphics.QualityIndex;
    GameSettingsManager.Instance.Graphics.ResolutionIndex = currentGraphics.ResolutionIndex;
    GameSettingsManager.Instance.Graphics.IsFullscreen = currentGraphics.IsFullscreen;
    GameSettingsManager.Instance.SaveSettings(GameSettingsManager.Instance.Graphics);

    GameSettingsManager.Instance.Audio.MasterVolume = currentAudio.MasterVolume;
    GameSettingsManager.Instance.Audio.MusicVolume = currentAudio.MusicVolume;
    GameSettingsManager.Instance.Audio.AmbienceVolume = currentAudio.AmbienceVolume;
    GameSettingsManager.Instance.Audio.SFXVolume = currentAudio.SFXVolume;
    GameSettingsManager.Instance.SaveSettings(GameSettingsManager.Instance.Audio);

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
    HandleGraphicsChange();

    currentAudio = defaultAudio.Clone();
    masterSlider.SetValueWithoutNotify(currentAudio.MasterVolume);
    musicSlider.SetValueWithoutNotify(currentAudio.MusicVolume);
    ambienceSlider.SetValueWithoutNotify(currentAudio.AmbienceVolume);
    sfxSlider.SetValueWithoutNotify(currentAudio.SFXVolume);
    HandleAudioChange();
  }

  private void UpdateUI()
  {
    qualityField.SetHighlight(currentGraphics.QualityIndex != defaultGraphics.QualityIndex);
    resolutionField.SetHighlight(currentGraphics.ResolutionIndex != defaultGraphics.ResolutionIndex);
    fullscreenField.SetHighlight(currentGraphics.IsFullscreen != defaultGraphics.IsFullscreen);

    masterField.SetHighlight(!Mathf.Approximately(currentAudio.MasterVolume, defaultAudio.MasterVolume));
    musicField.SetHighlight(!Mathf.Approximately(currentAudio.MusicVolume, defaultAudio.MusicVolume));
    ambienceField.SetHighlight(!Mathf.Approximately(currentAudio.AmbienceVolume, defaultAudio.AmbienceVolume));
    sfxField.SetHighlight(!Mathf.Approximately(currentAudio.SFXVolume, defaultAudio.SFXVolume));

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