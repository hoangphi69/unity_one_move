using UnityEngine;

public class Gateway : MonoBehaviour, IGateway
{
  [SerializeField] private SceneField _nextStage;
  [SerializeField] private bool _saveProgress = false;

  private bool isTransitioning = false;

  void OnTriggerEnter()
  {
    Transition();
  }

  public async void Transition()
  {
    if (isTransitioning) return;
    isTransitioning = true;

    await GameplayManager.Instance.LoadStageAsync(_nextStage);
    if (_saveProgress) await GameDataManager.Instance.SaveProgress(_nextStage);

    isTransitioning = false;
  }
}