using System.Threading.Tasks;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
  public static GameDataManager Instance { get; private set; }

  // Configs
  [SerializeField] private string fileName = "data.game";

  public GameData data { get; private set; }
  private GameDataFileHandler dataFileHandler;

  void Awake()
  {
    Instance = this;
    dataFileHandler = new(Application.persistentDataPath, fileName);
  }

  void OnApplicationQuit()
  {
    _ = SaveGameAsync();
  }

  public bool HasData()
  {
    if (data != null) print("Data progress: " + data.progress);
    return data != null;
  }

  public async Task SaveProgress(string stageName)
  {
    data.progress = stageName;
    await SaveGameAsync();
  }

  public string GetProgress() => data.progress;

  public async Task LoadGameAsync()
  {
    data = await dataFileHandler.LoadFile();

    if (data == null)
    {
      print("No save file found.");
    }
    else
    {
      print($"Loaded Save. Progress: {data.progress}");
    }
  }

  public async Task SaveGameAsync()
  {
    if (data == null) return;
    await dataFileHandler.SaveFile(data);
    print("Game Saved.");
  }

  public void NewGame()
  {
    data = new();
    _ = SaveGameAsync();
  }
}