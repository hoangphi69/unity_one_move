using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
  private Canvas bubble;
  private OutlineHighlight outlineHighlight;

  [SerializeField] private string knotName;

  void Awake()
  {
    bubble = GetComponentInChildren<Canvas>();
    bubble.enabled = false;
  }

  void Start()
  {
    // bubble.transform.LookAt(bubble.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
  }

  public void OnDetected() => ShowBubble();
  public void OnLost() => HideBubble();
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

  public void EnterDialogue()
  {
    GameEventsManager.Instance.dialogueEvents.EnterDialogue(knotName, DialogueMode.InGame);
  }
}