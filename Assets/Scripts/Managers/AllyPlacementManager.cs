using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class AllyPlacementManager : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private GameObject turretPrefab;
    [SerializeField] private Tilemap zonasPermitidasTilemap;
    [SerializeField] private BulletPool bulletPool;
    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.currentState != GameManager.GameState.Preparacion)
        {
            return;
        }

        Vector3 inputPosition = Vector3.zero;
        bool inputDetectado = false;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(touch.fingerId)) return;
                inputPosition = touch.position;
                inputDetectado = true;
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;
            inputPosition = Input.mousePosition;
            inputDetectado = true;
        }

        if (inputDetectado)
        {

            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(inputPosition);
            worldPosition.z = 0f;

            if (zonasPermitidasTilemap == null)
            {
                return;
            }

            Vector3Int cellPosition = zonasPermitidasTilemap.WorldToCell(worldPosition);

            if (zonasPermitidasTilemap.HasTile(cellPosition))
            {
                Vector3 posicionCentrada = zonasPermitidasTilemap.GetCellCenterWorld(cellPosition);
                ColocarAliado(posicionCentrada);
            }
            else
            {
                Debug.Log("clic fuera de un tile permitido.");
            }
        }
    }

    private void ColocarAliado(Vector3 posicion)
    {
        Collider2D objetoCercano = Physics2D.OverlapCircle(posicion, 0.1f);
        if (objetoCercano != null && objetoCercano.CompareTag("Tower"))
        {
            Debug.Log("torre ya existente en este casillero.");
            return;
        }

        GameObject torreta = Instantiate(turretPrefab, posicion, Quaternion.identity);
        torreta.GetComponent<Turret>().SetBulletPool(bulletPool);
    }
}