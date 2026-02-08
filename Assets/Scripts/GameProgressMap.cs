using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "GameProgressMap", menuName = "ScriptableObjects/GameProgressMap")]
public class GameProgressMap : ScriptableObject
{
  [Serializable]
  public struct ProgressMapping
  {
    public float value;
    public SceneField scene;
  }

  [SerializeField] private List<ProgressMapping> mappings = new();

  public SceneField GetGameplayScene(float progress)
  {
    ProgressMapping match = mappings.FirstOrDefault(m => Mathf.Approximately(m.value, progress));
    return match.scene;
  }

  public SceneField GetLobbyScene(float progress)
  {
    ProgressMapping match = mappings.FirstOrDefault(m => Mathf.Approximately(m.value, (int)progress));
    return match.scene;
  }
}