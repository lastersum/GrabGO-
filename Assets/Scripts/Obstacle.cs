using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public int damage = 1;
    public bool isDeadly = true;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isDeadly)
            {
                // OYUNCU ÖLDÜ
                GameManagerr.instance.GameOver();
            }
            else
            {
                // HASAR ALDI AMA ÖLMEDİ
                GameManagerr.instance.PlayerTakeDamage(damage);
            }

            // Efekt oynat ve yok et
            PlayCollisionEffect();
            Destroy(gameObject);
        }
    }

    void PlayCollisionEffect()
    {
        // Çarpışma efekti burada
        Debug.Log("💥 Engese çarpıldı!");
    }
}