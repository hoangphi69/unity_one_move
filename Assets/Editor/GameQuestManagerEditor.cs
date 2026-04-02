using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameQuestManager))]
public class GameQuestManagerEditor : Editor
{
  public override void OnInspectorGUI()
  {
    // Draw the default inspector (if you have other public fields)
    base.OnInspectorGUI();

    // Only show the tracker while the game is actually running
    if (!Application.isPlaying)
    {
      EditorGUILayout.HelpBox("Enter Play Mode to view the Live Quest Tracker.", MessageType.Info);
      return;
    }

    GameQuestManager manager = (GameQuestManager)target;
    var liveQuests = manager.GetQuestDictionary();

    if (liveQuests == null || liveQuests.Count == 0) return;

    EditorGUILayout.Space(10);
    EditorGUILayout.LabelField("Live Quest Tracker", EditorStyles.boldLabel);

    // Draw a clean box for every quest in the manager
    foreach (var kvp in liveQuests)
    {
      Quest quest = kvp.Value;
      QuestState state = quest.GetState();

      // Set the outline color of the entire box based on state
      GUI.color = state == QuestState.COMPLETED ? new Color(0.7f, 1f, 0.7f) :
                  state == QuestState.FAILED ? new Color(1f, 0.7f, 0.7f) :
                  state == QuestState.ACTIVE ? new Color(0.7f, 0.9f, 1f) : Color.white;

      EditorGUILayout.BeginVertical("box");
      GUI.color = Color.white; // Reset color so everything inside isn't tinted

      // --- HEADER ROW ---
      EditorGUILayout.BeginHorizontal();

      // 1. Title (Bigger and Bolder)
      GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel);
      EditorGUILayout.LabelField(quest.title, titleStyle, GUILayout.Width(GUIStyle.none.CalcSize(new GUIContent(quest.title)).x + 10));

      // 2. Status "Chip"
      GUIStyle chipStyle = new GUIStyle(GUI.skin.box);
      chipStyle.normal.textColor = state == QuestState.UNKNOWN ? Color.gray : Color.white;
      chipStyle.fontStyle = FontStyle.Bold;
      chipStyle.padding = new RectOffset(8, 8, 2, 2); // Pad the text inside the box

      // Give the chip a solid background color based on state
      GUI.backgroundColor = state == QuestState.COMPLETED ? new Color(0.2f, 0.6f, 0.2f) :
                            state == QuestState.FAILED ? new Color(0.8f, 0.2f, 0.2f) :
                            state == QuestState.ACTIVE ? new Color(0.2f, 0.5f, 0.8f) :
                            state == QuestState.ACHIEVED ? new Color(0.8f, 0.6f, 0.1f) : Color.grey;

      GUILayout.Box(state.ToString(), chipStyle);
      GUI.backgroundColor = Color.white; // Reset background color

      // 3. Push the ID to the far right
      GUILayout.FlexibleSpace();

      // 4. ID (Small and gray)
      GUIStyle idStyle = new GUIStyle(EditorStyles.miniLabel);
      idStyle.normal.textColor = Color.gray;
      GUILayout.Label(quest.id, idStyle);

      EditorGUILayout.EndHorizontal();

      // --- OBJECTIVES LIST ---
      if (state == QuestState.ACTIVE || state == QuestState.ACHIEVED)
      {
        EditorGUILayout.Space(5);
        int currentObj = quest.GetCurrentObjectiveIndex();

        for (int i = 0; i < quest.objectives.Count; i++)
        {
          bool isDone = i < currentObj;
          bool isCurrent = i == currentObj;

          EditorGUILayout.BeginHorizontal();
          GUILayout.Space(15); // Indent the objectives list slightly

          // Set up the icon and colors based on objective status
          string icon = isDone ? "✓" : (isCurrent ? ">" : "-");

          if (isDone) GUI.contentColor = Color.gray;
          else if (isCurrent) GUI.contentColor = new Color(0.4f, 0.8f, 1f); // Bright Cyan
          else GUI.contentColor = new Color(0.7f, 0.7f, 0.7f); // Dim white for future objectives

          // Make the current objective bold, others normal
          GUIStyle objStyle = isCurrent ? EditorStyles.boldLabel : EditorStyles.label;

          // Draw the objective
          GUILayout.Label($"{icon}  {quest.objectives[i].description}", objStyle);

          GUI.contentColor = Color.white; // Reset text color
          EditorGUILayout.EndHorizontal();
        }
      }

      EditorGUILayout.EndVertical();
      EditorGUILayout.Space(3); // Add a tiny gap between quests
    }

    // Force the inspector to repaint constantly so it updates in real-time
    Repaint();
  }
}