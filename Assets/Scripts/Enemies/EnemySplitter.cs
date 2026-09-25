using UnityEngine;

// ENEMIGO DIVISOR
// Al morir se parte en varias crias que siguen por el mismo camino
// desde donde cayo el padre. Castiga matarlo cerca de la puerta.
[RequireComponent(typeof(EnemyWalker))]
public class EnemySplitter : MonoBehaviour
{
    [Header("Comportamiento")]
    [SerializeField] private EnemyId tipoDeCria = EnemyId.Caminante;
    [SerializeField] private int cantidadDeCrias = 5;

    [Tooltip("Cuanto se separan las crias entre si al aparecer.")]
    [SerializeField] private float dispersion = 0.4f;

    private EnemyWalker walker;
    private bool yaSeDividio;

    private void Awake()
    {
        walker = GetComponent<EnemyWalker>();
    }

    private void OnEnable()
    {
        yaSeDividio = false;
        EventManager.Subscribe<GameObject>(GameEvents.EnemyDied, AlMorirAlguien);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe<GameObject>(GameEvents.EnemyDied, AlMorirAlguien);
    }

    // Escucha todas las muertes y se queda solo con la propia.
    private void AlMorirAlguien(GameObject muerto)
    {
        if (muerto != gameObject) return;
        if (yaSeDividio) return;

        yaSeDividio = true;
        Dividir();
    }

    private void Dividir()
    {
        if (EnemyFactory.Instance == null) return;

        if (walker == null || walker.Camino == null)
        {
            Debug.LogWarning("El divisor murio sin camino asignado, no puede parirse.");
            return;
        }

        for (int i = 0; i < cantidadDeCrias; i++)
        {
            Vector3 offset = new Vector3(
                Random.Range(-dispersion, dispersion),
                Random.Range(-dispersion, dispersion),
                0f);

            GameObject cria = EnemyFactory.Instance.Crear(
                tipoDeCria,
                walker.Camino,
                transform.position + offset,
                walker.IndiceActual);

            // Sin esto el contador del GameManager no las cuenta
            // y el juego declara la victoria con las crias todavia vivas.
            if (cria != null)
            {
                EventManager.TriggerEvent(GameEvents.EnemySpawned);
            }
        }
    }
}
