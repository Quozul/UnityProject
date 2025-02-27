using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class EditorGenerator : EditorWindow
{
    private struct GeneratorData
    {
        public int length;
        public int max;
        public int min;
        public int height;
    }
    
    private Object _lastGameObject;

    private GeneratorData _data = new GeneratorData
    {
        // Set default values:
        min = -8,
        max = 8,
        length = 10,
        height = 2,
    };

    [MenuItem("Tools/Generator")]
    public static void ExecButton()
    {
        GetWindow<EditorGenerator>("Generateur");
    }

    private void OnGUI()
    {
        DrawGUI();
    }

    private string _blenderExecutable;

    private void DrawGUI()
    {
        // Main structure : 
        EditorGUILayout.BeginVertical();

        // Length
        GUILayout.Label("Longueur du circuit : ");
        _data.length = EditorGUILayout.IntField(_data.length);

        // Min / Max
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Valeur min : ");

        _data.min = EditorGUILayout.IntField(_data.min);
        GUILayout.Label("Valeur max : ");

        _data.max = EditorGUILayout.IntField(_data.max);
        EditorGUILayout.EndHorizontal();
        // Height

        GUILayout.Label("Hauteur max du circuit");
        _data.height = EditorGUILayout.IntField(_data.height);

        EditorGUILayout.LabelField("Sélectionner Blender");
        if (GUILayout.Button("Sélectionner Blender"))
        {
            _blenderExecutable = EditorUtility.OpenFilePanel("Sélectionner Blender", "/", "exe");
        }

        GUILayout.Label(_blenderExecutable?.Length > 0 ? _blenderExecutable : "null");

        // Button
        EditorGUILayout.LabelField("Generateur de Maps");
        if (GUILayout.Button("Generate"))
        {
            LaunchProcess(_data);
        }

        EditorGUILayout.EndVertical();
    }

    private void LaunchProcess(GeneratorData data)
    {
        const string fileName = "Assets/Editor/map.fbx";
        var pwd = Directory.GetCurrentDirectory() + "/" + fileName;

        if (_blenderExecutable != "")
        {
            // Passer les chemins en absolu via os.system()
            var info = new ProcessStartInfo
            {
                FileName = _blenderExecutable,
                Arguments = "--background --python \"../Blender/random_map_generator.py\"",
                EnvironmentVariables =
                {
                    {"OUTPUT_PATH", pwd},
                    {"MIN_X", data.min.ToString()},
                    {"MIN_Y", data.max.ToString()},
                    {"TRACK_LENGTH", data.length.ToString()},
                    {"HEIGHT", data.height.ToString()}
                },
                UseShellExecute = false,
            };

            var process = new Process
            {
                StartInfo = info,
            };

            process.Start();
            process.WaitForExit();
            process.Close();

            // Add game object to scene
            AssetDatabase.Refresh();

            if (_lastGameObject != null)
            {
                DestroyImmediate(_lastGameObject);
            }
            
            var gameObject = (GameObject) AssetDatabase.LoadAssetAtPath(fileName, typeof(GameObject));
            _lastGameObject = Instantiate(gameObject);
        }
    }
}