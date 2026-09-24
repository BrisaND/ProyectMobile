using System.Collections.Generic;
using UnityEngine;

// Un evento de expedicion. Cada uno es un asset suelto en el proyecto,
// asi que agregar eventos nuevos NO requiere tocar codigo.
[CreateAssetMenu(fileName = "Evento", menuName = "Expedicion/Evento")]
public class EventoData : ScriptableObject
{
    [TextArea]
    [Tooltip("Escribi {heroe} donde quieras que aparezca el nombre del heroe sorteado.")]
    public string texto = "{heroe} encontro un cofre.";

    public List<OpcionDeEvento> opciones = new List<OpcionDeEvento>();
}
