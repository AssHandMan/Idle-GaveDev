/*using UnityEngine;

public class SwipeCameraController : MonoBehaviour
{
    [SerializeField] private float dragSpeed = 0.1f; // Скорость перемещения
    [SerializeField] private float minX, maxX, minY, maxY; // Границы перемещения (опционально)

    private Vector3 dragOrigin; // Точка начала свайпа
    private bool isDragging = false;

    void Update()
    {
        // Для ПК (мышь) и мобильных устройств (тач)
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            isDragging = true;
            return;
        }

        if (!Input.GetMouseButton(0))
        {
            isDragging = false;
            return;
        }

        if (isDragging)
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
            Vector3 move = new Vector3(-pos.x * dragSpeed, -pos.y * dragSpeed, 0);

            transform.Translate(move, Space.World);

            // Ограничение перемещения (если нужно)
            float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
            float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);
            transform.position = new Vector3(clampedX, clampedY, transform.position.z);
        }
    }
}*/

using UnityEngine;

public class SwipeCameraInertia : MonoBehaviour
{
    [SerializeField] private float dragSpeed = 0.1f;
    [SerializeField] private float inertiaDecay = 0.95f; // Замедление инерции
    [SerializeField] private float minX, maxX, minY, maxY;

    private Vector3 dragOrigin;
    private Vector3 velocity;
    private bool isDragging;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            isDragging = true;
            velocity = Vector3.zero;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
            velocity = new Vector3(-pos.x * dragSpeed, -pos.y * dragSpeed, 0);
            transform.Translate(velocity, Space.World);
        }
        else
        {
            // Инерция
            if (velocity.magnitude > 0.01f)
            {
                transform.Translate(velocity, Space.World);
                velocity *= inertiaDecay;
            }
            else
            {
                velocity = Vector3.zero;
            }
        }

        // Ограничение позиции
        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}