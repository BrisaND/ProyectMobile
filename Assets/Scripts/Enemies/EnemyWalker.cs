using System.Collections.Generic;
using UnityEngine;

public class EnemyWalker : MonoBehaviour
{
    public waypointManager waypointManager;
    [SerializeField] float speed = 2f;

    int currentWayPoint = 0;

    private void OnEnable()
    {
        currentWayPoint = 0;
    }
    private void Update()
    {
        if (currentWayPoint >= waypointManager.wayPointCount)
        {
            return;
        }

        Transform target = waypointManager.GetWayPoint(currentWayPoint);
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);


        if (Vector3.Distance(transform.position, target.position) < 0.05f)
        {
            currentWayPoint++;
        }

    }

    public void PushBack()
    {
        currentWayPoint--;
        if (currentWayPoint < 0)
        {
            currentWayPoint = 0;
        }
    }

}
