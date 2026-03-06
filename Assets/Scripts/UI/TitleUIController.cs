using UnityEngine;
using UnityEngine.UIElements;

public class TitleUIController : MonoBehaviour
{
  private VisualElement root;
  private Button continueGame;
  private Button newGame;
  private Button settings;
  private Button quit;

  void Awake()
  {
    root = GetComponent<UIDocument>().rootVisualElement;
    continueGame = root.Q<Button>("continue");
    newGame = root.Q<Button>("new_game");
    settings = root.Q<Button>("settings");
    quit = root.Q<Button>("quit");
  }

  void OnEnable()
  {
    continueGame.clicked += GameSceneManager.Instance.OnTitleContinue;
    newGame.clicked += GameSceneManager.Instance.OnTitleNewGame;
    quit.clicked += Application.Quit;
  }

  void OnDisable()
  {
    continueGame.clicked -= GameSceneManager.Instance.OnTitleContinue;
    newGame.clicked -= GameSceneManager.Instance.OnTitleNewGame;
    quit.clicked -= Application.Quit;
  }

  void Start()
  {
    if (!GameDataManager.Instance.HasData())
    {
      continueGame.style.display = DisplayStyle.None;
      root.schedule.Execute(() => newGame.Focus());
    }
    else
    {
      root.schedule.Execute(() => continueGame.Focus());
    }
  }
}