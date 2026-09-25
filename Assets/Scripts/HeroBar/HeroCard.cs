using UnityEngine;
using UnityEngine.EventSystems;

public class HeroCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform originalParent;
    int originalIndex;
    Canvas canvas;
    HeroSwitcher hero;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        hero = GetComponent<HeroSwitcher>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // BLOQUEAMOS LA CÁMARA MIENTRAS ARRASTRAMOS UN ELEMENTO
        CameraBlocker.IsDraggingUI = true;

        originalParent = transform.parent;
        originalIndex = transform.GetSiblingIndex();
        transform.SetParent(canvas.transform);
    }

    public void OnDrag(PointerEventData eventData) 
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData) 
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        Collider2D hit = Physics2D.OverlapPoint(worldPos);

        if (hit != null)
        {
            Turret turret = hit.GetComponent<Turret>();

            if (turret != null) { turret.SetHero(hero); }
        }

        transform.SetParent(originalParent);
        transform.SetSiblingIndex(originalIndex);

        // DESBLOQUEAMOS LA CÁMARA AL SOLTAR 
        CameraBlocker.IsDraggingUI = false;
    }
}