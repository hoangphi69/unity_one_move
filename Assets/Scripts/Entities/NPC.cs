using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class NPC : MonoBehaviour
{
  [SerializeField] private string knotName;

  private Interactable interactable;

  private void Awake()
  {
    interactable = GetComponent<Interactable>();
  }

  private void OnEnable()
  {
    interactable.OnInteracted += EnterDialogue;
  }

  private void OnDisable()
  {
    interactable.OnInteracted -= EnterDialogue;
  }

  public void EnterDialogue()
  {
    GameEventsManager.Instance.dialogueEvents.EnterDialogue(knotName, DialogueMode.InGame);
  }
}