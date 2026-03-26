using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
  private Canvas bubble;
  private Outline outline;
  [SerializeField] private string knotName;
  private bool hasInteracted = false;

  void Awake()
  {
    bubble = GetComponentInChildren<Canvas>();
    bubble.enabled = true;

    outline = GetComponent<Outline>();
    outline.enabled = false;
  }

  void Start()
  {
    // bubble.transform.LookAt(bubble.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
  }

  public void OnDetected()
  {
    ShowOutline();
  }
  public void OnLost()
  {
    HideOutline();
    if (hasInteracted) HideBubble();
  }
  public void OnInteract()
  {
    hasInteracted = true;
    HideBubble();
    EnterDialogue();
  }
  public Vector3 GetPosition() => transform.position;

  public void ShowBubble()
  {
    bubble.enabled = true;
  }

  public void HideBubble()
  {
    bubble.enabled = false;
  }

  public void ShowOutline()
  {
    outline.enabled = true;
  }

  public void HideOutline()
  {
    outline.enabled = false;
  }

  public void EnterDialogue()
  {
    GameEventsManager.Instance.dialogueEvents.EnterDialogue(knotName, DialogueMode.InGame);
  }
}

//box
//glass
//check raycast half size cube