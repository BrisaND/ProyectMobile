using System.Collections.Generic;
using UnityEngine;

// El Factory decide QUE enemigo crear y POR DONDE mandarlo;
// el Pool se encarga de COMO reciclarlo.
public class EnemyFactory : MonoBehaviour
{
    public static EnemyFactory Instance;

    [Header("Catalogo de enemigos")]
    [SerializeField] private List<EnemyDefinition> catalogo = new List<EnemyDefinition>();

    [Header("Recorridos")]
    [SerializeField] private CaminoManager caminoManager;

    private readonly Dictionary<EnemyId, EnemyPool> pools = new Dictionary<EnemyId, EnemyPool>();
    private readonly List<GameObject> activos = new List<GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        for (int i = 0; i < catalogo.Count; i++)
        {
            EnemyDefinition def = catalogo[i];

            if (def.prefab == null)
            {
                Debug.LogError("El catalogo tiene una entrada sin prefab: " + def.id);
                continue;
            }

            pools[def.id] = new EnemyPool(def.prefab, def.poolSize, transform);
        }
    }

    private void OnEnable()
    {
        EventManager.Subscribe<GameObject>(GameEvents.EnemyDied, Devolver);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe<GameObject>(GameEvents.EnemyDied, Devolver);
    }

    // Camino al azar entre los tres.
    public GameObject Crear(EnemyId id)
    {
        return Crear(id, caminoManager.GetCaminoAleatorio());
    }

    // Un camino puntual, por indice.
    public GameObject Crear(EnemyId id, int indiceDeCamino)
    {
        return Crear(id, caminoManager.GetCamino(indiceDeCamino));
    }

    // Desde el inicio de un camino concreto.
    public GameObject Crear(EnemyId id, waypointManager camino)
    {
        if (camino == null) return null;

        return Crear(id, camino, camino.GetWayPoint(0).position, 0);
    }

    // Version completa: sirve para el Divisor, cuyas crias tienen que
    // aparecer donde murio el padre y seguir por su mismo camino.
    public GameObject Crear(EnemyId id, waypointManager camino, Vector3 posicion, int waypointInicial)
    {
        if (!pools.ContainsKey(id))
        {
            Debug.LogError("No hay pool para el tipo " + id + ". Falta cargarlo en el catalogo.");
            return null;
        }

        if (camino == null)
        {
            Debug.LogError("Se pidio crear un " + id + " sin camino.");
            return null;
        }

        GameObject enemigo = pools[id].Obtener();
        enemigo.transform.position = posicion;

        if (enemigo.TryGetComponent<EnemyWalker>(out EnemyWalker walker))
        {
            walker.Configurar(camino, waypointInicial);
        }

        enemigo.SetActive(true);

        if (!activos.Contains(enemigo))
        {
            activos.Add(enemigo);
        }

        return enemigo;
    }

    private void Devolver(GameObject enemigo)
    {
        if (!activos.Contains(enemigo))
        {
            return;
        }

        activos.Remove(enemigo);
        enemigo.SetActive(false);
    }

    public List<GameObject> GetActiveEnemies()
    {
        return activos;
    }
}