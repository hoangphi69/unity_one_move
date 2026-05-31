using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using System.Threading; // Required for CancellationToken

public class GlitchFreeze : MonoBehaviour
{
  [SerializeField] private string id;
  [SerializeField] private GameObject normalObject;

  [Header("Freeze Settings")]
  [Tooltip("The local axis the door rotates around (usually 0, 1, 0 for Y-axis)")]
  [SerializeField] private Vector3 rotationAxis = Vector3.up;
  [Tooltip("How many degrees the door is stuck at when it spawns")]
  [SerializeField] private float stuckAngle = 200f;

  [Header("Idle Glitch Animation")]
  [SerializeField] private float minTwitchInterval = 1.5f;
  [SerializeField] private float maxTwitchInterval = 4.0f;
  [SerializeField] private float idleAnimationReplay = 2;
  [Tooltip("How violently it twitches trying to rotate")]
  [SerializeField] private Vector3 twitchStrength = new Vector3(0, 5f, 0);
  [SerializeField] private float twitchDuration = 0.2f;

  [Header("Fix Animation Settings")]
  [SerializeField] private float fixDuration = 1.0f;
  [SerializeField] private Ease fixEase = Ease.InOutBack;

  private bool isFixed = false;
  private CancellationTokenSource cts; // Controls the async cancellation

  void Awake()
  {
    // Instantly snap the door to its frozen 200-degree state on startup
    transform
      .DORotate(rotationAxis * stuckAngle, fixDuration, RotateMode.LocalAxisAdd)
      .SetEase(Ease.OutBack);
  }

  void OnEnable()
  {
    GameEventsManager.Instance.interactEvents.OnSwitch += HandleSwitchEvent;

    // Initialize the cancellation token and start the async loop (fire and forget)
    cts = new CancellationTokenSource();
    _ = IdleTwitchRoutineAsync(cts.Token);
  }

  void OnDisable()
  {
    if (GameEventsManager.Instance != null)
    {
      GameEventsManager.Instance.interactEvents.OnSwitch -= HandleSwitchEvent;
    }

    // Cancel the async task to prevent memory leaks and exceptions
    if (cts != null)
    {
      cts.Cancel();
      cts.Dispose();
      cts = null;
    }

    transform.DOKill();
  }

  private async Task IdleTwitchRoutineAsync(CancellationToken token)
  {
    try
    {
      // Loop infinitely until the door is fixed or the token is cancelled
      while (idleAnimationReplay > 0)
      {
        // Wait for a random amount of time (Task.Delay requires milliseconds)
        float waitTime = Random.Range(minTwitchInterval, maxTwitchInterval);
        await Task.Delay(Mathf.RoundToInt(waitTime * 1000f), token);
        idleAnimationReplay--;

        // Double check state before animating just in case it fired exactly on cancellation
        if (!isFixed && !token.IsCancellationRequested)
        {
          transform.DOPunchRotation(twitchStrength, twitchDuration, vibrato: 10, elasticity: 1f);
        }
      }
    }
    catch (TaskCanceledException)
    {
      // This block gracefully catches the exception thrown when cts.Cancel() is called
      // No action needed; the loop just exits safely.
    }
  }

  void HandleSwitchEvent(string incomingId, bool state)
  {
    if (incomingId == id && state == true && !isFixed)
    {
      isFixed = true;

      // Stop the idle twitching immediately
      if (cts != null)
      {
        cts.Cancel();
        cts.Dispose();
        cts = null;
      }

      // Kill any active twitch animation so it doesn't overlap with the fix animation
      transform.DOKill(true);

      PlayGlitchFixAnimation();
    }
  }

  void PlayGlitchFixAnimation()
  {
    float remainingAngle = 360f - stuckAngle;
    Vector3 finalRotationAdd = rotationAxis * remainingAngle;

    transform.DORotate(finalRotationAdd, fixDuration, RotateMode.LocalAxisAdd)
        .SetEase(fixEase)
        .OnComplete(SwapToNormal);
  }

  void SwapToNormal()
  {
    if (normalObject != null) normalObject.SetActive(true);
    else Debug.LogWarning("Normal object reference is missing on " + gameObject.name);

    gameObject.SetActive(false);
  }
}