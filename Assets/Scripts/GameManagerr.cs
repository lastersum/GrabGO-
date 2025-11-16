using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerr : MonoBehaviour
{
    public static GameManagerr instance;

    [Header("Game State")]
    public GameState currentGameState = GameState.Menu;
    public bool isGameRunning = false;

    [Header("Player Stats")]
    public int currentScore = 0;
    public int sessionCoins = 0;
    public int totalCoins = 0;
    public int playerHealth = 3;
    public int maxHealth = 3;
    public int highScore = 0;
    public int currentLevel = 1;

    [Header("Game Settings")]
    public float gameSpeed = 1f;
    public float initialGameSpeed = 1f;
    public float speedIncreaseRate = 0.1f;
    public float speedIncreaseInterval = 10f;

    // Events
    public System.Action<int> OnScoreUpdated;
    public System.Action<int> OnSessionCoinsUpdated;
    public System.Action<int> OnHealthUpdated;
    public System.Action<int> OnGameOver;

    private bool isRestarting = false;
    private float lastSpeedIncreaseTime;
    private float gameStartTime;

    public enum GameState
    {
        Menu,
        Playing,
        Paused,
        GameOver
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeGame();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeGame()
    {
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        LoadGameData();
        gameSpeed = initialGameSpeed;

        Debug.Log("🎮 GameManager initialized!");
    }

    void LoadGameData()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
    }

    void Update()
    {
        if (currentGameState == GameState.Playing && isGameRunning)
        {
            HandleGameSpeed();
        }
    }

    void HandleGameSpeed()
    {
        if (Time.time - lastSpeedIncreaseTime >= speedIncreaseInterval)
        {
            gameSpeed += speedIncreaseRate;
            lastSpeedIncreaseTime = Time.time;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isRestarting = false;
        StartCoroutine(DelayedGameStart());
    }

    private System.Collections.IEnumerator DelayedGameStart()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        StartGame();
    }

    public void StartGame()
    {
        currentGameState = GameState.Playing;
        isGameRunning = true;
        gameStartTime = Time.time;
        lastSpeedIncreaseTime = Time.time;

        currentScore = 0;
        sessionCoins = 0;
        playerHealth = maxHealth;
        gameSpeed = initialGameSpeed;
        currentLevel = 1;

        OnScoreUpdated?.Invoke(currentScore);
        OnSessionCoinsUpdated?.Invoke(sessionCoins);
        OnHealthUpdated?.Invoke(playerHealth);
        OnSessionCoinsUpdated?.Invoke(sessionCoins);

        Debug.Log($"🎮 Oyun başladı! Session: {sessionCoins} coins");
        Debug.Log("🎮 Oyun başladı!");
    }

    public void GameOver()
    {
        if (currentGameState != GameState.Playing) return;

        currentGameState = GameState.GameOver;
        isGameRunning = false;
        Time.timeScale = 0f;

        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore);
        }

        if (sessionCoins > 0)
        {
            totalCoins += sessionCoins;
            PlayerPrefs.SetInt("TotalCoins", totalCoins);
        }

        OnGameOver?.Invoke(currentScore);

        Debug.Log("🎮 Oyun bitti! Skor: " + currentScore);
    }

    public void RestartGame()
    {
        if (isRestarting) return;
        isRestarting = true;

        Debug.Log("🔄 Restart başlatılıyor...");

        ClearAllEvents();
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void ClearAllEvents()
    {
        OnScoreUpdated = null;
        OnSessionCoinsUpdated = null;
        OnHealthUpdated = null;
        OnGameOver = null;
    }

    public void AddScore(int points)
    {
        currentScore += points;
        sessionCoins += points;

        OnScoreUpdated?.Invoke(currentScore);
        OnSessionCoinsUpdated?.Invoke(sessionCoins);
        Debug.Log($"💰 Score: {currentScore}, Session Coins: {sessionCoins}");

    }
    public void AddSessionCoins(int coins)
    {
        sessionCoins += coins;
        OnSessionCoinsUpdated?.Invoke(sessionCoins);
        Debug.Log($"🪙 Session Coins +{coins} = {sessionCoins}");
    }
    public void CollectCoin(int coinValue)
    {
        currentScore += coinValue;
        sessionCoins += coinValue;

        OnScoreUpdated?.Invoke(currentScore);
        OnSessionCoinsUpdated?.Invoke(sessionCoins); // ✅ BU SATIR KESİNLİKLE OLSUN

        Debug.Log($"🎯 Coin toplandı! Score: {currentScore}, Coins: {sessionCoins}");
    }
    public void PlayerTakeDamage(int damage)
    {
        playerHealth -= damage;
        playerHealth = Mathf.Max(0, playerHealth);

        OnHealthUpdated?.Invoke(playerHealth);

        if (playerHealth <= 0)
        {
            GameOver();
        }
    }

    public int GetTotalCoins()
    {
        return totalCoins;
    }

    public bool SpendCoins(int amount)
    {
        if (totalCoins >= amount)
        {
            totalCoins -= amount;
            PlayerPrefs.SetInt("TotalCoins", totalCoins);
            return true;
        }
        return false;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Debug
    [ContextMenu("Add 1000 Score")]
    void DebugAddScore() => AddScore(1000);

    [ContextMenu("Take Damage")]
    void DebugTakeDamage() => PlayerTakeDamage(1);
}