// GraphicsSettings.cs
using System.Linq;
using UnityEngine;

[System.Serializable]
public class GraphicsSettings : IGameSettings
{
  // Private keys so nothing else in the game cares about them
  private const string PrefsQualityKey = "Settings_Quality";
  private const string PrefsResolutionKey = "Settings_Resolution";
  private const string PrefsFullscreenKey = "Settings_Fullscreen";
  private const string PrefsBrightnessKey = "Settings_Brightness";

  // The actual payload data
  public int QualityIndex;
  public int ResolutionIndex;
  public bool IsFullscreen;
  public float Brightness;

  // Runtime data specific to graphics
  public Resolution[] AvailableResolutions { get; private set; }

  public void InitDefaults()
  {
    // Graphics handles its own complex initialization
    AvailableResolutions = Screen.resolutions
        .GroupBy(res => new { res.width, res.height })
        .Select(g => g.OrderByDescending(res => res.refreshRateRatio.value).First())
        .ToArray();

    QualityIndex = 1; // Medium
    ResolutionIndex = Mathf.Max(0, AvailableResolutions.Length - 1);
    IsFullscreen = true;
    Brightness = 100f;
  }

  public void Load()
  {
    QualityIndex = PlayerPrefs.GetInt(PrefsQualityKey, QualityIndex);

    ResolutionIndex = PlayerPrefs.GetInt(PrefsResolutionKey, ResolutionIndex);
    if (ResolutionIndex >= AvailableResolutions.Length)
      ResolutionIndex = Mathf.Max(0, AvailableResolutions.Length - 1);

    IsFullscreen = PlayerPrefs.GetInt(PrefsFullscreenKey, IsFullscreen ? 1 : 0) == 1;
    Brightness = PlayerPrefs.GetFloat(PrefsBrightnessKey, Brightness);
  }

  public void Save()
  {
    PlayerPrefs.SetInt(PrefsQualityKey, QualityIndex);
    PlayerPrefs.SetInt(PrefsResolutionKey, ResolutionIndex);
    PlayerPrefs.SetInt(PrefsFullscreenKey, IsFullscreen ? 1 : 0);
    PlayerPrefs.SetFloat(PrefsBrightnessKey, Brightness);
    PlayerPrefs.Save();
  }

  public void Apply()
  {
    QualitySettings.SetQualityLevel(QualityIndex, true);

    if (AvailableResolutions != null && AvailableResolutions.Length > 0)
    {
      Resolution res = AvailableResolutions[ResolutionIndex];
      Screen.SetResolution(res.width, res.height, IsFullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed);
    }

    // Apply PostProcessing brightness here
  }

  // A helper method so UI panels can safely create a temporary copy to edit
  public GraphicsSettings Clone()
  {
    return (GraphicsSettings)MemberwiseClone();
  }

  public bool Equals(GraphicsSettings other)
  {
    return QualityIndex == other.QualityIndex &&
           ResolutionIndex == other.ResolutionIndex &&
           IsFullscreen == other.IsFullscreen &&
           Mathf.Approximately(Brightness, other.Brightness);
  }
}