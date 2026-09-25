using UnityEngine;

// Una entrada del catalogo del Factory: que prefab corresponde a que tipo
// y cuantos tener precreados en su pool.
[System.Serializable]
public class EnemyDefinition
{
    public EnemyId id;
    public GameObject prefab;
    public int poolSize = 10;
}
