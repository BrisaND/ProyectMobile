using System.Collections.Generic;
using UnityEngine;

// Pool de los circulos de explosion. Misma idea que BulletPool: no se instancia nada en pleno combate.
public class ExplosionPool : MonoBehaviour
{
    public static ExplosionPool Instance;

    [SerializeField] private ExplosionEffect prefab;
    [SerializeField] private int poolSize = 10;

    private readonly List<ExplosionEffect> pool = new List<ExplosionEffect>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (prefab == null)
        {
            Debug.LogError("El ExplosionPool no tiene prefab asignado.");
            return;
        }

        for (int i = 0; i < poolSize; i++)
        {
            Crear();
        }
    }

    private ExplosionEffect Crear()
    {
        ExplosionEffect efecto = Instantiate(prefab, transform);
        efecto.gameObject.SetActive(false);
        pool.Add(efecto);
        return efecto;
    }

    public void Reproducir(Vector3 posicion, float radio, Color color)
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].gameObject.activeInHierarchy)
            {
                pool[i].Reproducir(posicion, radio, color);
                return;
            }
        }

        // Si hay muchas explosiones juntas, el pool crece.
        Crear().Reproducir(posicion, radio, color);
    }
}
