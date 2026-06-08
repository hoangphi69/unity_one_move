using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(AudioMap))]
public class InkFMODMappingDrawer : PropertyDrawer
{
  public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
  {
    var eventProp = property.FindPropertyRelative("audio");
    return EditorGUI.GetPropertyHeight(eventProp, true);
  }

  public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
  {
    EditorGUI.BeginProperty(position, label, property);

    var eventProp = property.FindPropertyRelative("audio");
    var tagProp = property.FindPropertyRelative("tag");

    float gap = 12f;
    float singleLine = EditorGUIUtility.singleLineHeight;

    // 1. Calculate widths (30% for Tag on the left, 70% for FMOD on the right)
    float tagWidth = position.width * 0.30f;
    float eventWidth = position.width * 0.70f - gap;

    // 2. Build Rects
    Rect tagRect = new Rect(position.x, position.y, tagWidth, singleLine);
    Rect eventRect = new Rect(position.x + tagWidth + gap, position.y, eventWidth, position.height);

    // 3. Draw the Enum Tag (Left)
    EditorGUI.PropertyField(tagRect, tagProp, GUIContent.none);

    // --- THE FIX ---
    // Save the original label width so we don't break the rest of the inspector
    float originalLabelWidth = EditorGUIUtility.labelWidth;

    // Shrink the reserved label space to practically zero
    EditorGUIUtility.labelWidth = 1f;

    // 4. Draw the FMOD Event (Right)
    EditorGUI.PropertyField(eventRect, eventProp, GUIContent.none, true);

    // Restore the original label width for the next things drawn in the Inspector
    EditorGUIUtility.labelWidth = originalLabelWidth;

    EditorGUI.EndProperty();
  }
}