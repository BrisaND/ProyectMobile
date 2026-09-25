using UnityEngine;

// ENEMIGO ABSORBENTE
// Cuando una explosion alcanza a un grupo donde el esta, se come TODO
// el daño del area y los demas no reciben nada. La contra es el daño
// de objetivo unico: guerrero o arquero.
[RequireComponent(typeof(EnemyHealth))]
public class EnemyAbsorber : MonoBehaviour
{
    [Header("Absorcion")]
    [Tooltip("1 = se come el daño completo. Menos de 1 = parte se pierde, o sea que ademas resiste.")]
    [Range(0f, 2f)]
    [SerializeField] private float multiplicadorDeAbsorcion = 1f;

    [Header("Feedback")]
    [SerializeField] private float duracionDelDestello = 0.15f;
    [SerializeField] private Color colorDelDestello = Color.cyan;

    private EnemyHealth salud;
    private SpriteRenderer sr;
    private Color colorOriginal;
    private float destelloRestante;

    public bool PuedeAbsorber
    {
        get { return isActiveAndEnabled; }
    }

    private void Awake()
    {
        salud = GetComponent<EnemyHealth>();
        sr = GetComponentInChildren<SpriteRenderer>();

        if (sr != null)
        {
            colorOriginal = sr.color;
        }
    }

    private void OnEnable()
    {
        destelloRestante = 0f;

        if (sr != null)
        {
            sr.color = colorOriginal;
        }
    }

    // Lo llama la bala explosiva con la suma de todo lo que
    // le hubiera tocado a cada enemigo del radio.
    public void Absorber(int dañoTotal)
    {
        int dañoFinal = Mathf.Max(1, Mathf.RoundToInt(dañoTotal * multiplicadorDeAbsorcion));
        salud.TakeDamage(dañoFinal);

        destelloRestante = duracionDelDestello;

        if (sr != null)
        {
            sr.color = colorDelDestello;
        }
    }

    private void Update()
    {
        if (destelloRestante <= 0f) return;

        destelloRestante -= Time.deltaTime;

        if (destelloRestante <= 0f && sr != null)
        {
            sr.color = colorOriginal;
        }
    }
}
