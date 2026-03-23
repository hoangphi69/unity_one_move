using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class PauseSavesPanel : NavigationPanel, ISavesPanel
{
  private SaveSlot[] saveSlots;
  private SaveSlot selectedSlot;
  private string activeProfileID;

  [Header("Buttons")]
  [SerializeField] private Button saveGameButton;
  [SerializeField] private Button overrideGameButton;
  [SerializeField] private Button backButton;

  void Awake()
  {
    saveSlots = GetComponentsInChildren<SaveSlot>();
  }

  void OnEnable()
  {
    InitializeSaveSlots();
    ClearSelection();

    backButton.onClick.AddListener(OnBackClicked);
    saveGameButton.onClick.AddListener(saveGameClicked);
    overrideGameButton.onClick.AddListener(overrideGameClicked);
  }

  void OnDisable()
  {
    backButton.onClick.RemoveListener(OnBackClicked);
    saveGameButton.onClick.RemoveListener(saveGameClicked);
    overrideGameButton.onClick.RemoveListener(overrideGameClicked);
  }

  async void InitializeSaveSlots()
  {
    Dictionary<string, GameData> profiles = await GameDataManager.Instance.GetAllProfiles();
    activeProfileID = GameDataManager.Instance.selectedProfileID;
    foreach (SaveSlot saveSlot in saveSlots)
    {
      string profileID = saveSlot.GetProfileID();
      profiles.TryGetValue(profileID, out GameData data);
      saveSlot.Display(data);
      if (profileID == activeProfileID) saveSlot.DisplayActive();
    }
  }

  public void SelectSlot(SaveSlot slot)
  {
    ClearSelection();
    selectedSlot = slot;
    if (selectedSlot.GetProfileID() == activeProfileID) return;
    if (selectedSlot.HasData()) overrideGameButton.gameObject.SetActive(true);
    else saveGameButton.gameObject.SetActive(true);
  }

  void ClearSelection()
  {
    selectedSlot = null;
    saveGameButton.gameObject.SetActive(false);
    overrideGameButton.gameObject.SetActive(false);
  }

  async void saveGameClicked()
  {
    if (selectedSlot == null) return;

    await GameDataManager.Instance.SaveGame(selectedSlot.GetProfileID());
    selectedSlot.Display(GameDataManager.Instance.data);
    ClearSelection();
  }

  void overrideGameClicked()
  {
    if (selectedSlot == null) return;

    ConfirmOverlayUIController.Instance.Show(
      "Override save",
      $"You are about to override <color=#D57B19>save no. {selectedSlot.GetProfileID()}</color>. This action cannot be undone. Are you sure you want to continue?",
      onConfirm: saveGameClicked
    );
  }

  void OnBackClicked() => ClosePanel();
}