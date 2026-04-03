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
    interactable.OnMainAction += EnterDialogue;
  }

  private void OnDisable()
  {
    interactable.OnMainAction -= EnterDialogue;
  }

  public void EnterDialogue()
  {
    GameEventsManager.Instance.dialogueEvents.EnterDialogue(knotName, DialogueMode.InGame);
  }
}