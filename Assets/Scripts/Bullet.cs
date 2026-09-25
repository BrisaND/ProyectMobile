using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] protected float speed = 5.5f;
    [SerializeField] float hitDistance = 0.2f;

    protected Transform target;
    protected int damage;

    protected EnemyHealth objetivoSalud;
    private int generacionObjetivo;

    private Vector3 ultimaPosicionConocida;

    // El objetivo sigue siendo el mismo de cuando salio el disparo.
    // No alcanza con preguntar si esta activo: el pool recicla el mismo
    // GameObject para el enemigo siguiente, y sin el numero de generacion
    // la bala se iria persiguiendo a un enemigo distinto en pleno vuelo.
    protected bool ObjetivoValido
    {
        get
        {
            return target != null
                && target.gameObject.activeInHierarchy
                && objetivoSalud != null
                && objetivoSalud.Generacion == generacionObjetivo;
        }
    }

    public virtual void Launch(Transform newTarget, int newDamage)
    {
        target = newTarget;
        damage = newDamage;

        if (newTarget != null)
        {
            objetivoSalud = newTarget.GetComponent<EnemyHealth>();
            generacionObjetivo = objetivoSalud != null ? objetivoSalud.Generacion : -1;
            ultimaPosicionConocida = newTarget.position;
        }
        else
        {
            objetivoSalud = null;
            generacionObjetivo = -1;
            ultimaPosicionConocida = transform.position;
        }
    }

    private protected virtual void Update()
    {
        bool valido = ObjetivoValido;

        if (valido)
        {
            ultimaPosicionConocida = target.position;
        }

        transform.position = Vector3.MoveTowards(transform.position, ultimaPosicionConocida, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, ultimaPosicionConocida) < hitDistance)
        {
            Impactar(valido);
            gameObject.SetActive(false);
        }
    }

    // Lo que pasa al llegar. Cada tipo de bala lo resuelve a su manera.
    protected virtual void Impactar(bool objetivoValido)
    {
        if (!objetivoValido || objetivoSalud == null)
        {
            return;
        }

        objetivoSalud.TakeDamage(damage);
    }
}