using UnityEngine;

// Calculo de tiro predictivo: donde hay que apuntar para que un proyectil
// que vuela recto a velocidad fija intercepte a algo que se mueve.
public static class Balistica
{
    public static Vector3 PuntoDeIntercepcion(Vector3 origen, Vector3 objetivo, Vector3 velocidadObjetivo, float velocidadBala)
    {
        Vector3 d = objetivo - origen;

        float a = Vector3.Dot(velocidadObjetivo, velocidadObjetivo) - velocidadBala * velocidadBala;
        float b = 2f * Vector3.Dot(velocidadObjetivo, d);
        float c = Vector3.Dot(d, d);

        float t;

        if (Mathf.Abs(a) < 0.0001f)
        {
            // El enemigo va casi tan rapido como la bala: la cuadratica
            // degenera en una ecuacion lineal.
            if (Mathf.Abs(b) < 0.0001f)
            {
                return objetivo;
            }

            t = -c / b;
        }
        else
        {
            float discriminante = b * b - 4f * a * c;

            if (discriminante < 0f)
            {
                // No hay intercepcion posible, el enemigo es mas rapido y se escapa. Apuntamos directo y que sea lo que Dios quiera.
                return objetivo;
            }

            float raiz = Mathf.Sqrt(discriminante);
            float t1 = (-b + raiz) / (2f * a);
            float t2 = (-b - raiz) / (2f * a);

            t = MenorPositivo(t1, t2);
        }

        if (t <= 0f)
        {
            return objetivo;
        }

        return objetivo + velocidadObjetivo * t;
    }

    private static float MenorPositivo(float x, float y)
    {
        if (x > 0f && y > 0f) return Mathf.Min(x, y);
        if (x > 0f) return x;
        if (y > 0f) return y;
        return -1f;
    }
}
