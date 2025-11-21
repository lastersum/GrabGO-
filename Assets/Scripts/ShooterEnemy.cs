using UnityEngine;

public class ShooterEnemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float moveSpeed = 3f;
    public int health = 10;
    public int scoreValue = 10;

    [Header("Lane Settings")]
    public int currentLane = 1; // 0:Sol, 1:Orta, 2:Sağ
    private float[] lanePositions = { -2f, 0f, 2f };

    void Start()
    {
        // Rastgele lane seç
        currentLane = Random.Range(0, 3);
        float spawnX = lanePositions[currentLane];
        transform.position = new Vector3(spawnX, transform.position.y, transform.position.z);
    }

    void Update()
    {
        // Karaktere doğru ilerle
        transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);

        // Ekran dışına çıkınca yok et
        if (transform.position.z < -5f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(1);
            Destroy(other.gameObject); // Mermiyi yok et
        }
    }

    void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Skor ekle
        if (GameManagerr.instance != null)
        {
            GameManagerr.instance.AddScore(scoreValue);
        }

        // Efekt oynat (sonra ekleriz)
        Destroy(gameObject);

        Debug.Log("🎯 Düşman vuruldu! +" + scoreValue + " puan");
    }
}