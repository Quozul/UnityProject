using UnityEditor;
using UnityEngine;

public class EditorGenerator : EditorWindow
{
    
    struct GeneratorData
    {
        public int length;
        public int max;
        public int min;
        public int height;
    }

    [MenuItem("Tools/Generator")]
    public static void ExecButton()
    {
        GetWindow<EditorGenerator>("Generateur");
    }

    private void OnGUI()
    {
        GeneratorData data = new GeneratorData();
        DrawGUI(data);
    }

    private void DrawGUI(GeneratorData data)
    {
        // Main structure : 
        EditorGUILayout.BeginVertical();
        
        // Length
        GUILayout.Label("Longueur du circuit : ");
        data.length = EditorGUILayout.IntField(data.length);
        
        // Min / Max
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Valeur min : ");
        data.min = EditorGUILayout.IntField(data.min);
        GUILayout.Label("Valeur max : ");
        data.max = EditorGUILayout.IntField(data.max);
        EditorGUILayout.EndHorizontal();
        // Height
        
        GUILayout.Label("Hauteur max du circuit");
        data.height = EditorGUILayout.IntField(data.height);
        
        // Button
        EditorGUILayout.LabelField("Generateur de Maps");
        if (GUILayout.Button("Generate"))
        {
            Debug.Log("Afficher une map");
        }
        
        EditorGUILayout.EndVertical();
    }
}
