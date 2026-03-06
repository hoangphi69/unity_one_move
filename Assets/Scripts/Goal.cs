using System.Threading.Tasks;
using UnityEngine;

public class Goal : MonoBehaviour, IInteractable
{
  private Canvas bubble;

  [SerializeField] private string cutsceneName;
  [SerializeField] private SceneField nextStage;

  void Awake()
  {
    bubble = GetComponentInChildren<Canvas>();
    bubble.enabled = false;
  }

  void Start()
  {
    bubble.transform.LookAt(bubble.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
  }

  public void OnDetected() => ShowBubble();
  public void OnLost() => HideBubble();
  public void OnInteract() => _ = CompleteGoal();
  public Vector3 GetPosition() => transform.position;

  public void ShowBubble()
  {
    bubble.enabled = true;
  }

  public void HideBubble()
  {
    bubble.enabled = false;
  }

  private async Task CompleteGoal()
  {
    await GameDataManager.Instance.SaveProgress(nextStage);
    string nextScene = GameDataManager.Instance.GetProgressScene();
    GameplayManager.Instance.LoadStageAsync(nextScene, cutsceneName);
  }
}