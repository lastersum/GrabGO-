using UnityEngine;
using System.Collections.Generic;

public class ImprovedItemSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject coinPrefab;
    public GameObject goldCoinPrefab;
    public float[] lanePositions = { -2f, 0f, 2f };

    [Header("Timing Settings")]
    public float initialSpawnDelay = 2f;
    public float minSpawnInterval = 0.8f;
    public float maxSpawnInterval = 2f;

    [Header("Spawn Chances")]
    [Range(0, 1)] public float goldCoinChance = 0.2f;
    public float minDistanceBetweenItems = 3f;

    private Transform player;
    private float nextSpawnTime;
    private Dictionary<int, float> lastSpawnZPerLane = new Dictionary<int, float>();
    private bool isSpawning = false; // ÇİFT SPAWN KORUMASI

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nextSpawnTime = Time.time + initialSpawnDelay;

        for (int i = 0; i < lanePositions.Length; i++)
        {
            lastSpawnZPerLane[i] = 0f;
        }
    }

    void Update()
    {
        // ÇİFT SPAWN KORUMASI
        if (Time.time >= nextSpawnTime && player != null && !isSpawning)
        {
            isSpawning = true;
            StartCoroutine(SpawnWithDelay());
        }
    }

    System.Collections.IEnumerator SpawnWithDelay()
    {
        if (TrySpawnItem())
        {
            nextSpawnTime = Time.time + Random.Range(minSpawnInterval, maxSpawnInterval);
        }
        else
        {
            nextSpawnTime = Time.time + 0.5f;
        }

        yield return new WaitForEndOfFrame(); // Frame sonuna kadar bekle
        isSpawning = false;
    }

    bool TrySpawnItem()
    {
        Debug.Log($"🔄 SPAWN DENENİYOR: {Time.time}");

        List<int> availableLanes = GetAvailableLanes();
        if (availableLanes.Count == 0) return false;

        int randomLaneIndex = Random.Range(0, availableLanes.Count);
        int selectedLane = availableLanes[randomLaneIndex];

        float spawnX = lanePositions[selectedLane];
        float spawnZ = player.position.z + Random.Range(12f, 18f);

        lastSpawnZPerLane[selectedLane] = spawnZ;

        GameObject itemToSpawn = Random.value < goldCoinChance ? goldCoinPrefab : coinPrefab;
        Instantiate(itemToSpawn, new Vector3(spawnX, 0.5f, spawnZ), Quaternion.identity);

        Debug.Log($"✅ SPAWN BAŞARILI: {itemToSpawn.name} at Lane {selectedLane}");
        return true;
    }

    List<int> GetAvailableLanes()
    {
        List<int> availableLanes = new List<int>();

        for (int i = 0; i < lanePositions.Length; i++)
        {
            float distanceSinceLastSpawn = player.position.z - lastSpawnZPerLane[i];
            if (distanceSinceLastSpawn >= minDistanceBetweenItems || lastSpawnZPerLane[i] == 0f)
            {
                availableLanes.Add(i);
            }
        }

        return availableLanes;
    }
}