using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CustomSlider : MonoBehaviour
{
  [SerializeField] private TextMeshProUGUI valueText;
  [SerializeField] private Slider slider;

  public UnityEvent<float> OnValueChanged;

  void OnEnable()
  {
    slider.onValueChanged.AddListener(HandleSliderChange);
  }

  void OnDisable()
  {
    slider.onValueChanged.RemoveListener(HandleSliderChange);
  }

  public void Setup(float minValue, float maxValue, float currentValue)
  {
    slider.minValue = minValue;
    slider.maxValue = maxValue;
    SetValueWithoutNotify(currentValue);
  }

  public void SetValueWithoutNotify(float val)
  {
    slider.SetValueWithoutNotify(val);
    UpdateValueText(val);
  }

  private void HandleSliderChange(float val)
  {
    UpdateValueText(val);
    OnValueChanged?.Invoke(val);
  }

  private void UpdateValueText(float val)
  {
    // If your slider max is 1.0 (for FMOD), we multiply by 100 for the UI text.
    // If your slider max is already 100, we just show it directly.
    float displayValue = slider.maxValue <= 1.0f ? val * 100f : val;
    valueText.text = Mathf.RoundToInt(displayValue).ToString();
  }
}