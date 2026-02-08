using UnityEngine;

public class GameProgress : MonoBehaviour
{
  private float progress;

  [Header("Config")]
  [SerializeField] private SceneField lobby1;
  [SerializeField] private SceneField gameplay1;
  [SerializeField] private SceneField gameplay2;
  [SerializeField] private SceneField gameplay3;
  [SerializeField] private SceneField gameplay4;
  [SerializeField] private SceneField gameplay5;

  public GameProgress(float progress)
  {
    this.progress = progress;
  }

  public SceneField GetGameplay()
  {
    return (int)progress switch
    {
      1 => gameplay1,
      2 => gameplay2,
      3 => gameplay3,
      4 => gameplay4,
      5 => gameplay5,
      _ => gameplay1,
    };
  }

  public SceneField GetLobby()
  {
    return (int)progress switch
    {
      1 => lobby1,
      _ => lobby1,
    };
  }
}