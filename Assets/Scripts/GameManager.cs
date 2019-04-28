using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [Range(5, 200)]
    public int TargetSum = 100;
    [Range(0, 1)]
    public float MaxHealthPoints = 1f;
    public float HealthPoints;
    public GameConfig gameConfig;
    public Coin coinPrefab;

    public float SuccessReward = 0.25f;
    public float FailPenalty = 0.15f;
    public HUDManager HUD;

    private enum GameState {
        Evaluating,
        EvaluationComplete,
        SuccessfulRound,
        FailedRound,
        Waiting
    }

    private GameState gameState;
    private int CurrentSum = 0;
    private int CoinCounter = 0;
    private bool DummyCoinAccepted = false;

    // Start is called before the first frame update
    void Start() {
        HUD.Score = 0;
        StartRound();
    }

    private void StartRound() {
        var randomIndex = Random.Range(0, gameConfig.CoinSpawns.Count);
        Debug.Log(randomIndex);
        var spawn = gameConfig.CoinSpawns[randomIndex];
        TargetSum = spawn.TargetAmount;
        gameState = GameState.Evaluating;
        HUD.Price = TargetSum;

        foreach(var c in spawn.Spawns) {
            var objC = Instantiate(coinPrefab, c.Position, c.Rotation, transform);
            var coin = objC.GetComponent<Coin>();
            coin.Config = gameConfig.GetCoinConfig(c.CoinID);
            coin.ApplyConfig();
        }
    }

    // Update is called once per frame
    void Update() {
        switch (gameState) {
            case (GameState.Evaluating):
                if (CoinCounter == 0) {
                    Debug.Log("no coins left");
                    gameState = GameState.EvaluationComplete;
                    CheckEvaulation();
                }
                break;
            default:
                break;
        }

        HUD.Health = HealthPoints;
    }

    public void CoinAdded() {
        CoinCounter++;
    }

    public void CoinAccepted(CoinConfig config) {
        Debug.Log($"CoinAccepted {config.Amount}");
        CoinCounter--;
        CurrentSum += config.Amount;
        DummyCoinAccepted = DummyCoinAccepted || config.IsDummy;
    }

    public void CoinRejected(CoinConfig config) {
        Debug.Log($"CoinRejected {config.Amount}");
        CoinCounter--;
    }

    private void CheckEvaulation() {
        // what if we accepted a dummy coin?
        // do dummy coins have analagous value?
        if(CurrentSum == TargetSum) {
            HealthPoints = Mathf.Min(HealthPoints + SuccessReward, MaxHealthPoints);
        }
        else {
            HealthPoints = Mathf.Max(HealthPoints - FailPenalty, 0);
        }
        Debug.Log($"Health Points: {HealthPoints}");
        StartRound();

        HUD.Score += 1;
    }
}
