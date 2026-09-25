using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    [SerializeField] private Image healthBar;

    int currentHealth;

    // Sube cada vez que este objeto sale del pool como un enemigo nuevo.
    // Sirve para que las balas viejas no confundan al inquilino anterior
    // con el actual, porque es el mismo GameObject reciclado.
    public int Generacion { get; private set; }

    private void OnEnable()
    {
        Generacion++;
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0)
        {
            return;
        }

        currentHealth -= damage;
        UpdateHealthBar();

        EventManager.TriggerEvent(GameEvents.EnemyDamaged);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            EventManager.TriggerEvent<GameObject>(GameEvents.EnemyDied, gameObject);
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBar == null)
        {
            return;
        }

        healthBar.fillAmount = (float)currentHealth / maxHealth;
    }
}