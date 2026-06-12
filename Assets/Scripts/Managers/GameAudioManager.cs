using System.Collections.Generic;
using System.Threading.Tasks;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public enum AudioTag
{
  None, // default/empty state

  // MUSIC
  tile_menu,
  vn_theme,
  ch1_library,
  ch2_cafe,

  // AMBIENCE
  ambient_cafe,
  ambient_library,
  ambient_dorm_morning,
  ambient_dorm_afternoon,

  // SFX (Gameplay)
  bump,
  failed,
  door,
  push,
  slide,
  step,
  glitch_fix,
  switch_glitch,
  collect,

  // SFX (UI)
  restart,
  pause,
  dialogue_on,
  dialogue_off,
  camera_snap,
  calendar_flip,
  clock_ticking,
}

[System.Serializable]
public struct AudioMap
{
  public AudioTag tag;
  public EventReference audio;
}

public class GameAudioManager : MonoBehaviour
{
  public static GameAudioManager Instance { get; private set; }

  private EventInstance ambienceChannel;
  private EventReference ambienceTrack;

  private EventInstance musicChannel;
  private EventReference musicTrack;

  private List<EventInstance> eventInstances = new();

  [SerializeField] private List<AudioMap> tracks = new();
  private readonly Dictionary<AudioTag, EventReference> lookup = new();
  [HideInInspector] public bool hasDuplicateTags = false;

  void Awake()
  {
    if (Instance == null) Instance = this;
    InitializeTrackDictionary();
  }

  void OnDestroy()
  {
    StopMusic();
    StopAmbience();
    foreach (var instance in eventInstances)
    {
      if (instance.isValid())
      {
        instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        instance.release();
      }
    }
  }

  void OnValidate()
  {
    hasDuplicateTags = false;
    HashSet<AudioTag> seenTags = new HashSet<AudioTag>();

    foreach (var mapping in tracks)
    {
      if (mapping.tag == AudioTag.None) continue;

      // HashSet.Add returns false if the item is already in the set
      if (!seenTags.Add(mapping.tag))
      {
        hasDuplicateTags = true;
        break; // Stop checking, we already know there's at least one duplicate
      }
    }
  }

  private void InitializeTrackDictionary()
  {
    lookup.Clear();
    foreach (var map in tracks)
    {
      if (map.tag == AudioTag.None) continue;
      if (!lookup.ContainsKey(map.tag)) lookup.Add(map.tag, map.audio);
    }
  }

  private EventReference GetAudio(AudioTag tag)
  {
    if (tag == AudioTag.None) return new();

    if (lookup.TryGetValue(tag, out EventReference audio)) return audio;

    Debug.LogWarning($"[GameAudioManager] No EventReference mapped for tag: {tag}");
    return new();
  }


  // SFX METHODS
  public void PlaySFX(AudioTag tag) => PlaySFX(GetAudio(tag));
  public void PlaySFX(AudioTag tag, string key, float value) => PlaySFX(GetAudio(tag), key, value);
  public void PlaySFX(AudioTag tag, string key, string value) => PlaySFX(GetAudio(tag), key, value);

  public void PlaySFX(EventReference audio)
  {
    RuntimeManager.PlayOneShot(audio);
  }

  public void PlaySFX(EventReference audio, string key, float value)
  {
    var instance = RuntimeManager.CreateInstance(audio);
    instance.setParameterByName(key, value);
    instance.start();
    instance.release();
  }

  public void PlaySFX(EventReference audio, string key, string value)
  {
    var instance = RuntimeManager.CreateInstance(audio);
    instance.setParameterByNameWithLabel(key, value);
    instance.start();
    instance.release();
  }

  public Task PlaySFXAsync(AudioTag tag) => PlaySFXAsync(GetAudio(tag));
  public Task PlaySFXAsync(AudioTag tag, string key, float value) => PlaySFXAsync(GetAudio(tag), key, value);
  public Task PlaySFXAsync(AudioTag tag, string key, string value) => PlaySFXAsync(GetAudio(tag), key, value);

  public async Task PlaySFXAsync(EventReference audio)
  {
    var instance = RuntimeManager.CreateInstance(audio);
    instance.getDescription(out EventDescription description);
    description.getLength(out int length);

    instance.start();
    instance.release();

    await Task.Delay(length);
  }

  public async Task PlaySFXAsync(EventReference audio, string key, float value)
  {
    var instance = RuntimeManager.CreateInstance(audio);

    // Set the continuous or discrete parameter
    instance.setParameterByName(key, value);

    // Get the length of the event's timeline
    instance.getDescription(out EventDescription description);
    description.getLength(out int length);

    instance.start();
    instance.release();

    await Task.Delay(length);
  }

  public async Task PlaySFXAsync(EventReference audio, string key, string value)
  {
    var instance = RuntimeManager.CreateInstance(audio);

    // Set the labeled parameter
    instance.setParameterByNameWithLabel(key, value);

    // Get the length of the event's timeline
    instance.getDescription(out EventDescription description);
    description.getLength(out int length);

    instance.start();
    instance.release();

    // Wait for the duration of the timeline
    await Task.Delay(length);
  }


  // AMBIENCE METHODS
  public void PlayAmbience(AudioTag tag) => PlayAmbience(GetAudio(tag));

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


  // MUSIC METHODS

  public void PlayMusic(AudioTag tag) => PlayMusic(GetAudio(tag));

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

  public void SetMusicParameter(string paramName, float paramValue)
  {
    if (!musicChannel.isValid()) return;
    musicChannel.setParameterByName(paramName, paramValue);
  }

  public void SetMusicVolume(float volume)
  {
    if (!musicChannel.isValid()) return;
    musicChannel.setVolume(volume);
  }

  public void StopMusic()
  {
    if (!musicChannel.isValid()) return;
    musicChannel.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    musicChannel.release();
    musicTrack = new();
  }


  // PAUSE METHODS

  private EventInstance pauseStaticSFX;

  public void PlayPauseStaticAudio()
  {
    EventReference audio = GetAudio(AudioTag.pause);
    if (audio.IsNull) return;

    pauseStaticSFX = CreateEventInstance(audio);
    pauseStaticSFX.setParameterByName("pause", 0f);
    pauseStaticSFX.start();
  }

  public void EndPauseStaticAudio()
  {
    if (!pauseStaticSFX.isValid()) return;
    pauseStaticSFX.setParameterByName("pause", 1f);
    pauseStaticSFX.release();
    pauseStaticSFX.clearHandle();
  }

  EventInstance CreateEventInstance(EventReference eventReference)
  {
    EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
    eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(Vector3.zero));
    eventInstances.Add(eventInstance);
    return eventInstance;
  }
}