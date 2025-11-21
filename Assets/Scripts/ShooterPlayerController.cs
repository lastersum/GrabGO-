using UnityEngine;

public class ShooterPlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float[] lanePositions = { -2f, 0f, 2f };
    public int currentLane = 1;
    public float laneChangeSpeed = 5f;

    [Header("Shooting Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;
    public bool autoFire = true;

    [Header("Game Settings")]
    public float enemyKillDistance = 1.5f; // Düşman bu mesafeye gelirse ölürüz

    private Vector3 targetPosition;
    private bool isMoving = false;
    private float nextFireTime;
    private float swipeDeadZone = 50f;
    private Vector2 touchStartPos;

    void Start()
    {
        targetPosition = new Vector3(lanePositions[currentLane], transform.position.y, transform.position.z);
        transform.position = targetPosition;
    }

    void Update()
    {
        HandleInput();
        MovePlayer();
        HandleShooting();
        CheckEnemyDistance();
    }

    void HandleInput()
    {
        // MOBILE INPUT
        if (Input.GetMouseButtonDown(0))
        {
            touchStartPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Vector2 touchEndPos = Input.mousePosition;
            ProcessSwipe(touchEndPos - touchStartPos);
        }

        // TOUCH INPUT
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                Vector2 touchEndPos = touch.position;
                ProcessSwipe(touchEndPos - touchStartPos);
            }
        }

        // KEYBOARD INPUT (TEST)
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            MoveLeft();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            MoveRight();
        }

        // MANUAL FIRE (TEST)
        if (!autoFire && Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    void ProcessSwipe(Vector2 swipeDelta)
    {
        if (Mathf.Abs(swipeDelta.x) > swipeDeadZone)
        {
            if (swipeDelta.x > 0) MoveRight();
            else MoveLeft();
        }
    }

    public void MoveLeft()
    {
        if (currentLane > 0 && !isMoving)
        {
            currentLane--;
            targetPosition = new Vector3(lanePositions[currentLane], transform.position.y, transform.position.z);
            isMoving = true;
        }
    }

    public void MoveRight()
    {
        if (currentLane < lanePositions.Length - 1 && !isMoving)
        {
            currentLane++;
            targetPosition = new Vector3(lanePositions[currentLane], transform.position.y, transform.position.z);
            isMoving = true;
        }
    }

    void MovePlayer()
    {
        if (isMoving)
        {
            Vector3 newPosition = new Vector3(
                Mathf.MoveTowards(transform.position.x, targetPosition.x, laneChangeSpeed * Time.deltaTime),
                transform.position.y,
                transform.position.z
            );
            transform.position = newPosition;

            if (Mathf.Abs(transform.position.x - targetPosition.x) < 0.01f)
            {
                transform.position = new Vector3(targetPosition.x, transform.position.y, transform.position.z);
                isMoving = false;
            }
        }
    }

    void HandleShooting()
    {
        if (autoFire && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }

    void CheckEnemyDistance()
    {
        // Tüm düşmanları kontrol et
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < enemyKillDistance)
                {
                    // DÜŞMAN ÇOK YAKIN - ÖL!
                    GameManagerr.instance.GameOver();
                    Debug.Log("💀 Düşman çok yakına geldi! Game Over!");
                    return;
                }
            }
        }
    }
}