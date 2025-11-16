using UnityEngine;
using System.Collections.Generic;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] obstaclePrefabs;
    public float[] lanePositions = { -2f, 0f, 2f };
    public float spawnInterval = 3f;
    public float spawnDistance = 50f;

    [Header("Spawn Chance")]
    [Range(0, 1)] public float obstacleChance = 0.7f;

    private Transform player;
    private float nextSpawnTime;
    private int lastSpawnedLane = -1;

    void Start()
    {
        StartCoroutine(WaitForGameStart());
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nextSpawnTime = Time.time + 2f;
    }
    private System.Collections.IEnumerator WaitForGameStart()
    {
        yield return new WaitUntil(() =>
            GameManagerr.instance != null &&
            GameManagerr.instance.currentGameState == GameManagerr.GameState.Playing
        );

        // Spawn sistemini başlat
        InitializeSpawner();
    }
    void InitializeSpawner()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nextSpawnTime = Time.time + 2f;
        Debug.Log("🚧 ObstacleSpawner başlatıldı!");
    }
    void Update()
    {
        if (Time.time >= nextSpawnTime && player != null)
        {
            TrySpawnObstacle();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void TrySpawnObstacle()
    {
        // Engel şansı kontrolü
        if (Random.value > obstacleChance)
        {
            Debug.Log("🎲 Engel spawn şansı tutmadı");
            return;
        }

        // AYNI LANE'DE ARKA ARKAYA SPAWN OLMASIN
        int laneIndex = GetRandomLane();
        float spawnX = lanePositions[laneIndex];
        float spawnZ = player.position.z + spawnDistance;

        Vector3 spawnPosition = new Vector3(spawnX, 0.5f, spawnZ);

        GameObject randomObstacle = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
        Instantiate(randomObstacle, spawnPosition, Quaternion.identity);

        lastSpawnedLane = laneIndex;
        Debug.Log($"🚧 TEK Engel spawn: Lane {laneIndex}");
    }

    int GetRandomLane()
    {
        int laneIndex;

        // Aynı lane'de arka arkaya spawn olmasın
        do
        {
            laneIndex = Random.Range(0, lanePositions.Length);
        }
        while (laneIndex == lastSpawnedLane && lanePositions.Length > 1);

        return laneIndex;
    }

    // DEBUG - SADECE 1 LANE GÖSTER
    void OnDrawGizmos()
    {
        if (player == null) return;

        Gizmos.color = Color.red;
        int debugLane = (lastSpawnedLane == -1) ? 1 : lastSpawnedLane;
        float spawnX = lanePositions[debugLane];
        float spawnZ = player.position.z + spawnDistance;

        Vector3 spawnPos = new Vector3(spawnX, 0.5f, spawnZ);
        Gizmos.DrawWireCube(spawnPos, new Vector3(1f, 1f, 1f));
        Gizmos.DrawLine(spawnPos, spawnPos + Vector3.down * 2f);
    }
}