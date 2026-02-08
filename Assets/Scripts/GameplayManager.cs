using UnityEngine;

public class GameplayManager : MonoBehaviour
{
  [SerializeField] private float progress = 1.0f;

  private void Awake()
  {
    // Save progress
    _ = GameDataManager.Instance.SaveProgress(progress);
  }
}