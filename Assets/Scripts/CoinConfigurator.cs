using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CoinConfigurator : MonoBehaviour
{
    public GameConfig GameConfig;
    public BoxCollider2D SpawnArea;
    public Coin CoinPrefab;
}

#if UNITY_EDITOR
public class CoinConfiguratorWindow : EditorWindow
{
    public string Filename = "";
    public int TargetAmount;

    public int SolutionCount;
    public bool IsSolvable;

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
        IsSolvable = EditorGUILayout.Toggle("Solvable:", IsSolvable);

        GUILayout.BeginHorizontal();
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
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        bool newConfig = false;
        if (GUILayout.Button("New Configuration"))
        {
            NewConfiguration();
            newConfig = true;
        }

        if (GUILayout.Button("New Random Configuration"))
        {
            NewConfiguration();
            var noDummies = config.GameConfig.Coins.Where(cfg => !cfg.IsDummy).ToList();
            var coinConfigs = SolutionHelper.GenerateRandomValues(noDummies);
            foreach (var cfg in coinConfigs)
            {
                SpawnCoin(config.SpawnArea.bounds, config.CoinPrefab, cfg);
            }

            newConfig = true;
        }
        GUILayout.EndHorizontal();

        if (newConfig || GUILayout.Button("Suggest sum")) {
            var coins = GameObject.FindObjectsOfType<Coin>();
            var amounts = coins.Select(coin => coin.Config.Amount).ToList();
            TargetAmount = SolutionHelper.PickSumFor(amounts);
            newConfig = true;
        }

        if (newConfig || GUILayout.Button("Update Solution Count: " + SolutionCount)) {
            // expensive to calculate without keeping a list of coins around
            var coins = GameObject.FindObjectsOfType<Coin>();
            var amounts = coins.Select(coin => coin.Config.Amount).ToList();
            SolutionCount = SolutionHelper.NumberSolutions(amounts, TargetAmount);
            IsSolvable = SolutionCount > 0;
        }

    }

    [MenuItem("Window/Configurator")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(CoinConfiguratorWindow));
    }

    private void UpdateSolutionCount() {
    }

    private void SpawnCoin(Bounds bounds, Coin coinPrefab, CoinConfig cfg) {
        var posX = Random.Range(bounds.min.x, bounds.max.x);
        var posY = Random.Range(bounds.min.y, bounds.max.y);
        var pos = new Vector3(posX, posY);
        var rotation = Random.rotation;
        rotation.y = 0;
        rotation.x = 0;
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
        IsSolvable = coinSpawns.IsSolvable;
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
        spawnAsset.IsSolvable = IsSolvable;

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
        foreach(var coin in coins) {
            DestroyImmediate(coin.gameObject);
        }
    }
}
#endif