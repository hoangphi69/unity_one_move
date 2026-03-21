using UnityEngine;

public class LoadingScreenUIController : MonoBehaviour
{
  public static LoadingScreenUIController Instance { get; private set; }

  [SerializeField] private GameObject uiContainer;


  void Awake()
  {
    if (Instance == null) Instance = this;
    else Destroy(gameObject);
  }

  public void Show() => uiContainer.SetActive(true);

  public void Hide() => uiContainer.SetActive(false);
}