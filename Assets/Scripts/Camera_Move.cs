using UnityEngine;

public class Camera_move : MonoBehaviour
{
    [Header("========== LIMITS ==========")]
    public float minX = -10f;
    public float maxX = 10f;
    public float minY = -5f;
    public float maxY = 10f;

    [Header("========== DRAG SETTINGS ==========")]
    public float dragSpeed = 0.05f;

    private Vector3 dragOrigin;
    private bool isDragging;

    private float fixedZ;

    void Start()
    {
        fixedZ = transform.position.z;
    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouseDrag();
#elif UNITY_ANDROID || UNITY_IOS
        HandleTouchDrag();
#endif
    }

    void HandleMouseDrag()
    {
        if (Time.timeScale == 0) return; // Ngăn di chuyển khi game pause

        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            isDragging = true;
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            Vector3 difference = Input.mousePosition - dragOrigin;

            float deltaX = -difference.x * dragSpeed;
            float deltaY = -difference.y * dragSpeed;

            Vector3 newPosition = transform.position + new Vector3(deltaX, deltaY, 0);

            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
            newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
            newPosition.z = fixedZ;

            transform.position = newPosition;

            dragOrigin = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }

    void HandleTouchDrag()
    {
        if (Time.timeScale == 0) return; // Ngăn di chuyển khi game pause

        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                dragOrigin = touch.position;
                isDragging = true;
            }

            if (touch.phase == TouchPhase.Moved && isDragging)
            {
                Vector3 difference = (Vector3)touch.position - dragOrigin;

                float deltaX = -difference.x * dragSpeed;
                float deltaY = -difference.y * dragSpeed;

                Vector3 newPosition = transform.position + new Vector3(deltaX, deltaY, 0);

                newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
                newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
                newPosition.z = fixedZ;

                transform.position = newPosition;

                dragOrigin = touch.position;
            }

            if (touch.phase == TouchPhase.Ended)
            {
                isDragging = false;
            }
        }
    }
}
