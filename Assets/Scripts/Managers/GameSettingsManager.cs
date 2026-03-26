using System.Collections.Generic;
using UnityEngine;

public class GameSettingsManager : MonoBehaviour
{
  public static GameSettingsManager Instance { get; private set; }

  public GraphicsSettings Graphics { get; private set; }
  public AudioSettings Audio { get; private set; }

  private List<IGameSettings> categories = new List<IGameSettings>();

  private void Awake()
  {
    if (Instance != null && Instance != this)
    {
      Destroy(gameObject);
      return;
    }
    Instance = this;
  }

  public void Initialize()
  {
    Graphics = new GraphicsSettings();
    Audio = new AudioSettings();

    categories.Add(Graphics);
    categories.Add(Audio);

    foreach (var settings in categories)
    {
      settings.InitDefaults();
      settings.Load();
      settings.Apply();
    }
  }

  public void PreviewSettings(IGameSettings settings)
  {
    settings.Apply();
  }

  public void SaveSettings(IGameSettings settings)
  {
    settings.Save();
    settings.Apply();
  }

  public void RevertSettings(IGameSettings settings)
  {
    settings.Load();
    settings.Apply();
  }
}