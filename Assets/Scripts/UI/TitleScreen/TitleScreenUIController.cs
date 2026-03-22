using UnityEngine;

public static class TitleScreenRoutes
{
  public const string TITLE = "title";
  public const string OPTIONS = "options";
  public const string SAVES = "saves";
}

public class TitleScreenUIController : NavigationUIController
{
  public static TitleScreenUIController Instance { get; private set; }

  [Header("Panels")]
  [SerializeField] private NavigationPanel titlePanel;
  [SerializeField] private NavigationPanel optionsPanel;
  [SerializeField] private NavigationPanel savesPanel;

  void Awake()
  {
    Instance = this;

    RegisterPanel(TitleScreenRoutes.TITLE, titlePanel);
    RegisterPanel(TitleScreenRoutes.OPTIONS, optionsPanel);
    RegisterPanel(TitleScreenRoutes.SAVES, savesPanel);
  }

  void Start() => Show();

  void OnDestroy() => UnregisterAllPanels();

  public override void Show()
  {
    base.Show();
    ClearStack();
    NavigateToPanel(TitleScreenRoutes.TITLE);
  }
}