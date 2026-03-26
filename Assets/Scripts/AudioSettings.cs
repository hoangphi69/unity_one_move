using UnityEngine;
using FMODUnity;

[System.Serializable]
public class AudioSettings : IGameSettings
{
  private const string PrefsMasterKey = "Settings_Audio_Master";
  private const string PrefsMusicKey = "Settings_Audio_Music";
  private const string PrefsAmbienceKey = "Settings_Audio_Ambience";
  private const string PrefsSFXKey = "Settings_Audio_SFX";

  public float MasterVolume;
  public float MusicVolume;
  public float AmbienceVolume;
  public float SFXVolume;

  public void InitDefaults()
  {
    MasterVolume = 1.0f;
    MusicVolume = 1.0f;
    AmbienceVolume = 1.0f;
    SFXVolume = 1.0f;
  }

  public void Load()
  {
    MasterVolume = PlayerPrefs.GetFloat(PrefsMasterKey, MasterVolume);
    MusicVolume = PlayerPrefs.GetFloat(PrefsMusicKey, MusicVolume);
    AmbienceVolume = PlayerPrefs.GetFloat(PrefsAmbienceKey, AmbienceVolume);
    SFXVolume = PlayerPrefs.GetFloat(PrefsSFXKey, SFXVolume);
  }

  public void Save()
  {
    PlayerPrefs.SetFloat(PrefsMasterKey, MasterVolume);
    PlayerPrefs.SetFloat(PrefsMusicKey, MusicVolume);
    PlayerPrefs.SetFloat(PrefsAmbienceKey, AmbienceVolume);
    PlayerPrefs.SetFloat(PrefsSFXKey, SFXVolume);
    PlayerPrefs.Save();
  }

  public void Apply()
  {
    RuntimeManager.GetBus("bus:/").setVolume(MasterVolume);
    RuntimeManager.GetBus("bus:/Music").setVolume(MusicVolume);
    RuntimeManager.GetBus("bus:/Ambience").setVolume(AmbienceVolume);
    RuntimeManager.GetBus("bus:/SFX").setVolume(SFXVolume);
  }

  public AudioSettings Clone()
  {
    return (AudioSettings)MemberwiseClone();
  }

  public bool Equals(AudioSettings other)
  {
    return Mathf.Approximately(MasterVolume, other.MasterVolume) &&
           Mathf.Approximately(MusicVolume, other.MusicVolume) &&
           Mathf.Approximately(AmbienceVolume, other.AmbienceVolume) &&
           Mathf.Approximately(SFXVolume, other.SFXVolume);
  }
}