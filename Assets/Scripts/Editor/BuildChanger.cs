using System.Collections; 
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

public class BuildChanger : EditorWindow {
    //List<SceneAsset> _sceneAssets = new List<SceneAsset>();
    GameBuildSettings _gameBuildSettings;

    [MenuItem("Window/Build Changer")]
    public static void ShowWindow() {
        EditorWindow.GetWindow(typeof(BuildChanger));
    }

    void Awake() {
        if (_gameBuildSettings == null)
            _gameBuildSettings = GameBuildSettings.GetDefault();
    }

    void OnGUI() {
        // Settins Generales
        EditorGUILayout.LabelField("General Settings", EditorStyles.boldLabel);
        GUILayout.Space(4);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Bundle Version:");
        _gameBuildSettings.bundleVersion = EditorGUILayout.IntField(_gameBuildSettings.bundleVersion);
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(8);

        EditorGUILayout.LabelField("Game Settings", EditorStyles.boldLabel);
        GUILayout.Space(4);

        EditorGUI.indentLevel++;
        for (int i = 0; i < _gameBuildSettings.settings.Count; i++) {
            var game = _gameBuildSettings.settings[i];

            game._collapsed = EditorGUILayout.Foldout(game._collapsed, (!string.IsNullOrEmpty(game.productName) ? game.productName : "None"), true);

            //EditorGUILayout.LabelField(game.productName + " Settings:", EditorStyles.boldLabel);
            if (game._collapsed) {
                EditorGUI.indentLevel++;

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Product Name:");
                game.productName = EditorGUILayout.TextField(game.productName);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Game Version:");
                game.version = EditorGUILayout.TextField(game.version);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Background Color:");
                game.backgroundColor = EditorGUILayout.ColorField(game.backgroundColor);
                EditorGUILayout.EndHorizontal();

                GUILayout.Space(2);

                EditorGUILayout.LabelField("Scene List:", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                for (int j = 0; j < game.scenes.Count; j++) {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel("Scene " + j + ":");
                    game.scenes[j] = (SceneAsset)EditorGUILayout.ObjectField(game.scenes[j], typeof(SceneAsset), false);
                    EditorGUILayout.EndHorizontal();
                }

                if (GUILayout.Button("Add Scene")) {
                    game.scenes.Add(null);
                }
                EditorGUI.indentLevel--;


                if (GUILayout.Button("Apply " + game.productName + " Settings")) {
                    SetEditorBuildSettingsScenes(i);
                }

                EditorGUI.indentLevel--;
            }

            _gameBuildSettings.settings[i] = game;
            GUILayout.Space(4);
        }
        EditorGUI.indentLevel--;

        GUILayout.Space(12);

        if (GUILayout.Button("Add Game Build Config")) {
            var game = new GameBuildSettings.GameSettings {
                productName = "Nuevo Juego",
                version = "v0.0",
                backgroundColor = Color.white,
                scenes = new List<SceneAsset>(2),
                _collapsed = false
            };

            _gameBuildSettings.settings.Add(game);
        }

    }

    void SetEditorBuildSettingsScenes(int gameIndex) {
        //Find a valid path for each scene
        var editorScenes = new List<EditorBuildSettingsScene>();

        // Add all the scenes but only activate the ones to be use
        for (int i = 0; i < _gameBuildSettings.settings.Count; i++) {
            var game = _gameBuildSettings.settings[i];
            foreach (var scene in game.scenes) {
                string path = AssetDatabase.GetAssetPath(scene);
                if (!string.IsNullOrEmpty(path)) {
                    editorScenes.Add(new EditorBuildSettingsScene(path, i == gameIndex));
                }
            }
        }

        // Set the scenes
        EditorBuildSettings.scenes = editorScenes.ToArray();

        // Set the rest of the settings
        var settings = _gameBuildSettings.settings[gameIndex];

        PlayerSettings.productName = settings.productName;
        PlayerSettings.bundleVersion = settings.version;
        PlayerSettings.Android.bundleVersionCode = _gameBuildSettings.bundleVersion;
        PlayerSettings.SplashScreen.backgroundColor = settings.backgroundColor;
        
        //bgcolor
    }
}
