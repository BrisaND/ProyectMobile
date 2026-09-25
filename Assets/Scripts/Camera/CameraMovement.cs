using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Camera))]
public class CameraMovement : MonoBehaviour
{
    [Header("Sensibilidad y Suavizado")]
    [SerializeField] private float panSpeed = 1f;
    [SerializeField] private float smoothness = 10f;

    [Header("Límites del Mapa (World Bounds)")]
    [SerializeField] private Vector2 minBounds = new Vector2(-15f, -10f);
    [SerializeField] private Vector2 maxBounds = new Vector2(15f, 10f);

    private Camera cam;
    private Vector3 targetPosition;
    private Vector3 lastMousePosition;
    
    // Bandera para bloquear la cámara si el gesto empezó sobre UI o sobre un aliado
    private bool isDraggingCamera = false;
    private bool touchStartedOnUI = false;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        targetPosition = transform.position;
    }

    private void Update()
    {
        HandlePanInput();
        ApplyMovement();
    }

    private void HandlePanInput()
{
    // Si hay un elemento de UI en arrastre, NO movemos la cámara
    if (CameraBlocker.IsDraggingUI)
    {
        isDraggingCamera = false;
        return;
    }

    // CONTROL CELu
    if (Input.touchCount == 1)
    {
        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            touchStartedOnUI = IsPointerOverUIObject();
            isDraggingCamera = !touchStartedOnUI;
        }

        if (touch.phase == TouchPhase.Moved && isDraggingCamera)
        {
            Vector3 move = cam.ScreenToViewportPoint(touch.deltaPosition);
            Vector3 translation = new Vector3(-move.x * panSpeed * cam.orthographicSize, -move.y * panSpeed * cam.orthographicSize, 0f);

            targetPosition += translation;
            ClampTargetPosition();
        }

        if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
        {
            touchStartedOnUI = false;
            isDraggingCamera = false;
        }
    }
    // CONTROL DE PC
    else
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            touchStartedOnUI = IsPointerOverUIObject();
            if (!touchStartedOnUI)
            {
                isDraggingCamera = true;
                lastMousePosition = Input.mousePosition;
            }
        }
        else if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            if (isDraggingCamera)
            {
                Vector3 delta = Input.mousePosition - lastMousePosition;
                Vector3 move = cam.ScreenToViewportPoint(delta);

                Vector3 translation = new Vector3(-move.x * panSpeed * cam.orthographicSize, -move.y * panSpeed * cam.orthographicSize, 0f);

                targetPosition += translation;
                ClampTargetPosition();

                lastMousePosition = Input.mousePosition;
            }
        }

        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            isDraggingCamera = false;
            touchStartedOnUI = false;
        }
    }
}
    private bool IsPointerOverUIObject()
    {
        if (EventSystem.current == null) return false;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            return EventSystem.current.IsPointerOverGameObject(touch.fingerId);
        }

        return EventSystem.current.IsPointerOverGameObject();
    }

    private void ClampTargetPosition()
    {
        targetPosition.x = Mathf.Clamp(targetPosition.x, minBounds.x, maxBounds.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minBounds.y, maxBounds.y);
    }

    private void ApplyMovement()
    {
        Vector3 newPos = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothness);
        transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 center = new Vector3((minBounds.x + maxBounds.x) / 2f, (minBounds.y + maxBounds.y) / 2f, 0f);
        Vector3 size = new Vector3(maxBounds.x - minBounds.x, maxBounds.y - minBounds.y, 1f);
        Gizmos.DrawWireCube(center, size);
    }
}