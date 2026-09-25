using UnityEngine;

// ENEMIGO DESACTIVADOR
// Cada tanto apaga la torre mas cercana por unos segundos.
// No hace daño: abre huecos para que pasen los que vienen atras.
public class EnemyDisabler : MonoBehaviour
{
    [Header("Sabotaje")]
    [SerializeField] private float radioDeAlcance = 3f;
    [SerializeField] private float duracionDelApagado = 4f;
    [SerializeField] private float tiempoEntreSabotajes = 6f;

    [Header("Feedback")]
    [SerializeField] private float duracionDelDestello = 0.2f;
    [SerializeField] private Color colorDelDestello = Color.magenta;

    private float timer;
    private SpriteRenderer sr;
    private Color colorOriginal;
    private float destelloRestante;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();

        if (sr != null)
        {
            colorOriginal = sr.color;
        }
    }

    private void OnEnable()
    {
        // Arranca con el cooldown corriendo para que no sabotee
        // en el mismo instante en que aparece.
        timer = tiempoEntreSabotajes;
        destelloRestante = 0f;

        if (sr != null)
        {
            sr.color = colorOriginal;
        }
    }

    private void Update()
    {
        ActualizarDestello();

        timer -= Time.deltaTime;

        if (timer > 0f) return;

        Turret objetivo = BuscarTorreCercana();

        if (objetivo == null)
        {
            // No hay nada que apagar cerca: reintenta pronto
            // en vez de perder el cooldown entero.
            timer = 0.5f;
            return;
        }

        objetivo.Apagar(duracionDelApagado);
        Destellar();
        timer = tiempoEntreSabotajes;
    }

    private Turret BuscarTorreCercana()
    {
        Collider2D[] tocados = Physics2D.OverlapCircleAll(transform.position, radioDeAlcance);

        Turret mejor = null;
        float mejorDistancia = float.MaxValue;

        for (int i = 0; i < tocados.Length; i++)
        {
            if (!tocados[i].TryGetComponent<Turret>(out Turret torre)) continue;

            // Si ya esta apagada, no tiene sentido gastarle el sabotaje.
            if (torre.Apagada) continue;

            float distancia = Vector2.Distance(transform.position, tocados[i].transform.position);

            if (distancia < mejorDistancia)
            {
                mejorDistancia = distancia;
                mejor = torre;
            }
        }

        return mejor;
    }

    private void Destellar()
    {
        destelloRestante = duracionDelDestello;

        if (sr != null)
        {
            sr.color = colorDelDestello;
        }
    }

    private void ActualizarDestello()
    {
        if (destelloRestante <= 0f) return;

        destelloRestante -= Time.deltaTime;

        if (destelloRestante <= 0f && sr != null)
        {
            sr.color = colorOriginal;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0f, 1f, 0.4f);
        Gizmos.DrawWireSphere(transform.position, radioDeAlcance);
    }
}
