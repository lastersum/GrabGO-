using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float speed = 10f;
    public float lifetime = 3f;

    void Start()
    {
        // Belirli süre sonra otomatik yok ol
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Ýleri doðru hareket et
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}