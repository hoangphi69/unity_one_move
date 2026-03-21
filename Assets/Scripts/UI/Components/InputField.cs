using UnityEngine;
using UnityEngine.UI;

public class InputField : MonoBehaviour
{
  [SerializeField] private Image highlight;

  public void SetHighlight(bool active)
  {
    highlight.gameObject.SetActive(active);
  }
}