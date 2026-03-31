using UnityEngine;

public enum QuestState
{
  UNAVAILABLE,
  AVAILABLE,
  IN_PROGRESS,
  DONE,
  COMPLETED,
}

public class Quest
{
  public QuestInfoSO info;
  public QuestState state;

  private int currentStep;

  public Quest(QuestInfoSO questInfo)
  {
    info = questInfo;
    state = QuestState.UNAVAILABLE;
    currentStep = 0;
  }

  public void MoveToNextStep()
  {
    currentStep++;
  }

  public bool CurrentStepExists()
  {
    return currentStep < info.questSteps.Length;
  }

  public void InitializeCurrentStep(Transform parent)
  {
    GameObject prefab = GetCurrentQuestStepPrefab();
    if (prefab != null)
      Object
      .Instantiate(prefab, parent)
      .GetComponent<QuestStep>()
      .Initialize(info.id);
  }

  private GameObject GetCurrentQuestStepPrefab()
  {
    if (!CurrentStepExists()) return null;
    else return info.questSteps[currentStep];
  }
}