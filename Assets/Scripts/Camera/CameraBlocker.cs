using UnityEngine;

public static class CameraBlocker //bloquea la cámara si si se está tocando algo de la ui o moviendo algo 
{
    public static bool IsDraggingUI { get; set; } = false;
}

