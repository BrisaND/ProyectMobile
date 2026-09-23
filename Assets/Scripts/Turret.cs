using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public class Turret : MonoBehaviour
{
    [SerializeField] float range = 5f;
    [SerializeField] EnemyPool enemyPool;

    [SerializeField] int damage = 34;
    [SerializeField] float fireRate = 1f;

    private float fireCountdown = 0f;
    Transform target;

    [SerializeField] private BulletPool bulletPool;
    [SerializeField] private HeroSwitcher hero;
    private void Update()
    {
        FindTarget();



        if (target != null)
        {

            if (fireCountdown <= 0)
            {
                Shoot();
                fireCountdown = 1 / GetFireRate();
            }

            fireCountdown -= Time.deltaTime;
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

    private void Shoot()
    {
        Bullet bullet = GetBulletPool().getBullet(transform.position);

        if (bullet != null)
        {
            bullet.Launch(target, GetDamage());
        }
    }

    private float GetFireRate()
    {
        if (hero == null)
        {
            return fireRate;
        }

        return hero.ModifyFireRate(fireRate);
    }

    private int GetDamage()
    {
        if (hero == null)
        {
            return damage;
        }

        return hero.ModifyDamage(damage);
    }

    private BulletPool GetBulletPool()
    {
        if (hero == null)
        {
            return bulletPool;
        }

        return hero.ModifyBulletPool(bulletPool);
    }
}
