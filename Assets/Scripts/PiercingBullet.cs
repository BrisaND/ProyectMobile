using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PiercingBullet : Bullet
{
    [SerializeField] float MaxDistance = 30;

    private Vector3 direction;
    private float traveled;
    public override void Launch (Transform newTarget, int newDamage)
    {
        base.Launch (newTarget, newDamage);
        direction = (newTarget.position - transform.position).normalized;
        traveled = 0f;
    }

    private protected override void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position += direction * step;
        traveled += step;

        if (traveled >= MaxDistance) 
        {
            gameObject.SetActive (false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("toco " + collision.name);
        EnemyHealth enemy = collision.GetComponent<EnemyHealth>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
    }
   
}
