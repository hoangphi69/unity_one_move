using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
  public static GameEventsManager Instance { get; private set; }

  public DialogueEvents dialogueEvents;
  public TurnEvents turnEvents;
  public FlowEvents flowEvents;
  public QuestEvents questEvents;
  public InteractEvents interactEvents;

  void Awake()
  {
    if (Instance == null) Instance = this;

    dialogueEvents = new();
    turnEvents = new();
    flowEvents = new();
    questEvents = new();
    interactEvents = new();
  }
}