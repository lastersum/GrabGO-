using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerr : MonoBehaviour
{
    public static GameManagerr instance;

    [Header("Game Settings")]
    public int currentScore;
    public int totalCoins;
    public int currentLevel = 1;

    [Header("Mobile Optimization")]
    public int targetFrameRate = 60;
    public bool neverSleep = true;
    public ScreenOrientation screenOrientation = ScreenOrientation.Portrait;
    public bool enableMobileOptimization = true;

    void Awake()
    {
        // SINGLETON PATTERN - Tek instance olsun
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // MOBİL OPTİMİZASYONU BAŞLAT
            SetupMobileOptimization();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void SetupMobileOptimization()
    {
        if (!enableMobileOptimization) return;

        // PERFORMANS AYARLARI
        Application.targetFrameRate = targetFrameRate;

        // EKRAN AYARLARI
        if (neverSleep)
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        else
            Screen.sleepTimeout = SleepTimeout.SystemSetting;

        Screen.orientation = screenOrientation;

        // MOBİL CİHAZLAR İÇİN EK OPTİMİZASYON
        if (Application.isMobilePlatform)
        {
            // Kalite ayarları
            QualitySettings.SetQualityLevel(2, true); // Medium quality
            QualitySettings.vSyncCount = 0; // VSync kapalı

            // Ses optimizasyonu (opsiyonel)
            AudioListener.volume = 1.0f;
        }

        Debug.Log($"🎮 Mobile Optimizasyon Aktif: {targetFrameRate}FPS | {screenOrientation}");
        Debug.Log($"📱 Cihaz: {SystemInfo.deviceModel} | İşlemci: {SystemInfo.processorCount} core");
    }

    // OYUN YÖNETİM FONKSİYONLARI
    public void AddScore(int points)
    {
        currentScore += points;
        totalCoins += points;

        // Event tetikleme (UI güncelleme için)
        OnScoreUpdated?.Invoke(currentScore);
    }

    public void LevelComplete(int collectedCoins)
    {
        totalCoins += collectedCoins;
        currentLevel++;

        // Reklam gösterimi (her 3 seviyede bir)
        if (currentLevel % 3 == 0)
        {
            ShowInterstitialAd();
        }

        Debug.Log($"🎉 Seviye {currentLevel} tamamlandı! Toplam Para: {totalCoins}");
    }

    public void PlayerCaught()
    {
        // Oyuncu yakalandığında yapılacaklar
        Debug.Log("🚨 Oyuncu yakalandı!");

        // Ödüllü reklam teklifi
        ShowRewardedAd("second_chance");
    }

    // REKLAM FONKSİYONLARI
    public void ShowRewardedAd(string rewardType)
    {
        // Unity Ads entegrasyonu buraya gelecek
        Debug.Log($"📺 Ödüllü reklam gösteriliyor: {rewardType}");

        // Geçici test ödülü
        if (rewardType == "second_chance")
        {
            totalCoins += 50;
            Debug.Log("💰 50 jeton ödül verildi!");
        }
    }

    public void ShowInterstitialAd()
    {
        // Ara reklam buraya gelecek
        Debug.Log("🔄 Ara reklam gösteriliyor");
    }

    // EVENT SYSTEM
    public System.Action<int> OnScoreUpdated;
    public System.Action<int> OnCoinsUpdated;
    public System.Action<int> OnLevelCompleted;

    // DEBUG FONKSİYONLARI
    [ContextMenu("Debug: Add 100 Coins")]
    void DebugAddCoins()
    {
        AddScore(100);
        Debug.Log($"💰 Debug: 100 jeton eklendi. Toplam: {totalCoins}");
    }

    [ContextMenu("Debug: Complete Level")]
    void DebugCompleteLevel()
    {
        LevelComplete(150);
    }
}