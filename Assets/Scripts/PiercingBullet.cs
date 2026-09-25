using System.Collections.Generic;
using UnityEngine;

// Bala penetrante: vuela recto y atraviesa todo lo que encuentra.
// Como no persigue al objetivo, apunta a donde el enemigo VA A ESTAR y no a donde esta. Asi le pega sin corregir el rumbo en el aire.
public class PiercingBullet : Bullet
{
    [SerializeField] float MaxDistance = 30;

    private Vector3 direction;
    private float traveled;

    private readonly List<EnemyHealth> yaGolpeados = new List<EnemyHealth>();

    public override void Launch(Transform newTarget, int newDamage)
    {
        base.Launch(newTarget, newDamage);

        yaGolpeados.Clear();
        traveled = 0f;

        if (newTarget == null)
        {
            gameObject.SetActive(false);
            return;
        }

        Vector3 puntoDeMira = CalcularPuntoDeMira(newTarget);
        direction = (puntoDeMira - transform.position).normalized;
    }

    private Vector3 CalcularPuntoDeMira(Transform objetivo)
    {
        // Si el enemigo no camina (o no es un enemigo), apuntamos directo.
        if (!objetivo.TryGetComponent<EnemyWalker>(out EnemyWalker walker))
        {
            return objetivo.position;
        }

        return Balistica.PuntoDeIntercepcion(
            transform.position,
            objetivo.position,
            walker.Velocidad,
            speed);
    }

    private protected override void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position += direction * step;
        traveled += step;

        if (traveled >= MaxDistance)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent<EnemyHealth>(out EnemyHealth enemy))
        {
            return;
        }

        if (yaGolpeados.Contains(enemy))
        {
            return;
        }

        yaGolpeados.Add(enemy);
        enemy.TakeDamage(damage);
    }
}