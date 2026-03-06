using System;

[Serializable]
public class GameData
{
  public string progress;
  public string[] collectibles;
  public int level;

  public GameData()
  {
    level = 1;
    progress = "Lobby_1";
    collectibles = new string[0];
  }
}