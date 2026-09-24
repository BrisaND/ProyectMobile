using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Corre una expedicion completa: sortea 2 a 4 eventos, y en cada uno
// sortea que heroe del grupo lo enfrenta.
public class PanelDeEvento : MonoBehaviour
{
    [Header("Datos de prueba")]
    [SerializeField] List<HeroeData> grupo = new List<HeroeData>();
    [SerializeField] List<EventoData> eventosPosibles = new List<EventoData>();
    [SerializeField] int minEventos = 2;
    [SerializeField] int maxEventos = 4;

    [Header("UI")]
    [SerializeField] TMP_Text textoEvento;
    [SerializeField] TMP_Text textoResultado;
    [SerializeField] Transform contenedorDeOpciones;
    [SerializeField] Button prefabDeOpcion;
    [SerializeField] Button botonSiguiente;
    [SerializeField] Button botonIniciar;

    private int eventosRestantes;
    private HeroeData heroeActual;
    private EventoData eventoActual;
    private List<Button> botonesActivos = new List<Button>();

    private void Start()
    {
        textoEvento.text = "La expedicion esta lista para salir.";
        textoResultado.text = "";
        botonSiguiente.gameObject.SetActive(false);
        botonIniciar.gameObject.SetActive(true);
    }

    // Conectar al boton Iniciar desde el inspector.
    public void IniciarExpedicion()
    {
        if (grupo.Count == 0 || eventosPosibles.Count == 0)
        {
            Debug.LogError("Falta cargar el grupo o los eventos en el PanelDeEvento.");
            return;
        }

        eventosRestantes = Random.Range(minEventos, maxEventos + 1);
        botonIniciar.gameObject.SetActive(false);
        MostrarSiguienteEvento();
    }

    // Conectar al boton Siguiente desde el inspector.
    public void MostrarSiguienteEvento()
    {
        LimpiarOpciones();
        textoResultado.text = "";
        botonSiguiente.gameObject.SetActive(false);

        if (eventosRestantes <= 0)
        {
            textoEvento.text = "La expedicion volvio al campamento.";
            botonIniciar.gameObject.SetActive(true);
            return;
        }

        eventosRestantes--;

        // El heroe lo sortea el juego, no lo elige el jugador.
        heroeActual = grupo[Random.Range(0, grupo.Count)];
        eventoActual = eventosPosibles[Random.Range(0, eventosPosibles.Count)];

        textoEvento.text = PonerNombre(eventoActual.texto);

        // Evento sin opciones: es solo narrativo. No hay decision ni dado.
        // Se muestra el texto y el jugador sigue de largo.
        if (eventoActual.opciones.Count == 0)
        {
            botonSiguiente.gameObject.SetActive(true);
            return;
        }

        for (int i = 0; i < eventoActual.opciones.Count; i++)
        {
            CrearBoton(eventoActual.opciones[i]);
        }
    }

    // Reemplaza {heroe} por el nombre del heroe sorteado.
    // Sirve para el texto del evento, para los resultados y para los botones.
    private string PonerNombre(string texto)
    {
        if (heroeActual == null || string.IsNullOrEmpty(texto))
        {
            return texto;
        }

        return texto.Replace("{heroe}", heroeActual.nombre);
    }

    private void CrearBoton(OpcionDeEvento opcion)
    {
        Button boton = Instantiate(prefabDeOpcion, contenedorDeOpciones);

        float probabilidad = ResolutorDeEventos.CalcularProbabilidad(heroeActual, opcion);
        TMP_Text texto = boton.GetComponentInChildren<TMP_Text>();

        if (opcion.sinCheck)
        {
            texto.text = PonerNombre(opcion.texto);
        }
        else
        {
            int stat = heroeActual.GetStat(opcion.stat);
            texto.text = PonerNombre(opcion.texto) + "\n<size=70%>" + opcion.stat + " " + stat
                       + "   -   " + Mathf.RoundToInt(probabilidad * 100) + "%</size>";
        }

        boton.onClick.AddListener(delegate { Resolver(opcion, probabilidad); });
        botonesActivos.Add(boton);
    }

    private void Resolver(OpcionDeEvento opcion, float probabilidad)
    {
        bool exito = ResolutorDeEventos.Tirar(probabilidad);

        LimpiarOpciones();

        if (exito)
        {
            textoResultado.text = PonerNombre(opcion.resultadoExito);
        }
        else
        {
            textoResultado.text = PonerNombre(opcion.resultadoFallo);
        }

        botonSiguiente.gameObject.SetActive(true);
    }

    private void LimpiarOpciones()
    {
        for (int i = 0; i < botonesActivos.Count; i++)
        {
            Destroy(botonesActivos[i].gameObject);
        }

        botonesActivos.Clear();
    }
}