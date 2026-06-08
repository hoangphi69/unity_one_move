using UnityEditor;

[CustomEditor(typeof(GameAudioManager))]
public class GameAudioManagerEditor : Editor
{
  public override void OnInspectorGUI()
  {
    GameAudioManager manager = (GameAudioManager)target;

    DrawDefaultInspector();

    // If our OnValidate method detected a duplicate, draw the warning box!
    if (manager.hasDuplicateTags)
    {
      EditorGUILayout.Space();

      EditorGUILayout.HelpBox(
          "DUPLICATE TAGS DETECTED!\n" +
          "You have assigned the same AudioTag multiple times in the list below. " +
          "The dictionary will ignore the duplicates at runtime. Please fix this.",
          MessageType.Warning);
    }
  }
}