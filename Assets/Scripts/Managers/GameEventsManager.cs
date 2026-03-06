using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
  public static GameEventsManager Instance { get; private set; }

  public DialogueEvents dialogueEvents;
  public TurnEvents turnEvents;

  void Awake()
  {
    Instance = this;

    // initialize the events
    dialogueEvents = new();
    turnEvents = new();
  }
}