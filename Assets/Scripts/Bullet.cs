using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] protected float speed = 5.5f;

    protected Transform target;
    protected int damage;

    public virtual void Launch(Transform newTarget, int newDamage)
    {
        target = newTarget;
        damage = newDamage;
    }

    private protected virtual void Update()
    {
        if (target == null || target.gameObject.activeInHierarchy == false)
        {
            gameObject.SetActive(false);
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);


        if (Vector3.Distance(transform.position, target.position) < 1)
        {
            target.GetComponent<EnemyHealth>().TakeDamage(damage);

            gameObject.SetActive(false);

        }
    }

}
