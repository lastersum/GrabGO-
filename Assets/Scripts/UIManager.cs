using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Text scoreText;
    public Text sessionCoinsText; // ✅ BU SATIRI EKLE
    public Text healthText;
    public Slider healthBar;

    [Header("Screens")]
    public GameObject gameOverScreen;
    public GameObject inGameUI;
    public Button restartButton;

    private bool canRestart = true;

    void Start()
    {
        StartCoroutine(ConnectToGameManager());
        gameOverScreen.SetActive(false);
        if (restartButton != null)
        {
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(RestartGame);
        }
    }

    private System.Collections.IEnumerator ConnectToGameManager()
    {
        yield return new WaitUntil(() => GameManagerr.instance != null);

        GameManagerr.instance.OnScoreUpdated += UpdateScore;
        GameManagerr.instance.OnSessionCoinsUpdated += UpdateSessionCoins; // ✅ BU SATIRI EKLE
        GameManagerr.instance.OnHealthUpdated += UpdateHealth;
        GameManagerr.instance.OnGameOver += ShowGameOver;

        UpdateScore(GameManagerr.instance.currentScore);
        UpdateSessionCoins(GameManagerr.instance.sessionCoins); // ✅ BU SATIRI EKLE
        UpdateHealth(GameManagerr.instance.playerHealth);

        Debug.Log("🔗 UI Manager bağlandı!");
    }

    void UpdateScore(int score)
    {
        if (scoreText != null)
            scoreText.text = $"SKOR: {score}";
    }

    void UpdateSessionCoins(int coins) // ✅ BU FONKSİYONU EKLE
    {
        if (sessionCoinsText != null)
        {
            sessionCoinsText.text = $"COINS: {coins}";

            // ✅ TOTAL COINS'I DE LOG'DA GÖSTER
            if (GameManagerr.instance != null)
            {
                Debug.Log($"🔄 UI Güncellendi: Session={coins}, Total={GameManagerr.instance.GetTotalCoins()}");
            }
        }
    }
    void UpdateHealth(int health)
    {
        if (healthText != null)
            healthText.text = $"CAN: {health}";

        if (healthBar != null)
        {
            healthBar.value = health;
            healthBar.maxValue = GameManagerr.instance.maxHealth;
        }
    }

    void ShowGameOver(int finalScore)
    {
        if (inGameUI != null)
            inGameUI.SetActive(false);

        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);

            if (restartButton != null)
                restartButton.interactable = true;
        }
    }

    public void RestartGame()
    {
        if (!canRestart || GameManagerr.instance == null) return;

        canRestart = false;
        Debug.Log("🔄 Restart butonuna tıklandı");

        if (restartButton != null)
            restartButton.interactable = false;

        GameManagerr.instance.RestartGame();
        StartCoroutine(EnableRestartAfterDelay());
    }

    private System.Collections.IEnumerator EnableRestartAfterDelay()
    {
        yield return new WaitForSecondsRealtime(3f);
        canRestart = true;
    }

    void OnDestroy()
    {
        if (GameManagerr.instance != null)
        {
            GameManagerr.instance.OnScoreUpdated -= UpdateScore;
            GameManagerr.instance.OnSessionCoinsUpdated -= UpdateSessionCoins; // ✅ BU SATIRI EKLE
            GameManagerr.instance.OnHealthUpdated -= UpdateHealth;
            GameManagerr.instance.OnGameOver -= ShowGameOver;
        }
    }

    // ✅ DEBUG BUTONLARI
    void Update()
    {
        // C tuşu ile manuel coin ekle (test için)
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (GameManagerr.instance != null)
            {
                GameManagerr.instance.CollectCoin(10);
                Debug.Log("🔧 Manuel coin eklendi: +10");
            }
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (GameManagerr.instance != null)
            {
                Debug.Log($"📊 COINS DETAY: Session={GameManagerr.instance.sessionCoins}, Total={GameManagerr.instance.GetTotalCoins()}, HighScore={GameManagerr.instance.highScore}");
            }
        }
    }
}