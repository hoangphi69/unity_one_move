using System;
using UnityEngine;

[AddTypeMenu("Interact")]
[Serializable]
public class InteractObjective : QuestObjectiveInfo
{
  [SerializeField] private string targetID;

  public override void Initialize(string questID)
  {
    base.Initialize(questID);
    GameEventsManager.Instance.interactEvents.OnInteract += OnInteract;
  }

  void OnInteract(string id)
  {
    if (string.IsNullOrEmpty(id)) return;
    if (id == targetID)
    {
      Complete();
      GameEventsManager.Instance.interactEvents.OnInteract -= OnInteract;
    }
  }
}