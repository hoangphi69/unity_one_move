using System.Collections.Generic;
using System.Threading.Tasks;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class GameAudioManagger : MonoBehaviour
{
  public static GameAudioManagger Instance { get; private set; }

  private EventInstance ambienceChannel;
  private EventReference ambienceTrack;

  private EventInstance musicChannel;
  private EventReference musicTrack;

  private List<EventInstance> eventInstances = new();

  [Header("Volume")]
  [Range(0, 1)] public float masterVolume = 1;
  [Range(0, 1)] public float musicVolume = 1;
  [Range(0, 1)] public float ambienceVolume = 1;
  [Range(0, 1)] public float SFXVolume = 1;

  void Awake()
  {
    if (Instance == null) Instance = this;
  }

  void OnDestroy()
  {
    foreach (var instance in eventInstances)
    {
      if (instance.isValid())
      {
        instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        instance.release();
      }
    }
  }

  // --- START Ambience channel ---

  public void PlaySFX(EventReference audio, Vector3 position)
  {
    RuntimeManager.PlayOneShot(audio, position);
  }

  public async Task PlaySFXAsync(EventReference audio, Vector3 position)
  {
    if (audio.IsNull) return;

    var instance = RuntimeManager.CreateInstance(audio);
    instance.getDescription(out EventDescription description);
    description.getLength(out int length);

    instance.start();
    instance.release();

    await Task.Delay(length);
  }

  // --- END Ambience channel ---

  // --- START Ambience channel ---

  public void PlayAmbience(EventReference audio)
  {
    if (audio.IsNull) return;

    if (ambienceChannel.isValid() && ambienceTrack.Guid == audio.Guid)
    {
      // Continue playing if the SAME track
      ambienceChannel.setVolume(1);
      ambienceChannel.getPlaybackState(out PLAYBACK_STATE state);
      if (state == PLAYBACK_STATE.STOPPED ||
      state == PLAYBACK_STATE.STOPPING) ambienceChannel.start();
    }
    else
    {
      // Clean up an play NEW track
      StopAmbience();
      ambienceTrack = audio;
      ambienceChannel = CreateEventInstance(audio);
      ambienceChannel.setVolume(1);
      ambienceChannel.start();
    }
  }

  public void StopAmbience()
  {
    if (!ambienceChannel.isValid()) return;
    ambienceChannel.stop(0);
    ambienceChannel.release();
    ambienceTrack = new();
  }

  // --- END Ambience channel ---

  // --- START Music channel ---

  public void PlayMusic(EventReference audio)
  {
    if (audio.IsNull) return;

    if (musicChannel.isValid() && musicTrack.Guid == audio.Guid)
    {
      // Continue playing if the SAME track
      musicChannel.setVolume(1);
      musicChannel.getPlaybackState(out PLAYBACK_STATE state);
      if (state == PLAYBACK_STATE.STOPPED ||
      state == PLAYBACK_STATE.STOPPING) musicChannel.start();
    }
    else
    {
      // Clean up an play NEW track
      StopMusic();
      musicTrack = audio;
      musicChannel = CreateEventInstance(audio);
      musicChannel.setVolume(1);
      musicChannel.start();
    }
  }

  public void LowerMusic()
  {
    if (!musicChannel.isValid()) return;
    musicChannel.setVolume(0.2f);
  }

  public void StopMusic()
  {
    if (!musicChannel.isValid()) return;
    musicChannel.stop(0);
    musicChannel.release();
    musicTrack = new();
  }

  // --- END Music channel ---

  EventInstance CreateEventInstance(EventReference eventReference)
  {
    EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
    eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(Vector3.zero));
    eventInstances.Add(eventInstance);
    return eventInstance;
  }
}