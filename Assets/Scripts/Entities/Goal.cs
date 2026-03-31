using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class Goal : MonoBehaviour
{

  [SerializeField] private string cutscene;
  [SerializeField] private SceneField nextStage;

  private Interactable interactable;
  private Animator animator;

  void Awake()
  {
    interactable = GetComponent<Interactable>();
    animator = GetComponent<Animator>();
  }

  private void OnEnable()
  {
    interactable.OnInteracted += CompleteGoal;
  }

  private void OnDisable()
  {
    interactable.OnInteracted -= CompleteGoal;
  }

  private async void CompleteGoal()
  {
    animator.CrossFade("Open", .3f);
    await Task.Delay(500);

    await GameDataManager.Instance.SaveProgress(this.nextStage);
    string nextStage = GameDataManager.Instance.GetProgress();
    await GameplayManager.Instance.LoadStageAsync(nextStage, cutscene);
  }
}