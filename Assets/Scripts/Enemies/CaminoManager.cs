using System.Collections.Generic;
using UnityEngine;

// Contiene los caminos del laberinto. Cada camino es un waypointManager
// con su propia lista de waypoints, y todos terminan en la puerta.
public class CaminoManager : MonoBehaviour
{
    [SerializeField] private List<waypointManager> caminos = new List<waypointManager>();

    public int CantidadDeCaminos
    {
        get { return caminos.Count; }
    }

    public waypointManager GetCamino(int index)
    {
        if (caminos.Count == 0)
        {
            Debug.LogError("El CaminoManager no tiene ningun camino cargado.");
            return null;
        }

        index = Mathf.Clamp(index, 0, caminos.Count - 1);
        return caminos[index];
    }

    public waypointManager GetCaminoAleatorio()
    {
        if (caminos.Count == 0)
        {
            Debug.LogError("El CaminoManager no tiene ningun camino cargado.");
            return null;
        }

        return caminos[Random.Range(0, caminos.Count)];
    }
}
