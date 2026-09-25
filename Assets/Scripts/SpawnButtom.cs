using UnityEngine;

// Boton de prueba: spawnea un enemigo suelto sin esperar a la oleada.
public class SpawnButtom : MonoBehaviour
{
    [Tooltip("Que tipo spawnear al tocar el boton.")]
    [SerializeField] private EnemyId tipo = EnemyId.Caminante;

    public void Spawn()
    {
        if (EnemyFactory.Instance == null)
        {
            Debug.LogError("No hay EnemyFactory en la escena.");
            return;
        }

        GameObject enemigo = EnemyFactory.Instance.Crear(tipo);

        // Avisamos igual que el WaveSpawner, asi el contador del GameManager
        // no se desincroniza al usar el boton de prueba.
        if (enemigo != null)
        {
            EventManager.TriggerEvent(GameEvents.EnemySpawned);
        }
    }
}