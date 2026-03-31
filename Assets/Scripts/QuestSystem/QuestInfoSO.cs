using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestInfoSO", menuName = "ScriptableObjects/QuestInfoSO", order = 1)]
public class QuestInfoSO : ScriptableObject
{
  [field: SerializeField] public string id { get; private set; }

  [Header("General")]
  public string displayName;

  [Header("Requirements")]
  public QuestInfoSO[] questPrerequisites;

  [Header("Steps")]
  public GameObject[] questSteps;

  // Implement rewards later
  // [Header("Rewards")]

  void OnValidate()
  {
#if UNITY_EDITOR
    id = name;
    EditorUtility.SetDirty(this);
#endif
  }
}