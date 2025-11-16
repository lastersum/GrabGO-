using UnityEngine;

public class CollactableItem : MonoBehaviour
{
    [Header("Item Settings")]
    public int coinValue = 1;
    public bool isGoldCoin = false;
    public float rotationSpeed = 100f;

    [Header("Effects")]
    public GameObject collectEffect;
    public AudioClip collectSound;

    void Update()
    {
        // Eşyayı döndür (görsel efekt)
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }

    void Collect()
    {
        // ✅ YENİ SİSTEM - COLLECTCOIN FONKSİYONUNU ÇAĞIR
        if (GameManagerr.instance != null)
        {
            int finalValue = coinValue;

            if (isGoldCoin)
            {
                finalValue = coinValue * 3; // Gold coin 3 katı değer
                Debug.Log($"🎉 Altın coin toplandı! +{finalValue}");
            }
            else
            {
                Debug.Log($"💰 Normal coin toplandı! +{finalValue}");
            }

            GameManagerr.instance.CollectCoin(finalValue);
        }

        // Efekt ve yok etme
        Destroy(gameObject);
    

        // EFEKT OLUŞTUR
        if (collectEffect != null)
        {
            Instantiate(collectEffect, transform.position, Quaternion.identity);
        }

        // SES ÇAL
        if (collectSound != null && Camera.main != null)
        {
            AudioSource.PlayClipAtPoint(collectSound, Camera.main.transform.position);
        }

        // YOK ET
        Destroy(gameObject);
    }
}