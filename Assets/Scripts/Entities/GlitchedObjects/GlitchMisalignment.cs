using UnityEngine;
using DG.Tweening;

public class GlitchMisalignment : MonoBehaviour
{
  [SerializeField] private string id;
  [SerializeField] private GameObject normalObject;

  [Header("Glitch Animation Settings")]
  [SerializeField] private float glitchDuration = 1.0f;
  [SerializeField] private float positionGlitchStrength = 0.5f;
  [SerializeField] private Vector3 rotationGlitchStrength = new Vector3(90f, 90f, 90f);
  [SerializeField] private int glitchVibrato = 40; // Higher vibrato = faster glitching

  private bool isFixed = false;

  void OnEnable()
  {
    GameEventsManager.Instance.interactEvents.OnSwitch += HandleSwitchEvent;
  }

  void OnDisable()
  {
    GameEventsManager.Instance.interactEvents.OnSwitch -= HandleSwitchEvent;
  }

  void HandleSwitchEvent(string incomingId, bool state)
  {
    // Check if this event is meant for this specific object and it's being turned 'on'
    if (incomingId == id && state == true && !isFixed)
    {
      isFixed = true;
      PlayGlitchFixAnimation();
    }
  }

  void PlayGlitchFixAnimation()
  {
    // Create a sequence to play position and rotation shakes at the exact same time
    Sequence glitchSequence = DOTween.Sequence();

    // .Join adds the tween to the sequence and plays it simultaneously with the others
    glitchSequence.Join(transform.DOShakePosition(glitchDuration, positionGlitchStrength, glitchVibrato));
    glitchSequence.Join(transform.DOShakeRotation(glitchDuration, rotationGlitchStrength, glitchVibrato));

    // Trigger the swap only when the entire sequence is finished
    glitchSequence.OnComplete(SwapToNormal);
  }

  void SwapToNormal()
  {
    if (normalObject != null) normalObject.SetActive(true);
    else Debug.LogWarning("Normal object reference is missing on " + gameObject.name);

    // Disable this glitched object
    gameObject.SetActive(false);
  }
}