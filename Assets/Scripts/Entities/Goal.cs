using System.Threading.Tasks;
using UnityEngine;

public class Goal : MonoBehaviour, IInteractable, IObstacle
{
  private Outline outline;
  private Animator animator;

  [SerializeField] private string cutscene;
  [SerializeField] private SceneField nextStage;

  [SerializeField] private bool BlockPlayer;

  void Awake()
  {
    outline = GetComponentInChildren<Outline>();
    outline.enabled = false;
    animator = GetComponent<Animator>();
  }

  public void OnDetected() => ShowOutline();
  public void OnLost() => HideOutline();
  public void OnInteract() => _ = CompleteGoal();
  public Vector3 GetPosition() => transform.position;

  public void ShowOutline()
  {
    outline.enabled = true;
  }

  public void HideOutline()
  {
    outline.enabled = false;
  }

  private async Task CompleteGoal()
  {

    animator.CrossFade("Open", .3f);
    // await GameDataManager.Instance.SaveProgress(this.nextStage);
    // string nextStage = GameDataManager.Instance.GetProgress();
    // await GameplayManager.Instance.LoadStageAsync(nextStage, cutscene);
    print("door opened");
  }

  public bool IsPlayerBlocking()
  {
    throw new System.NotImplementedException();
  }

  public bool IsEnemyBlocking()
  {
    throw new System.NotImplementedException();
  }

  public bool IsEnemySightBlocking()
  {
    throw new System.NotImplementedException();
  }
}