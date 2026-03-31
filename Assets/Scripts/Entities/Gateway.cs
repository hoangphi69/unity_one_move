using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Collide))]
public class Gateway : MonoBehaviour
{
  [SerializeField] private SceneField _nextStage;
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

  async void OpenDoor(Vector3 _)
  {
    if (isOpen) return;
    isOpen = true;
    await PlayAnimation();
    await Transition();
  }

  async Task PlayAnimation()
  {
    animator.CrossFade("Open", .1f);
    await Task.Delay(150);
  }

  async Task Transition()
  {
    await GameplayManager.Instance.LoadStageAsync(_nextStage);
    if (_saveProgress) await GameDataManager.Instance.SaveProgress(_nextStage);
  }
}