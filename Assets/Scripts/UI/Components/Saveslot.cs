using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
  private GameData _data;

  [Header("Profile")]
  [SerializeField] private string profileID = "";

  [Header("Content")]
  [SerializeField] private Button slot;
  [SerializeField] private GameObject vacantSlot;
  [SerializeField] private GameObject occupiedSlot;
  [SerializeField] private TextMeshProUGUI title;
  [SerializeField] private TextMeshProUGUI subtitle;
  [SerializeField] private TextMeshProUGUI progress;
  [SerializeField] private GameObject active;

  void OnEnable()
  {
    slot.onClick.AddListener(OnSlotClicked);
  }

  void OnDisable()
  {
    slot.onClick.RemoveListener(OnSlotClicked);
  }

  void OnSlotClicked()
  {
    GetComponentInParent<ISavesPanel>()?.SelectSlot(this);
  }

  public bool HasData() => _data != null;

  public string GetProfileID() => profileID;

  public void Display(GameData data)
  {
    _data = data;
    if (data == null) DisplayVacantSlot();
    else DisplayOccupiedSlot(data);
  }

  public void DisplayVacantSlot()
  {
    vacantSlot.SetActive(true);
    occupiedSlot.SetActive(false);
  }

  public void DisplayOccupiedSlot(GameData data)
  {
    vacantSlot.SetActive(false);
    occupiedSlot.SetActive(true);

    subtitle.text = data.GetSubtitle();
    title.text = data.GetTitle();
    progress.text = data.GetProgressPercentage().ToString();
  }

  public void DisplayActive()
  {
    active.gameObject.SetActive(true);
  }

  public void ClearActive()
  {
    active.gameObject.SetActive(false);
  }
}