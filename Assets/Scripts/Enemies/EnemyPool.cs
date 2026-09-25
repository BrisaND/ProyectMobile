using System.Collections.Generic;
using UnityEngine;

// Pool generico de UN tipo de enemigo.
// Ya no es MonoBehaviour: es una clase comun que el Factory crea y administra.
// Su unica responsabilidad es reciclar objetos (la S de SOLID).
public class EnemyPool
{
    private readonly GameObject prefab;
    private readonly List<GameObject> objetos = new List<GameObject>();
    private readonly Transform contenedor;

    public EnemyPool(GameObject prefab, int tamaño, Transform contenedor)
    {
        this.prefab = prefab;
        this.contenedor = contenedor;

        for (int i = 0; i < tamaño; i++)
        {
            Crear();
        }
    }

    private GameObject Crear()
    {
        GameObject go = Object.Instantiate(prefab, contenedor);
        go.SetActive(false);
        objetos.Add(go);
        return go;
    }

    public GameObject Obtener()
    {
        for (int i = 0; i < objetos.Count; i++)
        {
            if (!objetos[i].activeInHierarchy)
            {
                return objetos[i];
            }
        }

        // Si se acabaron, el pool crece en vez de devolver null.
        // Asi un divisor que escupe 5 chiquitos nunca se queda sin cupo.
        return Crear();
    }

    public bool Contiene(GameObject go)
    {
        return objetos.Contains(go);
    }
}