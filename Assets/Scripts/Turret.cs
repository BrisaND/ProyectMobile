using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Atributos Base (los pisa Remote Config si esta disponible)")]
    [SerializeField] private float range = 5f;
    [SerializeField] private int damage = 34;
    [SerializeField] private float fireRate = 1f;

    [Header("Referencias")]
    [SerializeField] private HeroSwitcher hero;
    [SerializeField] private BulletPool bulletPool;

    [Header("Cuando esta apagada")]
    [SerializeField] private Color colorApagada = new Color(0.45f, 0.45f, 0.5f, 1f);

    private float fireCountdown = 0f;
    private Transform target;

    private float apagadaRestante;
    private SpriteRenderer sr;
    private Color colorNormal;

    public bool Apagada
    {
        get { return apagadaRestante > 0f; }
    }

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        if (sr != null)
        {
            colorNormal = sr.color;
        }
    }

    private void Start()
    {
        AplicarConfig();
    }

    private void OnEnable()
    {
        EventManager.Subscribe(GameEvents.RemoteConfigReady, AplicarConfig);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(GameEvents.RemoteConfigReady, AplicarConfig);
    }

    private void AplicarConfig()
    {
        damage = RemoteConfigManager.GetInt(RemoteConfigKeys.TorreDanioBase, damage);
        range = RemoteConfigManager.GetFloat(RemoteConfigKeys.TorreAlcance, range);
    }

    // La llama el enemigo desactivador. La torre no sabe quien la apago ni por que: solo recibe cuantos segundos tiene que quedarse quieta.
    public void Apagar(float segundos)
    {
        if (segundos <= 0f) return;

        // Si ya estaba apagada, se queda con el apagado mas largo en vez de acumularlos.
        apagadaRestante = Mathf.Max(apagadaRestante, segundos);
        target = null;

        if (sr != null)
        {
            sr.color = colorApagada;
        }

        EventManager.TriggerEvent(GameEvents.TurretDisabled);
    }

    private void Update()
    {
        if (apagadaRestante > 0f)
        {
            apagadaRestante -= Time.deltaTime;

            if (apagadaRestante <= 0f)
            {
                apagadaRestante = 0f;

                if (sr != null)
                {
                    sr.color = colorNormal;
                }
            }

            return;
        }

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
        if (EnemyFactory.Instance == null) return;

        List<GameObject> enemies = EnemyFactory.Instance.GetActiveEnemies();

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

        if (poolToUse == null) return;

        Bullet bullet = poolToUse.getBullet(transform.position);

        if (bullet != null)
        {
            bullet.Launch(target, GetDamage());
            EventManager.TriggerEvent(GameEvents.TurretShot);
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