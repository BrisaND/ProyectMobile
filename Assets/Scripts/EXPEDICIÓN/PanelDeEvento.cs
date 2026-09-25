using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelDeEvento : MonoBehaviour
{
    [Header("Datos de prueba")]
    [SerializeField] List<HeroeData> grupo = new List<HeroeData>();
    [SerializeField] List<EventoData> eventosPosibles = new List<EventoData>();
    [SerializeField] int minEventos = 2;

    [Header("Valores por defecto (los pisa Remote Config si esta disponible)")]
    [SerializeField] int maxEventos = 4;
    [SerializeField] bool mostrarPorcentaje = true;

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

    private void OnEnable()
    {
        EventManager.Subscribe(GameEvents.RemoteConfigReady, AplicarConfig);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(GameEvents.RemoteConfigReady, AplicarConfig);
    }

    private void AplicarConfig()
    {
        maxEventos = RemoteConfigManager.GetInt(RemoteConfigKeys.ExpedicionEventosMax, maxEventos);
        mostrarPorcentaje = RemoteConfigManager.GetBool(RemoteConfigKeys.MostrarPorcentajeChecks, mostrarPorcentaje);
    }

    private void Start()
    {
        AplicarConfig();

        textoEvento.text = "La expedicion esta lista para salir.";
        textoResultado.text = "";
        botonSiguiente.gameObject.SetActive(false);
        botonIniciar.gameObject.SetActive(true);
    }

    public void IniciarExpedicion()
    {
        if (grupo.Count == 0 || eventosPosibles.Count == 0)
        {
            Debug.LogError("Falta cargar el grupo o los eventos en el PanelDeEvento.");
            return;
        }

        eventosRestantes = Random.Range(minEventos, Mathf.Max(minEventos, maxEventos) + 1);
        botonIniciar.gameObject.SetActive(false);
        MostrarSiguienteEvento();
    }

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

        heroeActual = grupo[Random.Range(0, grupo.Count)];
        eventoActual = eventosPosibles[Random.Range(0, eventosPosibles.Count)];

        textoEvento.text = PonerNombre(eventoActual.texto);

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
        else if (mostrarPorcentaje)
        {
            int stat = heroeActual.GetStat(opcion.stat);
            texto.text = PonerNombre(opcion.texto) + "\n<size=70%>" + opcion.stat + " " + stat
                       + "   -   " + Mathf.RoundToInt(probabilidad * 100) + "%</size>";
        }
        else
        {
            // Modo a ciegas: se ve que stat se prueba, pero no la probabilidad.
            texto.text = PonerNombre(opcion.texto) + "\n<size=70%>" + opcion.stat + "</size>";
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