using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HorizontalSelector : MonoBehaviour
{
  [SerializeField] private Button leftButton;
  [SerializeField] private Button rightButton;
  [SerializeField] private TextMeshProUGUI valueText;

  public UnityEvent<int> OnValueChanged;

  private List<string> options = new List<string>();
  private int currentIndex = 0;

  public void Setup(List<string> newOptions, int startIndex)
  {
    options = newOptions;

    leftButton.onClick.RemoveAllListeners();
    rightButton.onClick.RemoveAllListeners();

    leftButton.onClick.AddListener(SelectPrevious);
    rightButton.onClick.AddListener(SelectNext);

    SetValueWithoutNotify(startIndex);
  }

  public void SetValueWithoutNotify(int index)
  {
    if (options.Count == 0) return;

    currentIndex = Mathf.Clamp(index, 0, options.Count - 1);
    valueText.text = options[currentIndex];
  }

  private void SelectPrevious()
  {
    if (options.Count == 0) return;

    currentIndex--;
    if (currentIndex < 0) currentIndex = options.Count - 1; // Loop back to end

    UpdateValue();
  }

  private void SelectNext()
  {
    if (options.Count == 0) return;

    currentIndex++;
    if (currentIndex >= options.Count) currentIndex = 0; // Loop back to start

    UpdateValue();
  }

  private void UpdateValue()
  {
    valueText.text = options[currentIndex];
    OnValueChanged?.Invoke(currentIndex);
  }
}