using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Atributos Base")]
    [SerializeField] private float range = 5f;
    [SerializeField] private int damage = 34;
    [SerializeField] private float fireRate = 1f;

    [Header("Referencias")]
    [SerializeField] private HeroSwitcher hero;

    private float fireCountdown = 0f;
    private Transform target;
    [SerializeField] private BulletPool bulletPool;
    private void Update()
    {
        if (target == null || !target.gameObject.activeInHierarchy || Vector2.Distance(transform.position, target.position) > range)
        {
            FindTarget();
        }

        if (target != null)
        {
            if (fireCountdown <= 0f)
            {
                Shoot();
                fireCountdown = 1f / GetFireRate();
            }

            fireCountdown -= Time.deltaTime;
        }
    }

    private void FindTarget()
    {

        List<GameObject> enemies = EnemyPool.Instance.GetActiveEnemies();

        if (enemies.Count == 0) return;

        float closestDistance = range;
        target = null;

        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == null || !enemies[i].activeInHierarchy) continue;

            float distance = Vector2.Distance(transform.position, enemies[i].transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                target = enemies[i].transform;
            }
        }

    }

    private void Shoot()
    {
        BulletPool poolToUse = GetBulletPool();

        Bullet bullet = poolToUse.getBullet(transform.position);

        if (bullet != null)
        {
            bullet.Launch(target, GetDamage());

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySound(AudioManager.Instance.shootSound);
            }
        }
    }

    private float GetFireRate()
    {
        if (hero == null) return fireRate;
        return hero.ModifyFireRate(fireRate);
    }

    private int GetDamage()
    {
        if (hero == null) return damage;
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

    public void SetHero(HeroSwitcher newHero)
    {
        hero = newHero;
    }
    public void SetBulletPool(BulletPool pool)
    {
        bulletPool = pool;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}