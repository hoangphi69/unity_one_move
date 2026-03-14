using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class GameDataFileHandler
{
  private readonly string filePath;
  private readonly string fileName;

  public GameDataFileHandler(string filePath, string fileName)
  {
    this.filePath = filePath;
    this.fileName = fileName;
  }

  public void DeleteFile(string profileID)
  {
    string path = Path.Combine(filePath, profileID, fileName);
    if (File.Exists(path)) File.Delete(path);
  }

  public async Task<GameData> LoadFile(string profileID)
  {
    string path = Path.Combine(filePath, profileID, fileName);
    if (!File.Exists(path)) return null;

    try
    {
      string dataJSON = await File.ReadAllTextAsync(path);
      return JsonUtility.FromJson<GameData>(dataJSON);
    }
    catch (Exception e)
    {
      Debug.LogError($"Error loading data from {path}: {e.Message}");
      return null;
    }
  }

  public async Task SaveFile(GameData data, string profileID)
  {
    string path = Path.Combine(filePath, profileID, fileName);
    try
    {
      Directory.CreateDirectory(Path.GetDirectoryName(path));
      string dataJSON = JsonUtility.ToJson(data, true);
      await File.WriteAllTextAsync(path, dataJSON);
    }
    catch (Exception e)
    {
      Debug.LogError($"Error saving data to {path}: {e.Message}");
    }
  }

  public async Task<string> GetRecentlyUpdatedProfileID()
  {
    Dictionary<string, GameData> profiles = await LoadAllProfiles();

    string mostRecentID = null;
    long latestTimestamp = long.MinValue;

    foreach (var profile in profiles)
    {
      string profileID = profile.Key;
      GameData data = profile.Value;

      if (data == null) continue;

      // Compare the binary timestamps
      if (data.lastUpdated > latestTimestamp)
      {
        latestTimestamp = data.lastUpdated;
        mostRecentID = profileID;
      }
    }

    return mostRecentID;
  }

  public async Task<Dictionary<string, GameData>> LoadAllProfiles()
  {
    Dictionary<string, GameData> profiles = new();

    IEnumerable<DirectoryInfo> directories = new DirectoryInfo(filePath).EnumerateDirectories();
    foreach (DirectoryInfo directory in directories)
    {
      string profileID = directory.Name;
      string path = Path.Combine(filePath, profileID, fileName);
      if (!File.Exists(path)) continue;

      GameData data = await LoadFile(profileID);
      if (data != null) profiles.Add(profileID, data);
      else Debug.LogError("Error loading profile at Profile ID: " + profileID);
    }

    return profiles;
  }
}