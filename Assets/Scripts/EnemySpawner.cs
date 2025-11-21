using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject enemyPrefab;
    public float spawnInterval = 2f;
    public float spawnDistance = 15f;

    [Header("Difficulty Settings")]
    public float minSpawnInterval = 0.8f;
    public float difficultyIncreaseRate = 0.1f;

    private Transform player;
    private float nextSpawnTime;
    private float currentSpawnInterval;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentSpawnInterval = spawnInterval;
        nextSpawnTime = Time.time + 2f;
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime && player != null)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + currentSpawnInterval;

            // Zorluk artışı
            currentSpawnInterval = Mathf.Max(minSpawnInterval, currentSpawnInterval - difficultyIncreaseRate * 0.1f);
        }
    }

    void SpawnEnemy()
    {
        Vector3 spawnPosition = new Vector3(0, 0.5f, player.position.z + spawnDistance);
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        Debug.Log("👾 Düşman spawn oldu!");
    }
}