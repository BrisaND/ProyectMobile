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
        Debug.Log("Toco " + collision.name);

        if (collision.TryGetComponent<EnemyHealth>(out EnemyHealth enemy))
        {
            enemy.TakeDamage(damage);

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySound(AudioManager.Instance.hitSound);
            }
        }
    }

}
