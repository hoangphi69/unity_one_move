using System;
using UnityEngine;

[Serializable]
public abstract class ObjectiveInfo
{
  private string questID;
  private bool completed = false;

  public virtual void Initialize(string questID)
  {
    this.questID = questID;
  }

  protected void Complete()
  {
    if (completed) return;
    completed = true;
    GameEventsManager.Instance.questEvents.AdvanceQuest(questID);
  }

  public virtual string Save()
  {
    return JsonUtility.ToJson(this);
  }

  public virtual void Load(string json)
  {
    if (string.IsNullOrEmpty(json)) return;
    JsonUtility.FromJsonOverwrite(json, this);
  }
}

[Serializable]
public class Objective
{
  public string description;
  [SerializeReference, SubclassSelector] public ObjectiveInfo details;
}