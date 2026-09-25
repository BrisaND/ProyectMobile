using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ZoomSystem : MonoBehaviour
{
    [Header("Límites de Zoom")]
    [SerializeField] private float minZoom = 3f;           // Límite Zoom In
    [SerializeField] private float maxZoomTarget = 8f;     // Límite Zoom Out (donde se acomoda la cámara)
    [SerializeField] private float maxZoomThreshold = 10f; // Over-zoom mientras estirás los dedos

    [Header("Sensibilidad")]
    [SerializeField] private float zoomSpeedMobile = 0.05f;
    [SerializeField] private float zoomSpeedMouse = 5f;

    [Header("Suavizado")]
    [SerializeField] private float smoothness = 10f;
    [SerializeField] private float bounceSmoothness = 6f;

    private Camera cam;
    private float targetZoom;
    private bool isTouching = false;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        targetZoom = GetCurrentZoom();
    }

    private void Update()
    {
        HandleTouchInput();
        ApplyZoom();
    }

    private void HandleTouchInput()
    {
        
        if (Input.touchCount == 2)
        {
            isTouching = true;

            Touch firstTouch = Input.GetTouch(0);
            Touch secondTouch = Input.GetTouch(1);

            
            Vector2 firstTouchLastPos = firstTouch.position - firstTouch.deltaPosition;
            Vector2 secondTouchLastPos = secondTouch.position - secondTouch.deltaPosition;

            Vector2 initialTouchPos = firstTouch.position - secondTouch.position;
            Vector2 finalTouchPos = firstTouchLastPos - secondTouchLastPos;

            float zoom = initialTouchPos.magnitude - finalTouchPos.magnitude;
            

            if (zoom != 0)
            {
                // Restamos para ajustar la visión y aplicamos el clamp de límites 
                targetZoom -= zoom * zoomSpeedMobile;
                targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoomThreshold);
            }
        }
        // --- CONTROL DE MOUSE (PC / EDITOR) Y REBOTE AL SOLTAR ---
        else
        {
            if (isTouching)
            {
                isTouching = false;
                CheckBounce();
            }

            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scrollInput) > 0.01f)
            {
                targetZoom -= scrollInput * zoomSpeedMouse;
                targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoomThreshold);
            }
            else if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
            {
                CheckBounce();
            }
        }
    }

    private void CheckBounce()
    {
        
        if (targetZoom > maxZoomTarget)
        {
            targetZoom = maxZoomTarget;
            EventManager.TriggerEvent("OnCameraMaxZoomBounced");
        }
    }

    private void ApplyZoom()
    {
        float currentZoom = GetCurrentZoom();
        float currentSmoothness = (targetZoom == maxZoomTarget && currentZoom > maxZoomTarget) ? bounceSmoothness : smoothness;

        if (cam.orthographic)
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * currentSmoothness);
        }
        else
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetZoom, Time.deltaTime * currentSmoothness);
        }
    }

    private float GetCurrentZoom()
    {
        return cam.orthographic ? cam.orthographicSize : cam.fieldOfView;
    }
}