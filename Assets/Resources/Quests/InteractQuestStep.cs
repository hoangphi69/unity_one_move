using UnityEngine;

public class InteractQuestStep : QuestStep
{
  [SerializeField] private string eventID;

  void OnEnable()
  {
    GameEventsManager.Instance.interactEvents.OnInteract += OnInteract;
  }

  void OnDisable()
  {
    GameEventsManager.Instance.interactEvents.OnInteract -= OnInteract;
  }

  void OnInteract(string id)
  {
    if (string.IsNullOrEmpty(id)) return;
    if (eventID == id) Done();
  }
}