using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

[Serializable]
public class GameData
{
  public long lastUpdated;
  public string progress;
  public string[] collectibles;

  public GameData()
  {
    progress = "chapter1_lobby";
    collectibles = new string[0];
  }

  public float GetProgressPercentage()
  {
    // Define the linear order of rooms
    List<string> progressionOrder = new List<string>();

    for (int c = 1; c <= 5; c++)
    {
      progressionOrder.Add($"chapter{c}_lobby");
      progressionOrder.Add($"chapter{c}_hallway1");
      progressionOrder.Add($"chapter{c}_hallway2");
      progressionOrder.Add($"chapter{c}_hallway3");
    }

    int currentIndex = progressionOrder.IndexOf(progress.ToLower());

    // If string isn't found, return 0 or a default
    if (currentIndex == -1) return 0f;

    // Calculate percentage (1st room = 5%, Last room = 100%)
    float percentage = (float)(currentIndex + 1) / progressionOrder.Count * 100f;

    return (float)Math.Round(percentage, 2);
  }

  public string GetTitle()
  {
    if (string.IsNullOrEmpty(progress) || !progress.Contains("_")) return "Unknown";

    // Split "chapter1_hallway2" -> ["chapter1", "hallway2"]
    string[] parts = progress.Split('_');
    string roomRaw = parts[1]; // "hallway2"

    // 1. Insert space between letters and numbers: "hallway2" -> "hallway 2"
    string withSpaces = Regex.Replace(roomRaw, @"(\p{L}+)(\d+)", "$1 $2");

    // 2. Capitalize: "hallway 2" -> "Hallway 2"
    TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
    return textInfo.ToTitleCase(withSpaces);
  }

  public string GetSubtitle()
  {
    if (string.IsNullOrEmpty(progress)) return "Chapter 0";

    string[] parts = progress.Split('_');

    if (parts.Length < 1) return "Chapter 1";

    // Format "chapter1" into "Chapter 1"
    string chapterPart = parts[0]; // e.g., "chapter1"

    // Extract the number from the end of the string
    string chapterNumber = chapterPart.Replace("chapter", "");

    return "Chapter " + chapterNumber;
  }
}