using System.Collections.Generic;
using UnityEngine;

public class waypointManager : MonoBehaviour
{
    [SerializeField] List<Transform> waypoints;

    [Header("Solo para verlo en el editor")]
    [SerializeField] Color colorDelCamino = Color.cyan;

    public Transform GetWayPoint(int index)
    {
        return waypoints[index];
    }

    public int wayPointCount
    {
        get { return waypoints.Count; }
    }

    // Dibuja el recorrido en la vista de escena.
    // No afecta al juego, es para poder armar los caminos sin volverse loco.
    private void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Count == 0) return;

        Gizmos.color = colorDelCamino;

        for (int i = 0; i < waypoints.Count; i++)
        {
            if (waypoints[i] == null) continue;

            Gizmos.DrawSphere(waypoints[i].position, 0.15f);

            if (i + 1 < waypoints.Count && waypoints[i + 1] != null)
            {
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
            }
        }
    }
}