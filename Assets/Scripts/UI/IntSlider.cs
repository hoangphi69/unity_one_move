using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class IntSlider : VisualElement, INotifyValueChanged<int>
{
  // Visuals
  private Label _valueLabel;
  private SliderInt _slider;

  // Attributes
  private int _minValue = 0;
  private int _maxValue = 100;
  private int _value = 0;
  private bool _fill = false;

  [UxmlAttribute]
  public int minValue
  {
    get => _slider?.lowValue ?? _minValue;
    set
    {
      _minValue = value;
      if (_slider != null) _slider.lowValue = value;
    }
  }

  [UxmlAttribute]
  public int maxValue
  {
    get => _slider?.highValue ?? _maxValue;
    set
    {
      _maxValue = value;
      if (_slider != null) _slider.highValue = value;
    }
  }

  [UxmlAttribute]
  public bool fill
  {
    get => _slider?.fill ?? _fill;
    set
    {
      _fill = value;
      if (_slider != null) _slider.fill = value;
    }
  }

  [UxmlAttribute]
  public int value
  {
    get => _slider?.value ?? _value;
    set
    {
      _value = value;
      if (_slider != null)
      {
        if (_slider.value == value) return;
        _slider.value = value;
        _valueLabel.text = _slider.value.ToString();
      }
    }
  }

  public void SetValueWithoutNotify(int newValue)
  {
    _slider.SetValueWithoutNotify(newValue);
    _valueLabel.text = newValue.ToString();
  }

  public IntSlider()
  {
    // Create Parts
    _valueLabel = new Label("0");
    _slider = new SliderInt(minValue, maxValue);

    // Styling
    AddToClassList("int-slider");
    _valueLabel.AddToClassList("int-slider__value");
    _slider.AddToClassList("int-slider__input");

    // Hierarchy
    Add(_valueLabel);
    Add(_slider);

    // Handles interaction
    _slider.pickingMode = PickingMode.Position;
    _slider.RegisterValueChangedCallback(evt =>
    {
      _valueLabel.text = evt.newValue.ToString();

      using var outEvt = ChangeEvent<int>.GetPooled(evt.previousValue, evt.newValue);
      outEvt.target = this;
      SendEvent(outEvt);
    });
  }
}