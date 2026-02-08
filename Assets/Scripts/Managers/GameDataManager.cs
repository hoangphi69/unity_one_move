using System.Threading.Tasks;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
  public static GameDataManager Instance { get; private set; }

  public GameData data;

  [SerializeField] private string fileName;
  private GameDataFileHandler dataFileHandler;

  [SerializeField] public GameProgressMap progressMap;

  void Awake()
  {
    Instance = this;
    dataFileHandler = new(Application.persistentDataPath, fileName);
  }

  void OnApplicationQuit()
  {
    _ = SaveGameAsync();
  }

  public bool HasData() => data != null;

  public async Task SaveProgress(float progress)
  {
    if (data.progress >= progress) return;
    data.progress = progress;
    await SaveGameAsync();
  }

  public SceneField GetLobbyScene()
  {
    return progressMap.GetLobbyScene(data.progress);
  }

  public SceneField GetGameplayScene()
  {
    return progressMap.GetGameplayScene(data.progress);
  }

  public async Task LoadGameAsync()
  {
    data = await dataFileHandler.LoadFile();

    if (data == null)
    {
      Debug.Log("No save file found.");
    }
    else
    {
      Debug.Log($"Loaded Save. Progress: {data.progress}");
    }
  }

  public async Task SaveGameAsync()
  {
    if (data == null) return;
    await dataFileHandler.SaveFile(data);
    Debug.Log("Game Saved.");
  }

  public void NewGame()
  {
    data = new();
    _ = SaveGameAsync();
  }
}