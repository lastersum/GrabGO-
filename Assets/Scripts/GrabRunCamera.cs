using UnityEngine;

public class GrabRunCamera : MonoBehaviour
{
    public Transform player;

    // DENEMEK ÝÇÝN BU DEÐERLERLE BAÞLA:
    public Vector3 offset = new Vector3(0, 6, -7   );  // DAHA YAKIN
    public float cameraAngle = 38f;                                   // public Vector3 offset = new Vector3(0, 6, -4);  // DAHA DA YAKIN
                                                                      // public Vector3 offset = new Vector3(0, 10, -7); // ORTA SEVÝYE

    public float smoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        Vector3 targetPosition = player.position + offset;
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref velocity,
            smoothTime
        );

        transform.LookAt(player.position);
        transform.rotation = Quaternion.Euler(cameraAngle, 0, 0);
    }

    // DEBUG ÝÇÝN - OYUN ÝÇÝNDEN AYAR YAPABÝLMEK
    void Update()
    {
       
    }
}