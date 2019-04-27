using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CoinConfigurator : MonoBehaviour
{
    public string Path = "Assets/Config";
    public string Filename = "test.asset";
    public GameConfig GameConfig;

}


[CustomEditor(typeof(CoinConfigurator))]
public class CoinConfiguratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var config = (CoinConfigurator)target;

        if (config.GameConfig != null) {
            foreach (var coin in config.GameConfig.Coins)
            {
                if (GUILayout.Button(coin.Sprite.texture, GUILayout.Width(100), GUILayout.Height(100))) {

                }
            }
        }
        DrawDefaultInspector();
        
        if(GUILayout.Button("Save Configuration"))
        {
            SaveConfiguration(config);
        }
    }

    private void SaveConfiguration(CoinConfigurator target) {
        var spawnAsset = ScriptableObject.CreateInstance<CoinSpawns>();
        var targetPath = target.Path + "/" + target.Filename;
        var uniquePath = AssetDatabase.GenerateUniqueAssetPath(targetPath);

        var coins = GameObject.FindObjectsOfType<Coin>();

        foreach (var coin in coins) {
            var spawn = new CoinSpawn();
            spawn.CoinID = coin.Config.CoinID;
            spawn.Position = coin.transform.position;
            spawn.Rotation = coin.transform.rotation;
            spawnAsset.Spawns.Add(spawn);
        }
        Debug.Log("path is " + uniquePath + " for " + targetPath);
        AssetDatabase.CreateAsset(spawnAsset, uniquePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = spawnAsset;

    }
}