using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SwitchToggle : MonoBehaviour
{
  [Header("UI References")]
  [SerializeField] private Button toggleButton; // Attach a button component to your prefab and link it here
  [SerializeField] private TextMeshProUGUI valueText;
  [SerializeField] private RectTransform knobTransform; // The RectTransform of the knob Image
  [SerializeField] private Image track;
  [SerializeField] private Image knob;

  [Header("Visual Settings")]
  [SerializeField] private float offPositionX = 56f; // Adjust these in the inspector based on your UI
  [SerializeField] private float onPositionX = 12f;
  [SerializeField] private Color offColor = new(56, 56, 56);
  [SerializeField] private Color onColor = new(209, 130, 44);

  public UnityEvent<bool> OnValueChanged;

  private bool isOn;

  void OnEnable()
  {
    if (toggleButton != null)
      toggleButton.onClick.AddListener(OnToggleClicked);
  }

  void OnDisable()
  {
    if (toggleButton != null)
      toggleButton.onClick.RemoveListener(OnToggleClicked);
  }

  public void SetValueWithoutNotify(bool value)
  {
    isOn = value;
    UpdateVisuals();
  }

  private void OnToggleClicked()
  {
    isOn = !isOn;
    UpdateVisuals();
    OnValueChanged?.Invoke(isOn);
  }

  private void UpdateVisuals()
  {
    valueText.text = isOn ? "ON" : "OFF";

    if (knobTransform != null)
    {
      Vector2 pos = knobTransform.anchoredPosition;
      pos.x = isOn ? onPositionX : offPositionX;
      knobTransform.anchoredPosition = pos;
    }

    track.color = isOn ? onColor : offColor;
  }
}