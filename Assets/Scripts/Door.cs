using UnityEngine;

public class Door : MonoBehaviour
{
    public int health = 10;
    [SerializeField] int damage = 5;


    private void OnTriggerEnter(Collider other)
    {
        EnemyWalker enemy = other.GetComponent<EnemyWalker>();
        if (enemy != null)
        {
            enemy.PushBack();
            takeDamage(damage);
        }
    }


    public void takeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Debug.Log("SEXO");
        }
    }
}
