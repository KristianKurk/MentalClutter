using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(RhythmData.Sequence))]
public class RhythmEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        var wordRect = new Rect(position.x, position.y, 30, position.height);
        var beatRect = new Rect(position.x + 90, position.y, position.width - 90, position.height);

        EditorGUI.PropertyField(wordRect, property.FindPropertyRelative("beat"), GUIContent.none);
        EditorGUI.PropertyField(beatRect, property.FindPropertyRelative("word"), GUIContent.none);

        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
