using UnityEngine;

// Toda la matematica de los checks vive aca
// Cuando decidan el rango definitivo de stats, se toca solo este archivo
public static class ResolutorDeEventos
{
    // PROVISORIO. Con estos valores:
    //   stat igual a la dificultad = 50%
    //   cada punto de diferencia   = 10% para un lado o para el otro
    // Nunca 0% ni 100%
    const float ProbabilidadBase = 0.5f;
    const float PasoPorPunto = 0.1f;
    const float ProbabilidadMinima = 0.05f;
    const float ProbabilidadMaxima = 0.95f;

    public static float CalcularProbabilidad(HeroeData heroe, OpcionDeEvento opcion)
    {
        if (heroe == null || opcion == null)
        {
            return 0f;
        }

        // Una opcion sin check no se tira: siempre sale bien.
        if (opcion.sinCheck)
        {
            return 1f;
        }

        int stat = heroe.GetStat(opcion.stat);
        float probabilidad = ProbabilidadBase + (stat - opcion.dificultad) * PasoPorPunto;

        return Mathf.Clamp(probabilidad, ProbabilidadMinima, ProbabilidadMaxima);
    }

    public static bool Tirar(float probabilidad)
    {
        return Random.value < probabilidad;
    }
}