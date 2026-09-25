using UnityEngine;

public class EnemyWalker : MonoBehaviour
{
    [Header("Valor por defecto (lo pisa Remote Config si esta disponible)")]
    [SerializeField] float speed = 2f;

    private waypointManager camino;
    private int currentWayPoint = 0;

    public bool Detenido { get; set; }

    public waypointManager Camino { get { return camino; } }

    public int IndiceActual { get { return currentWayPoint; } }
    public int CantidadDeWaypoints { get { return camino != null ? camino.wayPointCount : 0; } }

    // Hacia donde y a que velocidad se esta moviendo ahora mismo.
    // Lo usan las balas rectas para calcular el tiro predictivo.
    public Vector3 Velocidad
    {
        get
        {
            if (Detenido || camino == null) return Vector3.zero;
            if (currentWayPoint >= camino.wayPointCount) return Vector3.zero;

            Transform destino = camino.GetWayPoint(currentWayPoint);

            if (destino == null) return Vector3.zero;

            return (destino.position - transform.position).normalized * speed;
        }
    }

    public void Configurar(waypointManager nuevoCamino, int waypointInicial)
    {
        camino = nuevoCamino;
        currentWayPoint = waypointInicial;
        Detenido = false;

        speed = RemoteConfigManager.GetFloat(RemoteConfigKeys.EnemigoVelocidad, speed);
    }

    private void Update()
    {
        if (Detenido || camino == null) return;

        if (currentWayPoint >= camino.wayPointCount)
        {
            return;
        }

        Transform target = camino.GetWayPoint(currentWayPoint);
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