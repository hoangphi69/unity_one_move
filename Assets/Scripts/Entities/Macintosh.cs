using UnityEngine;


[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Interactable))]
public class Macintosh : MonoBehaviour
{
  private Interactable interactable;
  private MeshRenderer rend;

  private bool triggered = false;

  void Awake()
  {
    interactable = GetComponent<Interactable>();
    rend = GetComponent<MeshRenderer>();
  }

  void OnEnable()
  {
    interactable.OnMainAction += Toggle;
  }

  void OnDisable()
  {
    interactable.OnMainAction -= Toggle;
  }

  void Toggle()
  {
    if (triggered) return;
    triggered = true;

    rend.materials[1].mainTextureOffset = new Vector2(0.5f, 0);
  }
}