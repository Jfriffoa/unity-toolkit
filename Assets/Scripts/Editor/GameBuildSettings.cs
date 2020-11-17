using System.IO;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

public class GameBuildSettings : ScriptableObject {
    public int bundleVersion;         // Player Settings
    public List<GameSettings> settings;     

    [System.Serializable]
    public struct GameSettings {
        public string productName;      // Player Settings
        public string version;          //
        public Color backgroundColor;
        public List<SceneAsset> scenes; 

        internal bool _collapsed;       // Editor;
    }

    public static GameBuildSettings GetDefault() {
        string objectName = "cl.munditodt.gamebuildsettings";
        string path = "Assets/GameBuildSettings.asset";
        GameBuildSettings data = null;

        // Try to get it
        if (EditorBuildSettings.TryGetConfigObject<GameBuildSettings>(objectName, out data))
            return data;

        // If not, try to search the asset
        if (File.Exists(path))
            data = AssetDatabase.LoadAssetAtPath<GameBuildSettings>(path);

        // Create new if nothing exists
        if (data == null) {
            // Show dialog and save
            path = EditorUtility.SaveFilePanelInProject("New Build Settings File", "GameBuildSettings", "asset", "Select Config File Asset", "Assets");
            // Initialize
            data = ScriptableObject.CreateInstance<GameBuildSettings>();
            // Create and save
            AssetDatabase.CreateAsset(data, path);
        }

        EditorBuildSettings.AddConfigObject(objectName, data, false);
        return data;
    }
}
