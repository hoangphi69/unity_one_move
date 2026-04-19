using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LogPanel : MonoBehaviour
{
  [SerializeField] private GameObject entryPrefab;
  [SerializeField] private GameObject content;

  private List<(string speaker, string text)> entries = new();

  void Start()
  {
    gameObject.SetActive(false);
  }

  public void AddEntry(string speaker, string text)
  {
    entries.Add((speaker, text));
  }

  public void Show()
  {
    gameObject.SetActive(true);

    // Clear existing visual objects before rebuilding
    foreach (Transform child in content.transform) Destroy(child.gameObject);

    // Instantiate a prefab for every entry in our list
    foreach (var (speaker, text) in entries)
    {
      GameObject obj = Instantiate(entryPrefab, content.transform);
      TextMeshProUGUI[] texts = obj.GetComponentsInChildren<TextMeshProUGUI>();

      if (texts.Length >= 2)
      {
        texts[0].text = speaker; // Speaker
        texts[1].text = text;    // Dialogue
      }
    }
  }

  public void Hide()
  {
    gameObject.SetActive(false);
  }

  public void ClearEntries() => entries.Clear();
}