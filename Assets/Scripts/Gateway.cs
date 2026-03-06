using UnityEngine;

public class Gateway : MonoBehaviour, IGateway
{
  [SerializeField] private SceneField _nextStage;

  void OnTriggerEnter()
  {
    Transition();
  }

  public void Transition()
  {
    GameplayManager.Instance.LoadStageAsync(_nextStage);
  }
}