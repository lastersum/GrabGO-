using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Lane Settings")]
    public float[] lanePositions = { -2f, 0f, 2f }; // Sol, Orta, Sað
    public int currentLane = 1; // 0:Sol, 1:Orta, 2:Sað

    [Header("Movement Settings")]
    public float laneChangeSpeed = 5f;
    public float forwardSpeed = 5f;

    private Vector3 targetPosition;
    private bool isMoving = false;

    [Header("Input Settings")]
    public float swipeDeadZone = 50f; // Pixel cinsinden
    private Vector2 touchStartPos;

    void Start()
    {
        // Baþlangýç pozisyonu - orta lane
        targetPosition = new Vector3(lanePositions[currentLane], transform.position.y, transform.position.z);
        transform.position = targetPosition;
    }

    void Update()
    {
        HandleInput();
        MovePlayer();
        MoveForward();
    }

    void HandleInput()
    {
        // MOUSE INPUT (PC TEST)
        if (Input.GetMouseButtonDown(0))
        {
            touchStartPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Vector2 touchEndPos = Input.mousePosition;
            ProcessSwipe(touchEndPos - touchStartPos);
        }

        // TOUCH INPUT (MOBÝL)
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

        // KLAVYE INPUT (TEST ÝÇÝN)
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            MoveLeft();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            MoveRight();
        }
    }

    void ProcessSwipe(Vector2 swipeDelta)
    {
        // SWIPE KONTROLÜ
        if (Mathf.Abs(swipeDelta.x) > swipeDeadZone)
        {
            if (swipeDelta.x > 0)
            {
                // SAÐ SWIPE
                MoveRight();
            }
            else
            {
                // SOL SWIPE
                MoveLeft();
            }
        }
        else
        {
            // TAP - ORTAYA GÝT
            MoveToMiddle();
        }
    }

    public void MoveLeft()
    {
        if (currentLane > 0 && !isMoving)
        {
            currentLane--;
            targetPosition = new Vector3(lanePositions[currentLane], transform.position.y, transform.position.z);
            isMoving = true;
            Debug.Log($"Sol lane'e geçildi: {currentLane}");
        }
    }

    public void MoveRight()
    {
        if (currentLane < lanePositions.Length - 1 && !isMoving)
        {
            currentLane++;
            targetPosition = new Vector3(lanePositions[currentLane], transform.position.y, transform.position.z);
            isMoving = true;
            Debug.Log($"Sað lane'e geçildi: {currentLane}");
        }
    }

    public void MoveToMiddle()
    {
        if (currentLane != 1 && !isMoving)
        {
            currentLane = 1;
            targetPosition = new Vector3(lanePositions[currentLane], transform.position.y, transform.position.z);
            isMoving = true;
            Debug.Log("Orta lane'e geçildi");
        }
    }

    void MovePlayer()
    {
        // LANE DEÐÝÞÝM HAREKETÝ (SADECE X EKSENÝ)
        if (isMoving)
        {
            Vector3 newPosition = new Vector3(
                Mathf.MoveTowards(transform.position.x, targetPosition.x, laneChangeSpeed * Time.deltaTime),
                transform.position.y,
                transform.position.z
            );
            transform.position = newPosition;

            // HEDEFE ULAÞTI MI?
            if (Mathf.Abs(transform.position.x - targetPosition.x) < 0.01f)
            {
                transform.position = new Vector3(targetPosition.x, transform.position.y, transform.position.z);
                isMoving = false;
            }
        }
    }

    void MoveForward()
    {
        // SADECE Z EKSENÝNDE ÝLERÝ HAREKET (X'E DOKUNMA!)
        Vector3 forwardMove = Vector3.forward * forwardSpeed * Time.deltaTime;
        transform.Translate(forwardMove);
    }

    // DEBUG VE TEST FONKSÝYONLARI
    void OnDrawGizmos()
    {
        // LANE'LERÝ GÖRSELLEÞTÝRME (EDITOR'DE)
        Gizmos.color = Color.red;
        foreach (float lanePos in lanePositions)
        {
            Vector3 laneWorldPos = new Vector3(lanePos, transform.position.y, transform.position.z + 5f);
            Gizmos.DrawWireCube(laneWorldPos, new Vector3(0.5f, 0.5f, 10f));
        }
    }
}