using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
  public static GameDataManager Instance { get; private set; }

  // Configs
  [SerializeField] private string fileName = "data.game";

  public string selectedProfileID = "";
  public GameData data { get; private set; }
  private GameDataFileHandler dataFileHandler;

  void Awake()
  {
    Instance = this;
    dataFileHandler = new(Application.persistentDataPath, fileName);
  }

  void OnApplicationQuit()
  {
    _ = SaveGame();
  }

  public async Task Initialize()
  {
    string profileID = await dataFileHandler.GetRecentlyUpdatedProfileID();
    print($"active profileID: {profileID}");
    if (string.IsNullOrEmpty(profileID)) profileID = "1";
    await SwitchProfile(profileID);
  }

  public bool HasData()
  {
    if (data != null) print("Data progress: " + data.progress);
    return data != null;
  }

  public async Task SaveProgress(string stageName)
  {
    data.progress = stageName;
    await SaveGame();
  }

  public string GetProgress() => data.progress;

  public async Task SwitchProfile(string profileID)
  {
    selectedProfileID = profileID;
    await LoadGame();
  }

  public void EraseProfile(string profileID)
  {
    dataFileHandler.DeleteFile(profileID);

    if (selectedProfileID == profileID) data = null;
  }

  public async Task<Dictionary<string, GameData>> GetAllProfiles()
  {
    return await dataFileHandler.LoadAllProfiles();
  }


  public async Task LoadGame()
  {
    data = await dataFileHandler.LoadFile(selectedProfileID);

    if (data == null)
    {
      print("No save file found.");
    }
    else
    {
      print($"Loaded Save. Progress: {data.progress}");
    }
  }

  public async Task SaveGame() => await SaveGame(selectedProfileID);

  public async Task SaveGame(string profileID)
  {
    if (data == null) return;
    data.lastUpdated = DateTime.Now.ToBinary();
    await dataFileHandler.SaveFile(data, profileID);
    print("Game Saved.");
  }

  public void NewGame()
  {
    data = new();
    _ = SaveGame();
  }
}