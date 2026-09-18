using System.Collections.Generic;
using UnityEngine;

public class waypointManager : MonoBehaviour
{
    [SerializeField] List<Transform> waypoints;

    public Transform GetWayPoint(int index)
    {
        return waypoints[index];
    }

    public int wayPointCount
    {
        get { return waypoints.Count; }
    }
}
