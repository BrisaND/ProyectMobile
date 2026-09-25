using System.Collections.Generic;
using UnityEngine;

// Bala de area. Al llegar reparte daño a todo lo que este dentro del radio, salvo que haya un absorbente: en ese caso el absorbente se lo come todo.
public class ExplosiveBullet : Bullet
{
    [Header("Explosion")]
    [SerializeField] private float radio = 1.5f;

    [Tooltip("Cuanto daño recibe algo que este justo en el borde, entre 0 y 1.")]
    [Range(0f, 1f)]
    [SerializeField] private float dañoEnElBorde = 0.5f;

    private SpriteRenderer sr;

    // Listas reutilizadas para no generar basura en cada explosion.
    private readonly List<EnemyHealth> alcanzados = new List<EnemyHealth>();
    private readonly List<int> repartos = new List<int>();

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    protected override void Impactar(bool objetivoValido)
    {
        MostrarExplosion();

        alcanzados.Clear();
        repartos.Clear();

        Collider2D[] tocados = Physics2D.OverlapCircleAll(transform.position, radio);

        EnemyAbsorber absorbente = null;
        float distanciaAlAbsorbente = float.MaxValue;
        int dañoTotal = 0;

        // Primera pasada: calcular cuanto le tocaria a cada uno y ver si hay un absorbente en el radio.
        for (int i = 0; i < tocados.Length; i++)
        {
            if (!tocados[i].TryGetComponent<EnemyHealth>(out EnemyHealth enemy))
            {
                continue;
            }

            float distancia = Vector2.Distance(transform.position, tocados[i].transform.position);
            float factor = Mathf.Lerp(1f, dañoEnElBorde, Mathf.Clamp01(distancia / radio));
            int daño = Mathf.Max(1, Mathf.RoundToInt(damage * factor));

            alcanzados.Add(enemy);
            repartos.Add(daño);
            dañoTotal += daño;

            if (tocados[i].TryGetComponent<EnemyAbsorber>(out EnemyAbsorber candidato)
                && candidato.PuedeAbsorber
                && distancia < distanciaAlAbsorbente)
            {
                absorbente = candidato;
                distanciaAlAbsorbente = distancia;
            }
        }

        // Segunda pasada: o se lo come el absorbente, o se reparte normal.
        if (absorbente != null)
        {
            absorbente.Absorber(dañoTotal);
            return;
        }

        for (int i = 0; i < alcanzados.Count; i++)
        {
            alcanzados[i].TakeDamage(repartos[i]);
        }
    }

    private void MostrarExplosion()
    {
        if (ExplosionPool.Instance == null)
        {
            return;
        }

        Color color = sr != null ? sr.color : Color.white;
        ExplosionPool.Instance.Reproducir(transform.position, radio, color);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.4f);
        Gizmos.DrawWireSphere(transform.position, radio);
    }
}