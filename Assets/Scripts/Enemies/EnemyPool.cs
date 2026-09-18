using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Pool;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] int poolSize = 10;
    [SerializeField] waypointManager waypointManager;


    private List<GameObject> activeEnemies = new List<GameObject>();
    private List<GameObject> pool = new List<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < poolSize; i++) 
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.GetComponent<EnemyWalker>().waypointManager = waypointManager;
            enemy.SetActive(false);
            pool.Add(enemy);
            enemy.GetComponent<EnemyHealth>().SetPool(this);
        }
    }


    public GameObject GetEnemy()
    {
        for (int i = 0;i < poolSize; i++)
        {
            if (pool[i].activeInHierarchy == false)
            {
                pool[i].transform.position = waypointManager.GetWayPoint(0).position;
                pool[i].SetActive(true);
                activeEnemies.Add(pool[i]);
                return pool[i];
            }
        }

        return null;
    }

    public void ReturnEnemy(GameObject enemy)
    {
        activeEnemies.Remove(enemy);
        enemy.SetActive(false);
    }

    public List<GameObject> GetActiveEnemies()
    {
        return activeEnemies;
    }
}
