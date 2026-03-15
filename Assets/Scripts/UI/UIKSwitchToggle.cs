using UnityEngine.UIElements;

[UxmlElement]
public partial class UIKSwitchToggle : VisualElement, INotifyValueChanged<bool>
{
  // Visual
  readonly Label _stateLabel;
  readonly VisualElement _track;
  readonly VisualElement _knob;

  string _textOn = "ON";
  string _textOff = "OFF";

  [UxmlAttribute]
  public string textOn
  {
    get => _textOn;
    set { _textOn = value; UpdateVisualState(); }
  }

  [UxmlAttribute]
  public string textOff
  {
    get => _textOff;
    set { _textOff = value; UpdateVisualState(); }
  }

  // State
  private bool _value;

  // --- INotifyValueChanged Implementation ---
  public bool value
  {
    get => _value;
    set
    {
      if (_value == value) return;
      using var evt = ChangeEvent<bool>.GetPooled(_value, value);
      evt.target = this;
      SetValueWithoutNotify(value);
      SendEvent(evt);
    }
  }

  public void SetValueWithoutNotify(bool newValue)
  {
    _value = newValue;
    UpdateVisualState();
  }

  // Constructor
  public UIKSwitchToggle()
  {
    // Initialize parts
    _stateLabel = new Label();
    _track = new VisualElement();
    _knob = new VisualElement();

    // Styling
    AddToClassList("switch-toggle");
    _stateLabel.AddToClassList("switch-toggle__value");
    _track.AddToClassList("switch-toggle__track");
    _knob.AddToClassList("switch-toggle__knob");

    // Hierachy
    _track.Add(_knob);
    Add(_stateLabel);
    Add(_track);

    // Handle clicks
    pickingMode = PickingMode.Position;
    _stateLabel.pickingMode = PickingMode.Ignore;
    _track.pickingMode = PickingMode.Ignore;
    _knob.pickingMode = PickingMode.Ignore;

    RegisterCallback<ClickEvent>(OnClick);
    UpdateVisualState();
  }

  void OnClick(ClickEvent evt) => value = !value;

  void UpdateVisualState()
  {
    _stateLabel.text = _value ? _textOn : _textOff;
    if (_value == true) AddToClassList("checked");
    else RemoveFromClassList("checked");
  }
}