using UnityEngine;

// ENEMIGO A DISTANCIA
// Avanza hasta quedar a tiro de la puerta y desde ahi la castiga.
// Rompe la suposicion de que conviene amontonar todas las torres
// alrededor de la puerta.
[RequireComponent(typeof(EnemyWalker))]
public class EnemyRanged : MonoBehaviour
{
    [Header("Comportamiento")]
    [Tooltip("A que distancia de la puerta se planta, en unidades del mundo.")]
    [SerializeField] private float rangoDeAtaque = 6f;

    [SerializeField] private float tiempoEntreDisparos = 2f;
    [SerializeField] private int dañoPorDisparo = 1;

    private EnemyWalker walker;
    private float timer;
    private bool plantado;

    private void Awake()
    {
        walker = GetComponent<EnemyWalker>();
    }

    private void OnEnable()
    {
        plantado = false;
        timer = tiempoEntreDisparos;
    }

    private void Update()
    {
        if (!plantado)
        {
            RevisarSiLlego();
            return;
        }

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            timer = tiempoEntreDisparos;

            // No conoce a la puerta: avisa que la golpeo y quien corresponda escucha.
            EventManager.TriggerEvent<int>(GameEvents.DoorHit, dañoPorDisparo);
        }
    }

    private void RevisarSiLlego()
    {
        if (!TryGetPosicionDeLaPuerta(out Vector3 puerta)) return;

        if (Vector2.Distance(transform.position, puerta) > rangoDeAtaque) return;

        plantado = true;
        walker.Detenido = true;
    }

    // El ultimo waypoint del camino esta pegado a la puerta,
    // asi que sirve de referencia sin tener que conocer al objeto Door.
    private bool TryGetPosicionDeLaPuerta(out Vector3 posicion)
    {
        posicion = Vector3.zero;

        if (walker == null || walker.Camino == null) return false;

        int cantidad = walker.Camino.wayPointCount;

        if (cantidad == 0) return false;

        Transform ultimo = walker.Camino.GetWayPoint(cantidad - 1);

        if (ultimo == null) return false;

        posicion = ultimo.position;
        return true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.8f, 0f, 0.5f);
        Gizmos.DrawWireSphere(transform.position, rangoDeAtaque);
    }
}