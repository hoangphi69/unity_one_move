using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
  private Canvas bubble;
  private OutlineHighlight outlineHighlight;
  private Outline outline;

  [SerializeField] private string knotName;

  void Awake()
  {
    bubble = GetComponentInChildren<Canvas>();
    bubble.enabled = false;

    outline = GetComponent<Outline>();
    outline.enabled = false;
  }

  void Start()
  {
    // bubble.transform.LookAt(bubble.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
  }

  public void OnDetected()
  {
    ShowBubble();
    ShowOutline();
  } 
  public void OnLost()
  {
    HideBubble();
    HideOutline();
  } 
  public void OnInteract() => EnterDialogue();
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