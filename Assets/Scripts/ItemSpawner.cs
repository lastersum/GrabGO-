using UnityEngine;
using System.Collections.Generic;

public class ItemSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject coinPrefab;
    public GameObject goldCoinPrefab;
    public float[] lanePositions = { -2f, 0f, 2f };

    [Header("Timing Settings")]
    public float spawnInterval = 1.5f;

    [Header("Spawn Chances")]
    [Range(0, 1)] public float goldCoinChance = 0.2f;

    private Transform player;
    private float nextSpawnTime;
    private Queue<int> laneQueue = new Queue<int>();

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nextSpawnTime = Time.time + 2f;

        // Lane sırasını başlat
        InitializeLaneQueue();
    }

    void InitializeLaneQueue()
    {
        List<int> lanes = new List<int> { 0, 1, 2 };

        // Lane'leri karıştır
        while (lanes.Count > 0)
        {
            int randomIndex = Random.Range(0, lanes.Count);
            laneQueue.Enqueue(lanes[randomIndex]);
            lanes.RemoveAt(randomIndex);
        }
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime && player != null)
        {
            SpawnItem();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnItem()
    {
        // Sıradaki lane'i al
        if (laneQueue.Count == 0)
        {
            InitializeLaneQueue();
        }

        int laneIndex = laneQueue.Dequeue();
        float spawnX = lanePositions[laneIndex];

        // Player'ın 15 birim ilerisine spawn et
        float spawnZ = player.position.z + 25f;

        // Gold veya normal coin seç
        bool isGoldCoin = Random.value < goldCoinChance;
        GameObject itemToSpawn = isGoldCoin ? goldCoinPrefab : coinPrefab;

        // Spawn et
        Instantiate(itemToSpawn, new Vector3(spawnX, 0.5f, spawnZ), Quaternion.identity);

        Debug.Log($"✅ {(isGoldCoin ? "GOLD" : "COIN")} spawned at Lane {laneIndex}");
    }
}