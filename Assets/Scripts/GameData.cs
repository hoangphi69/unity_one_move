using System;

[Serializable]
public class GameData
{
  public float progress;
  public string[] collectibles;
  public int level;

  public GameData()
  {
    level = 1;
    progress = 1.0f;
    collectibles = new string[0];
  }
}