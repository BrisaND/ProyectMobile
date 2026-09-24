using UnityEngine;

// Una de las opciones que el jugador puede elegir dentro de un evento.
// No es un ScriptableObject aparte: vive dentro del EventoData.
[System.Serializable]
public class OpcionDeEvento
{
    [Tooltip("Lo que dice el boton. Ej: Romperlo")]
    public string texto = "Romperlo";

    [Tooltip("Si esta marcado, no se tira dado: siempre sale el resultado de exito. " +
             "El boton no muestra stat ni porcentaje.")]
    public bool sinCheck = false;

    public StatType stat = StatType.Fuerza;

    [Tooltip("Cuanto hay que tener de ese stat para estar en 50%. Lo define la zona.")]
    public int dificultad = 5;

    [TextArea]
    public string resultadoExito = "Salio bien.";

    [TextArea]
    public string resultadoFallo = "Salio mal. No consiguio nada.";
}