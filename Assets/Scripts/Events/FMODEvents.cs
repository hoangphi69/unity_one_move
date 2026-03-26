using FMODUnity;
using UnityEngine;

public class FMODEvents : MonoBehaviour
{
  [field: Header("Music")]
  [field: SerializeField] public EventReference TitleMusic { get; private set; }
  [field: SerializeField] public EventReference Ch1RadioMusic { get; private set; }

  [field: Header("Ambience")]
  [field: SerializeField] public EventReference DormAfternoon { get; private set; }
  [field: SerializeField] public EventReference LibraryAmbience { get; private set; }

  [field: Header("SFX")]
  [field: SerializeField] public EventReference Footstep { get; private set; }
  [field: SerializeField] public EventReference DoorOpen { get; private set; }
  [field: SerializeField] public EventReference RadioToggle { get; private set; }

  public static FMODEvents Instance { get; private set; }

  void Awake()
  {
    if (Instance == null) Instance = this;
    else Destroy(gameObject);
  }
}