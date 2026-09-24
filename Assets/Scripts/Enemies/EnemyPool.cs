using UnityEngine;
using System.Collections.Generic;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool Instance; // Referencia global

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int poolSize = 10;
    [SerializeField] private waypointManager waypointManager;

    private List<GameObject> activeEnemies = new List<GameObject>();
    private List<GameObject> pool = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        for (int i = 0; i < poolSize; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);

            if (enemy.TryGetComponent<EnemyWalker>(out EnemyWalker walker))
            {
                walker.waypointManager = waypointManager;
            }

            enemy.SetActive(false);
            pool.Add(enemy);

            if (enemy.TryGetComponent<EnemyHealth>(out EnemyHealth health))
            {
                health.SetPool(this);
            }
        }
    }

    public GameObject GetEnemy()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                pool[i].transform.position = waypointManager.GetWayPoint(0).position;
                pool[i].SetActive(true);

                if (!activeEnemies.Contains(pool[i]))
                {
                    activeEnemies.Add(pool[i]);
                }

                return pool[i];
            }
        }

        return null;
    }

    public void ReturnEnemy(GameObject enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
        }

        enemy.SetActive(false);
    }

    public List<GameObject> GetActiveEnemies()
    {
        return activeEnemies;
    }
}