using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collide))]
public class Gateway : MonoBehaviour
{
  [SerializeField] private SceneField _nextStage;
  [SerializeField] private string cutscene;
  [SerializeField] private bool _saveProgress = false;

  private Animator animator;
  private Collide collide;
  private bool isOpen = false;

  void Awake()
  {
    animator = GetComponent<Animator>();
    collide = GetComponent<Collide>();
  }

  void OnEnable()
  {
    collide.OnCollided += OpenDoor;
  }

  void OnDisable()
  {
    collide.OnCollided -= OpenDoor;
  }

  async void OpenDoor(Vector3 direction)
  {
    if (isOpen) return;
    isOpen = true;
    GameEventsManager.Instance.turnEvents.StageFinished();

    float dotProduct = Vector3.Dot(transform.forward, direction);
    string anim = dotProduct > 0 ? "Pull" : "Push";
    GameAudioManager.Instance.PlaySFX(AudioTag.Door);
    await PlayAnimation(anim);
    await Transition();
  }

  async Task PlayAnimation(string anim)
  {
    animator.CrossFade(anim, .1f);
    await Task.Delay(150);
  }

  async Task Transition()
  {
    if (string.IsNullOrEmpty(cutscene)) await GameplayManager.Instance.LoadStageAsync(_nextStage);
    else await GameplayManager.Instance.LoadStageAsync(_nextStage, cutscene);

    if (_saveProgress) await GameDataManager.Instance.SaveProgress(_nextStage);
  }
}