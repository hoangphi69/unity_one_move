using System;
using UnityEngine;

[AddTypeMenu("Interact")]
[Serializable]
public class InteractObjective : ObjectiveInfo
{
  [SerializeField] private string targetID;

  public override void Initialize(string questID)
  {
    base.Initialize(questID);
    GameEventsManager.Instance.interactEvents.OnInteract += OnInteract;
    GameEventsManager.Instance.questEvents.OnQueryObjectiveState += RespondToStateQuery;
    GameEventsManager.Instance.questEvents.ObjectiveActive(targetID);
  }

  void RespondToStateQuery(string id)
  {
    if (targetID == id) GameEventsManager.Instance.questEvents.ObjectiveActive(targetID);
  }

  void OnInteract(string id)
  {
    if (string.IsNullOrEmpty(id)) return;
    if (id == targetID)
    {
      Complete();
      GameEventsManager.Instance.interactEvents.OnInteract -= OnInteract;
      GameEventsManager.Instance.questEvents.OnQueryObjectiveState -= RespondToStateQuery;
      GameEventsManager.Instance.questEvents.ObjectiveComplete(targetID);
    }
  }
}