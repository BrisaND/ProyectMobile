using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Valores por defecto (los pisa Remote Config si esta disponible)")]
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private int damagePorEnemigo = 5;

    private int health;

    public int Health { get { return health; } }
    public int MaxHealth { get { return maxHealth; } }

    private void Awake()
    {
        AplicarConfig();
        health = maxHealth;
    }

    private void OnEnable()
    {
        EventManager.Subscribe<int>(GameEvents.DoorHit, TakeDamage);
        EventManager.Subscribe(GameEvents.RemoteConfigReady, AlLlegarLaConfig);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe<int>(GameEvents.DoorHit, TakeDamage);
        EventManager.Unsubscribe(GameEvents.RemoteConfigReady, AlLlegarLaConfig);
    }

    private void AplicarConfig()
    {
        maxHealth = RemoteConfigManager.GetInt(RemoteConfigKeys.PuertaVidaMaxima, maxHealth);
        damagePorEnemigo = RemoteConfigManager.GetInt(RemoteConfigKeys.PuertaDanioPorEnemigo, damagePorEnemigo);
    }

    // Si la config llega despues del Awake, se reconfigura y la puerta
    // vuelve a full. Solo pasa al principio del nivel.
    private void AlLlegarLaConfig()
    {
        AplicarConfig();
        health = maxHealth;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyWalker enemy = other.GetComponent<EnemyWalker>();

        if (enemy == null)
        {
            return;
        }

        enemy.PushBack();
        TakeDamage(damagePorEnemigo);
    }

    public void TakeDamage(int amount)
    {
        if (health <= 0)
        {
            return;
        }

        health -= amount;

        if (health <= 0)
        {
            health = 0;
            EventManager.TriggerEvent(GameEvents.DoorDestroyed);
            return;
        }

        EventManager.TriggerEvent<int>(GameEvents.DoorDamaged, health);
    }
}