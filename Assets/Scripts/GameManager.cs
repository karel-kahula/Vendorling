using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    [Range(5, 200)]
    public int TargetSum;
    [Range(0, 1)]
    public float MaxHealthPoints = 1f;
    [Range(0, 1)]
    public float HealthPoints;
    [Range(0, 1)]
    public float InitialHealth = 0.5f;
    public float HealthDrain = 0.01f;
    public float SuccessReward = 0.25f;
    public float FailPenalty = 0.15f;
    public float CameraShakeDuration = 0.5f;
    public GameConfig gameConfig;
    public Coin CoinPrefab;
    public HUDManager HUD;
    public GameState gameState;
    public CameraShaker cameraShaker;

    public enum GameState {
        Evaluating,
        EvaluationComplete,
        SuccessfulRound,
        FailedRound,
        Waiting,
        Ready,
        GameOver,
        Starting
    }

    private int CurrentSum = 0;
    private int CoinCounter = 0;
    private bool DummyCoinAccepted = false;

    // Start is called before the first frame update
    void Start() {
        Time.timeScale = 1;
        Debug.Log($"Start Gamestate {gameState}");
        SetupGame();
    }

    private void SetupGame() {
        HUD.Score = 0;
        HealthPoints = InitialHealth;
        Debug.Log("setupgame");
        StartRound();
    }

    private void StartRound() {
        var randomIndex = Random.Range(0, gameConfig.CoinSpawns.Count);
        var spawn = gameConfig.CoinSpawns[randomIndex];
        TargetSum = spawn.TargetAmount;
        CurrentSum = 0;
        HUD.Price = TargetSum;

        foreach(var c in spawn.Spawns) {
            var objC = Instantiate(CoinPrefab, c.Position, c.Rotation, transform);
            var coin = objC.GetComponent<Coin>();
            coin.Config = gameConfig.GetCoinConfig(c.CoinID);
            coin.ApplyConfig();
        }

        gameState = GameState.Starting;
    }

    // Update is called once per frame
    void Update() {
        switch (gameState) {
            case (GameState.Evaluating):
                if(HealthPoints == 0) {
                    GameOver();
                }
                else {
                    ChangeHealth(-HealthDrain * Time.deltaTime);
                    if (CoinCounter == 0) {
                        Debug.Log("no coins left");
                        CheckEvaulation();
                    }
                }
                break;
            case (GameState.Ready):
                StartRound();
                break;
            default:
                break;
        }

        HUD.Health = HealthPoints;
    }

    public void CoinAdded() {
        CoinCounter++;
        gameState = GameState.Evaluating;
        Debug.Log($"Coins registered {CoinCounter}");
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
        Debug.Log($"Current Sum: {CurrentSum}");

        if(CurrentSum == TargetSum) {
            HUD.Score += 1;
            ChangeHealth(SuccessReward);
            HUD.TriggerSuccess();
        }
        else {
            ChangeHealth(-FailPenalty);
            HUD.TriggerFailure();
        }
        cameraShaker.TriggerShake(CameraShakeDuration);
        if(HealthPoints == 0) {
            GameOver();
        }
        else {
            gameState = GameState.Ready;
        }
    }

    private void ChangeHealth(float hp) {
        HealthPoints = Mathf.Clamp01(HealthPoints + hp);
    }

    private void GameOver() {
        Debug.Log("gameover");
        gameState = GameState.GameOver;
        Time.timeScale = 0;
        HUD.ShowGameOverScreen();

        var score = PlayerPrefs.GetInt("HighScore", 0);
        if (HUD.Score > score)
            PlayerPrefs.SetInt("HighScore", HUD.Score);

    }

    public void NewGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
