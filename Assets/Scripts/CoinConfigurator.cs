using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CoinConfigurator : MonoBehaviour
{
    public GameConfig GameConfig;
    public BoxCollider2D SpawnArea;
    public Coin CoinPrefab;
}


//[CustomEditor(typeof(CoinConfigurator))]
public class CoinConfiguratorWindow : EditorWindow
{
    public string Filename = "";
    public int TargetAmount;

    void OnGUI()
    {
        var config = GameObject.Find("ConfiguratorSettings").GetComponent<CoinConfigurator>();
        if (!config)
            return;

        if (config.GameConfig != null) {
            int count = 0;
            bool began = false;
            foreach (var coin in config.GameConfig.Coins)
            {
                if (count % 3 == 0) {
                    if (began) GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    began = true;
                }
                count++;
                if (GUILayout.Button(coin.Sprite.texture, GUILayout.Width(100), GUILayout.Height(100))) {
                    SpawnCoin(config.SpawnArea.bounds, config.CoinPrefab, coin);
                }
            }

            GUILayout.EndHorizontal();
        }

        Filename = EditorGUILayout.TextField("Filename:", Filename);
        TargetAmount = EditorGUILayout.IntField("Target amount:", TargetAmount);

        if (GUILayout.Button("Load Configuration")) {
            EditorGUIUtility.ShowObjectPicker<CoinSpawns>(null, true, "", EditorGUIUtility.GetControlID(FocusType.Passive) + 100);
        }

        if (Event.current.commandName == "ObjectSelectorClosed") {
            LoadConfiguration(config.GameConfig, config.CoinPrefab, EditorGUIUtility.GetObjectPickerObject());
        }

        if(GUILayout.Button("Save Configuration"))
        {
            SaveConfiguration();
        }

        if (GUILayout.Button("New Configuration"))
        {
            NewConfiguration();
        }
    }

    [MenuItem("Window/Configurator")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(CoinConfiguratorWindow));
    }

    private void SpawnCoin(Bounds bounds, Coin coinPrefab, CoinConfig cfg) {
        var posX = Random.Range(bounds.min.x, bounds.max.x);
        var posY = Random.Range(bounds.min.y, bounds.max.y);
        var pos = new Vector3(posX, posY);
        var rotation = Quaternion.identity;
        var obj = Instantiate(coinPrefab, pos, rotation);
        var coin = obj.GetComponent<Coin>();
        coin.Config = cfg;
        coin.ApplyConfig();
    }

    private void SpawnCoin(GameConfig gameConfig, Coin coinPrefab, CoinSpawn spawn)
    {
        var obj = Instantiate(coinPrefab, spawn.Position, spawn.Rotation);
        var coin = obj.GetComponent<Coin>();
        coin.Config = gameConfig.GetCoinConfig(spawn.CoinID);
        coin.ApplyConfig();
    }


    private void NewConfiguration() {
        Filename = "";
        DestroyCoins();
    }

    private void LoadConfiguration(GameConfig gameConfig, Coin coinPrefab, Object obj) {
        var coinSpawns = (CoinSpawns)obj;
        Filename = AssetDatabase.GetAssetPath(coinSpawns);
        TargetAmount = coinSpawns.TargetAmount;
        DestroyCoins();

        foreach(var spawn in coinSpawns.Spawns)
        {
            SpawnCoin(gameConfig, coinPrefab, spawn);
        }
    }
    private void SaveConfiguration() {
        var spawnAsset = ScriptableObject.CreateInstance<CoinSpawns>();
        var targetPath = Filename;
        if (targetPath.Length == 0)
        {
            Filename = targetPath = AssetDatabase.GenerateUniqueAssetPath("Assets/Config/Spawns/Spawn.asset");
        }

        var coins = GameObject.FindObjectsOfType<Coin>();
        spawnAsset.TargetAmount = TargetAmount;

        foreach (var coin in coins) {
            var spawn = new CoinSpawn();
            spawn.CoinID = coin.Config.CoinID;
            spawn.Position = coin.transform.position;
            spawn.Rotation = coin.transform.rotation;
            spawnAsset.Spawns.Add(spawn);
        }
        AssetDatabase.CreateAsset(spawnAsset, targetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = spawnAsset;
    }

    private void DestroyCoins() {
        var coins = GameObject.FindObjectsOfType<Coin>();
        Debug.Log("search");
        foreach(var coin in coins) {
            Debug.Log("destroy");
            DestroyImmediate(coin.gameObject);
        }
    }
}