using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [Header("Configuración de la Oleada")]
    [SerializeField] private float spawnRate = 1.5f;
    [SerializeField] private int enemiesToSpawn = 5;

    private float timer;
    private int enemiesSpawned = 0;

    [HideInInspector] public bool waveFinishedSpawning = false;

    private void OnEnable()
    {
        timer = spawnRate;
        enemiesSpawned = 0;
        waveFinishedSpawning = false;
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
            SpawnEnemy();
            timer = spawnRate;
        }
    }

    private void SpawnEnemy()
    {
        if (EnemyPool.Instance != null)
        {
            GameObject enemy = EnemyPool.Instance.GetEnemy();

            if (enemy != null)
            {
                enemiesSpawned++;
            }

            if (enemiesSpawned >= enemiesToSpawn)
            {
                waveFinishedSpawning = true;
            }
        }
    }
}