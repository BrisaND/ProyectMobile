using UnityEngine;
using UnityEngine.UI;
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    [SerializeField] private Image healthBar;
    int currentHealth;
    private EnemyPool pool;
    private void OnEnable()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
           pool.ReturnEnemy(gameObject);
        }
    }
    

    public void SetPool(EnemyPool enemyPool)
    {
        pool = enemyPool;
    }
    private void UpdateHealthBar()
    {
        healthBar.fillAmount = (float)currentHealth / maxHealth;
    }
}
