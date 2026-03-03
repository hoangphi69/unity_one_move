using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class HorizontalSelector : VisualElement, INotifyValueChanged<string>
{
  // Visual
  private readonly Button _leftButton;
  private readonly Label _valueLabel;
  private readonly Button _rightButton;

  // State
  public List<string> choices { get; set; } = new List<string>();
  private int _selectedIndex = -1;
  private string _value;

  public int selectedIndex
  {
    get => _selectedIndex;
    set
    {
      int newIndex = value;

      if (newIndex < 0) newIndex = choices.Count - 1;
      if (newIndex >= choices.Count) newIndex = 0;

      if (_selectedIndex != newIndex)
      {
        _selectedIndex = newIndex;
        this.value = choices[_selectedIndex];
      }
    }
  }

  public string value
  {
    get => _value;
    set
    {
      if (value == _value) return;

      using var evt = ChangeEvent<string>.GetPooled(_value, value);
      evt.target = this;
      SetValueWithoutNotify(value);
      SendEvent(evt);
    }
  }

  public void SetValueWithoutNotify(string newValue)
  {
    _value = newValue;
    _valueLabel.text = newValue;

    if (choices != null)
    {
      _selectedIndex = choices.IndexOf(newValue);
    }
  }

  // Constructor
  public HorizontalSelector()
  {
    _leftButton = new Button(OnLeftClicked) { text = "◄" };
    _valueLabel = new Label() { text = "-" };
    _rightButton = new Button(OnRightClicked) { text = "►" };

    _leftButton.AddToClassList("horizontal-selector__button");
    _valueLabel.AddToClassList("horizontal-selector__value");
    _rightButton.AddToClassList("horizontal-selector__button");
    _leftButton.style.backgroundImage = Resources.Load<Texture2D>("UI/arrow_left");
    _rightButton.style.backgroundImage = Resources.Load<Texture2D>("UI/arrow_right");

    AddToClassList("horizontal-selector");
    Add(_leftButton);
    Add(_valueLabel);
    Add(_rightButton);
  }

  void OnLeftClicked() => selectedIndex--;
  void OnRightClicked() => selectedIndex++;
}