using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum QuestState
{
  UNKNOWN,    // 0: Not yet discovered
  AVAILABLE,  // 1: Prerequisites met, ready to pick up
  ACTIVE,     // 2: In the player's quest log
  ACHIEVED,   // 3: Objectives done, needs to be turned in
  COMPLETED,  // 4: Finished and rewarded (Terminal)
  FAILED      // 5: Failed or locked out (Terminal)
}

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quest System/Quest")]
public class Quest : ScriptableObject
{
  [field: SerializeField] public string id { get; private set; }

  [Header("General")]
  [SerializeField] public string title;
  [SerializeField] public string description;

  [SerializeField] private QuestState state = QuestState.UNKNOWN;

  [Header("Requirements (Optional)")]
  public Quest[] requiredQuests;

  [Header("Objectives (Sequential)")]
  [SerializeField] private int currentObjective = 0;
  public List<QuestObjective> objectives = new();

  [Header("Rewards (Optional)")]
  public bool requiresTurnIn = false;

  public QuestState GetState() => state;

  // Only accept forward state progression
  public void SetState(QuestState newState)
  {
    if (state == QuestState.COMPLETED || state == QuestState.FAILED) return;
    if (newState < state) return;
    state = newState;
  }

  public void AdvanceObjective() => currentObjective++;

  public bool StartObjective()
  {
    if (currentObjective >= objectives.Count) return false;
    objectives[currentObjective].details.Initialize(id);
    return true;
  }

  public int GetCurrentObjectiveIndex() => currentObjective;

  public QuestData Save()
  {
    QuestData data = new()
    {
      id = id,
      state = state,
      currentObjective = currentObjective
    };

    foreach (var obj in objectives) data.objectiveDataJsons.Add(obj.details.Save());

    return data;
  }

  public void Load(QuestData data)
  {
    state = data.state;
    currentObjective = data.currentObjective;

    for (int i = 0; i < objectives.Count; i++)
    {
      if (i < data.objectiveDataJsons.Count) objectives[i].details.Load(data.objectiveDataJsons[i]);
    }

    if (state == QuestState.ACTIVE) StartObjective();
  }

#if UNITY_EDITOR
  void OnValidate()
  {
    id = name;
    EditorUtility.SetDirty(this);
  }
#endif
}