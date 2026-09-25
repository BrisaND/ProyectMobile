using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GrupoDeOleada
{
    public EnemyId tipo;
    public int cantidad = 3;

    [Tooltip("-1 = camino al azar. 0, 1, 2 = ese camino puntual.")]
    public int camino = -1;
}

public class WaveSpawner : MonoBehaviour
{
    [Header("Composicion de la oleada")]
    [SerializeField] private List<GrupoDeOleada> grupos = new List<GrupoDeOleada>();

    [Header("Valor por defecto (lo pisa Remote Config si esta disponible)")]
    [SerializeField] private float spawnRate = 1.5f;

    private float timer;
    private int grupoActual;
    private int spawneadosDelGrupo;

    [HideInInspector] public bool waveFinishedSpawning = false;

    private void OnEnable()
    {
        EventManager.Subscribe(GameEvents.RemoteConfigReady, AplicarConfig);

        AplicarConfig();

        timer = spawnRate;
        grupoActual = 0;
        spawneadosDelGrupo = 0;
        waveFinishedSpawning = false;

        SaltearGruposVacios();

        if (grupos.Count == 0)
        {
            Debug.LogError("El WaveSpawner de '" + name + "' no tiene grupos cargados. " +
                           "Llena la lista Grupos en el inspector o la oleada termina apenas empieza.");
        }
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(GameEvents.RemoteConfigReady, AplicarConfig);
    }

    private void AplicarConfig()
    {
        spawnRate = RemoteConfigManager.GetFloat(RemoteConfigKeys.OleadaIntervaloSpawn, spawnRate);

        for (int i = 0; i < grupos.Count; i++)
        {
            if (grupos[i].tipo == EnemyId.Caminante)
            {
                grupos[i].cantidad = RemoteConfigManager.GetInt(RemoteConfigKeys.OleadaCantidadCaminantes, grupos[i].cantidad);
                break;
            }
        }
    }

    // Un grupo en 0 significa "no mandes ninguno de estos", asi que se saltea.
    // Sirve para desactivar un tipo mientras se prueba, sin borrar la entrada.
    private void SaltearGruposVacios()
    {
        while (grupoActual < grupos.Count && grupos[grupoActual].cantidad <= 0)
        {
            grupoActual++;
        }
    }

    private void Update()
    {
        if (GameManager.Instance == null || GameManager.Instance.currentState != GameManager.GameState.Combate)
        {
            return;
        }

        if (waveFinishedSpawning) return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SpawnSiguiente();
            timer = spawnRate;
        }
    }

    private void SpawnSiguiente()
    {
        if (EnemyFactory.Instance == null) return;

        SaltearGruposVacios();

        if (grupoActual >= grupos.Count)
        {
            Terminar();
            return;
        }

        GrupoDeOleada grupo = grupos[grupoActual];

        GameObject enemigo;

        if (grupo.camino < 0)
        {
            enemigo = EnemyFactory.Instance.Crear(grupo.tipo);
        }
        else
        {
            enemigo = EnemyFactory.Instance.Crear(grupo.tipo, grupo.camino);
        }

        if (enemigo != null)
        {
            spawneadosDelGrupo++;
            EventManager.TriggerEvent(GameEvents.EnemySpawned);
        }

        if (spawneadosDelGrupo >= grupo.cantidad)
        {
            grupoActual++;
            spawneadosDelGrupo = 0;
            SaltearGruposVacios();
        }

        if (grupoActual >= grupos.Count)
        {
            Terminar();
        }
    }

    private void Terminar()
    {
        if (waveFinishedSpawning) return;

        waveFinishedSpawning = true;
        EventManager.TriggerEvent(GameEvents.WaveFinished);
    }
}