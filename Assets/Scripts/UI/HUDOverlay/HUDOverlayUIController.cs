using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class HUDOverlayUIController : MonoBehaviour
{
  public static HUDOverlayUIController Instance { get; private set; }

  [Header("UI")]
  [SerializeField] private CanvasGroup uiContainer;
  [SerializeField] private CanvasGroup bottomContainer;
  [SerializeField] private float fadeDuration = .2f;

  [SerializeField] private GameObject questsContainer;
  [SerializeField] private GameObject questPrefab;

  [SerializeField] private TextMeshProUGUI stageName;

  [SerializeField] private TextMeshProUGUI cameraMode;

  [Header("Counter")]
  [SerializeField] private Canvas counter;
  [SerializeField] private RectTransform[] digitContainers = new RectTransform[3];
  [SerializeField] private TextMeshProUGUI[] activeDigits = new TextMeshProUGUI[3];

  [Header("Animation Settings")]
  [SerializeField] private float slideDuration = 0.2f;
  [SerializeField] private float staggerDelay = 0.03f; // The delay between each digit moving

  private string[] currentValues = new string[3] { "0", "0", "0" };
  private Sequence counterSequence;
  private float dynamicSlideDistance;

  void Awake()
  {
    if (Instance == null) Instance = this;
    if (uiContainer != null) uiContainer.alpha = 0;

    SetupCounter();
    UpdateQuestObjectives();
  }

  void OnEnable()
  {
    GameEventsManager.Instance.questEvents.OnQuestStateChanged += OnQuestUpdated;
    GameplayManager.Instance.OnCameraSwitch += SetCameraMode;
  }

  void OnDisable()
  {
    GameEventsManager.Instance.questEvents.OnQuestStateChanged -= OnQuestUpdated;
    GameplayManager.Instance.OnCameraSwitch -= SetCameraMode;
  }

  public void Show()
  {
    uiContainer.gameObject.SetActive(true);
    uiContainer.DOKill();
    uiContainer.DOFade(1f, fadeDuration).SetUpdate(true);
  }

  public void Hide()
  {
    uiContainer.DOKill();
    uiContainer
      .DOFade(0f, fadeDuration)
      .SetUpdate(true)
      .OnComplete(() => uiContainer.gameObject.SetActive(false));
  }

  // Only show bottom UI during puzzle stage
  public void ToggleBottom(bool active)
  {
    bottomContainer.DOKill();

    if (active)
    {
      bottomContainer.gameObject.SetActive(true);
      bottomContainer.DOFade(1f, fadeDuration).SetUpdate(true);
    }
    else
    {
      bottomContainer
        .DOFade(0f, fadeDuration)
        .SetUpdate(true)
        .OnComplete(() => bottomContainer.gameObject.SetActive(false));
    }
  }

  public void SetStageName(string name) => stageName.text = name;

  public void SetCameraMode(CameraMode mode) => cameraMode.text = $"Camera {mode}";

  public void SetCounterOnTop(bool isOnTop)
  {
    if (counter == null) return;
    counter.overrideSorting = isOnTop;
  }

  void OnQuestUpdated(Quest quest) => UpdateQuestObjectives();

  void UpdateQuestObjectives()
  {
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

  void SetupCounter()
  {
    if (activeDigits[0] == null) return;

    // Force TMP to update so we can get accurate measurements immediately
    activeDigits[0].ForceMeshUpdate();

    // Get the exact preferred height of a standard character (like "0")
    Vector2 textSize = activeDigits[0].GetPreferredValues("0");
    dynamicSlideDistance = textSize.y;

    // Apply this exact height to all our containers so the Mask perfectly clips the digits
    for (int i = 0; i < digitContainers.Length; i++)
    {
      // Keep the original width, but snap the height to the text's exact pixel height
      digitContainers[i].sizeDelta = new Vector2(digitContainers[i].sizeDelta.x, dynamicSlideDistance);
    }
  }

  public void SetStepLeft(int targetStep, bool instant = false)
  {
    string newText = targetStep < 0 ? "XXX" : targetStep.ToString("D3");

    if (instant)
    {
      UpdateInstantly(newText);
      return;
    }

    counterSequence?.Kill(true);
    counterSequence = DOTween.Sequence();

    for (int i = 0; i < 3; i++)
    {
      string newDigitValue = newText[i].ToString();

      if (currentValues[i] != newDigitValue)
      {
        float startTime = (2 - i) * staggerDelay;
        AnimateDigit(i, newDigitValue, startTime);
      }
    }
  }

  void AnimateDigit(int index, string newValue, float startTime)
  {
    string currentValue = currentValues[index];

    // Default to spinning UP when transitioning to or from an "X"
    bool movesUp = true;

    // Safely try to parse the strings. If BOTH are numbers, use our wheel math.
    if (int.TryParse(currentValue, out int currentNum) && int.TryParse(newValue, out int newNum))
    {
      movesUp = (newNum > currentNum && !(currentNum == 0 && newNum == 9))
             || (currentNum == 9 && newNum == 0);
    }

    float startY = movesUp ? -dynamicSlideDistance : dynamicSlideDistance;
    float endY = movesUp ? dynamicSlideDistance : -dynamicSlideDistance;

    TextMeshProUGUI oldText = activeDigits[index];

    // 1. CLONE THE TEXT
    GameObject cloneObj = Instantiate(oldText.gameObject, digitContainers[index]);
    TextMeshProUGUI newText = cloneObj.GetComponent<TextMeshProUGUI>();

    // 2. SETUP CLONE
    newText.text = newValue;
    newText.rectTransform.anchoredPosition = new Vector2(0, startY);

    // Ensure the new clone gets the correct color immediately before sliding in
    newText.color = (newValue == "X") ? new Color32(210, 38, 38, 255) : Color.white;

    // 3. ANIMATE OLD TEXT (Slide out with anticipation)
    counterSequence.Insert(startTime, oldText.rectTransform.DOAnchorPosY(endY, slideDuration).SetEase(Ease.OutBack));

    // 4. ANIMATE NEW TEXT (Slide in with a bounce)
    counterSequence.Insert(startTime, newText.rectTransform.DOAnchorPosY(0, slideDuration).SetEase(Ease.OutBack));

    // 5. CLEANUP ON COMPLETE
    counterSequence.InsertCallback(startTime + slideDuration, () =>
    {
      if (oldText != null) Destroy(oldText.gameObject);
    });

    // 6. UPDATE REFERENCES
    activeDigits[index] = newText;
    currentValues[index] = newValue;
  }

  void UpdateInstantly(string newText)
  {
    counterSequence?.Kill(true);

    for (int i = 0; i < 3; i++)
    {
      string val = newText[i].ToString();
      currentValues[i] = val;

      activeDigits[i].text = val;
      activeDigits[i].rectTransform.anchoredPosition = Vector2.zero;

      if (val == "X") activeDigits[i].color = new Color32(210, 38, 38, 255);
      else activeDigits[i].color = Color.white;
    }
  }
}