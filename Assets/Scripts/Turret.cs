using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public class Turret : MonoBehaviour
{
    [SerializeField] float range = 5f;
    [SerializeField] EnemyPool enemyPool;

    Transform target;

    private void Update()
    {
        FindTarget();

        if (target != null)
        {
            transform.LookAt(target);
        }
    }

    void FindTarget()
    {
        List<GameObject> enemies = enemyPool.GetActiveEnemies();
        float closesDistance = range;
        target = null;

        for (int i = 0; i < enemies.Count; i++)
        {
            float distance = Vector3.Distance(transform.position, enemies[i].transform.position);

            if (distance < closesDistance) 
            {
                closesDistance = distance;
                target = enemies[i].transform;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
