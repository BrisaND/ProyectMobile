using UnityEngine;

// Circulo que aparece donde exploto la bala: crece hasta el radio real
// del daño y se desvanece. Se apaga solo y vuelve al pool.
[RequireComponent(typeof(SpriteRenderer))]
public class ExplosionEffect : MonoBehaviour
{
    [SerializeField] private float duracion = 0.25f;

    [Range(0f, 1f)]
    [SerializeField] private float alphaInicial = 0.45f;

    [Tooltip("De que tamaño arranca, como fraccion del radio final.")]
    [Range(0f, 1f)]
    [SerializeField] private float escalaInicial = 0.35f;

    private SpriteRenderer sr;
    private float tiempo;
    private float radio = 1f;
    private Color colorBase = Color.white;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void Reproducir(Vector3 posicion, float radioDelDaño, Color color)
    {
        if (sr == null)
        {
            sr = GetComponent<SpriteRenderer>();
        }

        transform.position = posicion;
        radio = radioDelDaño;
        colorBase = color;
        tiempo = 0f;

        gameObject.SetActive(true);
        Aplicar();
    }

    private void Update()
    {
        tiempo += Time.deltaTime;

        if (tiempo >= duracion)
        {
            gameObject.SetActive(false);
            return;
        }

        Aplicar();
    }

    private void Aplicar()
    {
        float avance = Mathf.Clamp01(tiempo / duracion);

        // El sprite Circle de Unity mide 1 unidad de diametro,
        // asi que la escala final tiene que ser el radio por 2.
        float diametroFinal = radio * 2f;
        float escala = Mathf.Lerp(diametroFinal * escalaInicial, diametroFinal, avance);
        transform.localScale = new Vector3(escala, escala, 1f);

        Color c = colorBase;
        c.a = Mathf.Lerp(alphaInicial, 0f, avance);
        sr.color = c;
    }
}
