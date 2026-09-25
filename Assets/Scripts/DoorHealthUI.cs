using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Muestra la vida de la puerta. No conoce a la puerta: solo escucha el evento
// Es el ejemplo mas simple de por que sirve el Observer: podes agregar cuantos oyentes quieras sin tocar el script de Door
public class DoorHealthUI : MonoBehaviour
{
    [SerializeField] private Door puerta;
    [SerializeField] private Image barra;
    [SerializeField] private TMP_Text texto;

    private void OnEnable()
    {
        EventManager.Subscribe<int>(GameEvents.DoorDamaged, Actualizar);
        EventManager.Subscribe(GameEvents.DoorDestroyed, EnCero);
    }

    // Desuscribirse SIEMPRE. EventManager es static: si no lo haces,al recargar la escena quedan oyentes muertos apuntando a objetos destruidos.
    private void OnDisable()
    {
        EventManager.Unsubscribe<int>(GameEvents.DoorDamaged, Actualizar);
        EventManager.Unsubscribe(GameEvents.DoorDestroyed, EnCero);
    }

    private void Start()
    {
        if (puerta != null)
        {
            Actualizar(puerta.Health);
        }
    }

    private void Actualizar(int vidaActual)
    {
        if (puerta == null)
        {
            return;
        }

        if (barra != null)
        {
            barra.fillAmount = (float)vidaActual / puerta.MaxHealth;
        }

        if (texto != null)
        {
            texto.text = vidaActual + " / " + puerta.MaxHealth;
        }
    }

    private void EnCero()
    {
        Actualizar(0);
    }
}
