using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class GameDataFileHandler
{
  private string path;

  public GameDataFileHandler(string filePath, string fileName)
  {
    path = Path.Combine(filePath, fileName);
  }

  public async Task<GameData> LoadFile()
  {
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

  public async Task SaveFile(GameData data)
  {
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
}