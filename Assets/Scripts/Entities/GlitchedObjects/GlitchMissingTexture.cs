using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GlitchedMissingTexture : MonoBehaviour
{
  [SerializeField] private string id;
  [SerializeField] private GameObject normalPrefab;

  [Header("Glitch Animation Settings")]
  [SerializeField] private float glitchDuration = 1.0f;
  [SerializeField] private float glitchStrength = 0.3f;
  [SerializeField] private int glitchVibrato = 30; // Higher vibrato = faster back-and-forth

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
    // Check if this event is meant for this specific door and it's being turned 'on'
    if (incomingId == id && state == true && !isFixed)
    {
      isFixed = true;
      PlayGlitchFixAnimation();
      GameAudioManager.Instance.PlaySFX(AudioTag.GlitchFix);
    }
  }

  void PlayGlitchFixAnimation()
  {
    transform.DOShakePosition(glitchDuration, glitchStrength, glitchVibrato)
        .SetEase(Ease.InBounce) // Adds to the erratic mechanical feel
        .OnComplete(SwapToNormal); // Runs exactly when the tween finishes
  }

  void SwapToNormal()
  {
    if (normalPrefab != null)
    {
      GameObject normalObject = Instantiate(normalPrefab, transform.position, transform.rotation);
      SceneManager.MoveGameObjectToScene(normalObject, gameObject.scene);
      normalObject.SetActive(true);
    }
    else Debug.LogWarning("Normal object reference is missing on " + gameObject.name);

    // Disable this glitched object
    gameObject.SetActive(false);
  }
}