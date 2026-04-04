using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDOverlayUIController : MonoBehaviour
{
  public static HUDOverlayUIController Instance { get; private set; }

  [Header("UI")]
  [SerializeField] private GameObject uiContainer;
  [SerializeField] private GameObject bottomContainer;

  [SerializeField] private GameObject questsContainer;
  [SerializeField] private GameObject questPrefab;

  [SerializeField] private TextMeshProUGUI stageName;

  [SerializeField] private TextMeshProUGUI cameraMode;

  [SerializeField] private TextMeshProUGUI stepLeft;

  void Awake()
  {
    if (Instance == null) Instance = this;
    Hide();
    UpdateQuestObjectives();
  }

  void OnEnable()
  {
    GameEventsManager.Instance.flowEvents.onGamePaused += Hide;
    GameEventsManager.Instance.flowEvents.onGameContinue += Show;
    GameEventsManager.Instance.flowEvents.onGameNew += Show;
    GameEventsManager.Instance.turnEvents.onStageRestart += Show;
    GameEventsManager.Instance.questEvents.OnQuestStateChanged += OnQuestUpdated;
  }

  void OnDisable()
  {
    GameEventsManager.Instance.flowEvents.onGamePaused -= Hide;
    GameEventsManager.Instance.flowEvents.onGameContinue -= Show;
    GameEventsManager.Instance.flowEvents.onGameNew -= Show;
    GameEventsManager.Instance.turnEvents.onStageRestart -= Show;
    GameEventsManager.Instance.questEvents.OnQuestStateChanged -= OnQuestUpdated;
  }

  public void Show() => uiContainer.SetActive(true);

  public void Hide() => uiContainer.SetActive(false);

  // Only show bottom UI during puzzle stage
  public void ToggleBottom(bool active) => bottomContainer.SetActive(active);

  public void SetStageName(string name) => stageName.text = name;

  public void SetCameraMode(string mode) => cameraMode.text = mode;

  public void SetStepLeft(int step) => stepLeft.text = $"{step:D3}";

  void OnQuestUpdated(Quest quest) => UpdateQuestObjectives();

  void UpdateQuestObjectives()
  {
    print("Update quest log");
    foreach (Transform child in questsContainer.transform) Destroy(child.gameObject);

    List<Quest> activeQuests = GameQuestManager.Instance.GetActiveQuests();
    foreach (Quest quest in activeQuests)
    {
      string description = quest.objectives[quest.GetCurrentObjectiveIndex()].description;

      GameObject obj = Instantiate(questPrefab, questsContainer.transform);
      TextMeshProUGUI text = obj.GetComponentInChildren<TextMeshProUGUI>();
      if (text != null) text.text = description;
    }
  }
}