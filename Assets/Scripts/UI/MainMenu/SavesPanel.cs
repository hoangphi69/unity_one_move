using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavesPanel : MonoBehaviour
{
  private SaveSlot[] saveSlots;
  private SaveSlot activeSlot;
  private SaveSlot selectedSlot;

  [Header("Buttons")]
  [SerializeField] private Button newGameButton;
  [SerializeField] private Button loadButton;
  [SerializeField] private Button deleteButton;
  [SerializeField] private Button backButton;

  void Awake()
  {
    saveSlots = GetComponentsInChildren<SaveSlot>();
  }

  void OnEnable()
  {
    InitializeSaveSlots();
    ClearSelection();

    backButton.onClick.AddListener(backClicked);
    newGameButton.onClick.AddListener(newGameClicked);
    loadButton.onClick.AddListener(loadClicked);
  }

  void OnDisable()
  {
    backButton.onClick.RemoveListener(backClicked);
    newGameButton.onClick.RemoveListener(newGameClicked);
    loadButton.onClick.RemoveListener(loadClicked);
  }

  async void InitializeSaveSlots()
  {
    Dictionary<string, GameData> profiles = await GameDataManager.Instance.GetAllProfiles();
    foreach (SaveSlot saveSlot in saveSlots)
    {
      string profileID = saveSlot.GetProfileID();
      profiles.TryGetValue(profileID, out GameData data);
      saveSlot.Display(data);
      if (profileID == GameDataManager.Instance.selectedProfileID) SetActiveSlot(saveSlot);
    }
  }

  public void SetActiveSlot(SaveSlot slot)
  {
    if (activeSlot != null) activeSlot.ClearActive();
    activeSlot = slot;
    activeSlot.DisplayActive();
  }

  public void SelectSlot(SaveSlot slot)
  {
    selectedSlot = slot;
    if (selectedSlot.HasData())
    {
      // Case: Data exists
      newGameButton.gameObject.SetActive(false);
      loadButton.gameObject.SetActive(true);
      deleteButton.gameObject.SetActive(true);
    }
    else
    {
      // Case: Empty slot
      newGameButton.gameObject.SetActive(true);
      loadButton.gameObject.SetActive(false);
      deleteButton.gameObject.SetActive(false);
    }
  }

  void ClearSelection()
  {
    selectedSlot = null;
    newGameButton.gameObject.SetActive(false);
    loadButton.gameObject.SetActive(false);
    deleteButton.gameObject.SetActive(false);
  }

  async void loadClicked()
  {
    if (selectedSlot == null) return;
    SetActiveSlot(selectedSlot);
    await GameDataManager.Instance.SwitchProfile(activeSlot.GetProfileID());
    await GameSceneManager.Instance.LoadTitleGameplay();
  }

  async void newGameClicked()
  {
    if (selectedSlot == null) return;
    SetActiveSlot(selectedSlot);
    await GameDataManager.Instance.SwitchProfile(activeSlot.GetProfileID());
    GameSceneManager.Instance.OnTitleNewGame();
  }

  void backClicked()
  {
    TitleScreenManager.Instance.CloseCurrentPanel();
  }
}