using UnityEditor;

[CustomEditor(typeof(Word))]
public class WordCustomEditor : Editor
{
    SerializedProperty word, wordClass;

    /// <summary>
    /// Fetches the serializable properties of the ArtifactData class
    /// </summary>
    protected void OnEnable()
    {
        word = serializedObject.FindProperty("word");
        wordClass = serializedObject.FindProperty("wordClass");
    }

    /// <summary>
    /// Updates the value of the properties when one is changed
    /// </summary>
    public override void OnInspectorGUI()
    {
        ShowInspector();
    }

    /// <summary>
    /// Updates the value of the properties when one is changed and calls the parent's properties to be updated the same way
    /// </summary>
    protected void ShowInspector()
    {
        serializedObject.Update();

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(word);
        EditorGUILayout.PropertyField(wordClass);

        serializedObject.ApplyModifiedProperties();
    }
}
