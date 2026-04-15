using UnityEngine;

[RequireComponent(typeof(Collide))]
public class Collectible : MonoBehaviour
{
  [SerializeField] private string dialogue;
  private Collide collide;

  void Awake()
  {
    collide = GetComponent<Collide>();
  }

  void OnEnable()
  {
    collide.OnCollided += Collect;
  }

  void OnDisable()
  {
    collide.OnCollided -= Collect;
  }

  void Collect(Vector3 direction)
  {
    GameEventsManager.Instance.dialogueEvents.EnterDialogue(dialogue, DialogueMode.InGame);
    Destroy(gameObject);
  }
}