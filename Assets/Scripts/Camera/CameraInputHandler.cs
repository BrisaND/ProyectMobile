using System;
using UnityEngine;

public class CameraInputHandler : MonoBehaviour
{
    [Header("Sensibilidad")]
    [SerializeField] private float zoomSpeedMobile = 0.05f;
    [SerializeField] private float zoomSpeedMouse = 5f;

    public event Action<Vector2> OnPanDelta;
    public event Action<float> OnZoomDelta;
    public event Action OnInputReleased;

    private bool isTouching = false;
    private Vector3 lastMousePosition;

    private void Update()
    {
        // tactil
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                // disparamos el desplazamiento del dedo
                OnPanDelta?.Invoke(touch.deltaPosition);
            }

            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                OnInputReleased?.Invoke();
            }
        }
        else if (Input.touchCount == 2)
        {
            isTouching = true;

            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            OnZoomDelta?.Invoke(difference * zoomSpeedMobile);
        }
        // control pc 
        else
        {
            if (isTouching && Input.touchCount == 0)
            {
                isTouching = false;
                OnInputReleased?.Invoke();
            }

            // Click derecho o izq para arrastrar en PC
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                lastMousePosition = Input.mousePosition;
            }
            else if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {
                Vector3 delta = Input.mousePosition - lastMousePosition;
                OnPanDelta?.Invoke(delta);
                lastMousePosition = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
            {
                OnInputReleased?.Invoke();
            }

            // Zoom rueda del mouse
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scrollInput) > 0.01f)
            {
                OnZoomDelta?.Invoke(scrollInput * zoomSpeedMouse);
            }
        }
    }
}