using UnityEngine;

public class SwipePan3DWithInertia : MonoBehaviour
{
    private Vector3 touchStartPos;
    private bool isDragging = false;
    public float dragSpeed = 0.01f;

    [Header("Inertia Settings")]
    public float deceleration = 0.95f; // Коэффициент замедления (0.9-0.99)
    private Vector3 currentVelocity; // Текущая скорость движения

    [Header("Boundaries X and Z")]
    public Vector2 minBounds; // Минимальные X и Z координаты
    public Vector2 maxBounds; // Максимальные X и Z координаты

    void Update()
    {
        HandleInput();
        //ApplyInertia();
    }

    void HandleInput()
    {
        // Для мыши
        if (Input.GetMouseButtonDown(0))
        {
            touchStartPos = GetWorldPoint(Input.mousePosition);
            isDragging = true;
            currentVelocity = Vector3.zero; // Сброс скорости при новом касании
        }
        else if (Input.GetMouseButton(0) && isDragging)
        {
            Vector3 currentPos = GetWorldPoint(Input.mousePosition);
            Vector3 delta = currentPos - touchStartPos;
            currentVelocity = -delta * dragSpeed; // Обновляем скорость
            MoveCamera(currentVelocity);
            touchStartPos = currentPos;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        // Для тач-экранов
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = GetWorldPoint(touch.position);
                isDragging = true;
                currentVelocity = Vector3.zero;
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                Vector3 currentPos = GetWorldPoint(touch.position);
                Vector3 delta = currentPos - touchStartPos;
                currentVelocity = -delta * dragSpeed;
                MoveCamera(currentVelocity);
                touchStartPos = currentPos;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                isDragging = false;
            }
        }
    }

    void ApplyInertia()
    {
        if (!isDragging && currentVelocity.magnitude > 0.01f)
        {
            MoveCamera(currentVelocity);
            currentVelocity *= deceleration; // Постепенно замедляем
        }
        else if (!isDragging)
        {
            currentVelocity = Vector3.zero; // Полная остановка
        }
    }

    Vector3 GetWorldPoint(Vector2 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float distance;
        groundPlane.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }

    void MoveCamera(Vector3 delta)
    {
        Vector3 newPosition = transform.position + new Vector3(delta.x, 0, delta.z);

        // Ограничиваем по X и Z 
        newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
        newPosition.z = Mathf.Clamp(newPosition.z, minBounds.y, maxBounds.y);

        transform.position = newPosition;
    }
}