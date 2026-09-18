using UnityEngine;

public class SpawnButtom : MonoBehaviour
{
    [SerializeField] EnemyPool enemypool;

    public void Spawn()
    {
        enemypool.GetEnemy();
    }
}
