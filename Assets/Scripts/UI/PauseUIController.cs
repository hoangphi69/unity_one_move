using UnityEngine;
using UnityEngine.UIElements;

public class PauseUIController : MonoBehaviour
{
  private VisualElement root;
  private Button resume;
  private Button restart;
  private Button options;
  private Button returnToTitle;

  void Awake()
  {
    root = GetComponent<UIDocument>().rootVisualElement;
    resume = root.Q<Button>("resume");
    restart = root.Q<Button>("restart");
    options = root.Q<Button>("options");
    returnToTitle = root.Q<Button>("return_to_title");
  }

  void OnEnable()
  {
    resume.clicked += GameSceneManager.Instance.TogglePause;
    restart.clicked += GameSceneManager.Instance.RestartCurrentLevel;

    returnToTitle.clicked += GameSceneManager.Instance.ReturnToTitle;
  }

  void OnDisable()
  {
    resume.clicked -= GameSceneManager.Instance.TogglePause;
    restart.clicked -= GameSceneManager.Instance.RestartCurrentLevel;

    returnToTitle.clicked -= GameSceneManager.Instance.ReturnToTitle;
  }

  void Start()
  {
    root.schedule.Execute(() => resume.Focus());
  }
}