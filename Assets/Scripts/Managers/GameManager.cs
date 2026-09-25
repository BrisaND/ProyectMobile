using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GameState { Preparacion, Combate, GameOver }
    public GameState currentState = GameState.Preparacion;

    [Header("UI y Paneles")]
    [SerializeField] private Button startWaveButton;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;

    [Header("Referencias del Nivel")]
    [SerializeField] private WaveSpawner waveSpawner;

    private int enemigosVivos = 0;
    private int enemigosSpawneados = 0;
    private bool oleadaTerminada = false;
    private bool revisarVictoriaPendiente = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        if (startWaveButton != null)
        {
            startWaveButton.onClick.RemoveAllListeners();
            startWaveButton.onClick.AddListener(IniciarCombate);
        }

        IniciarFasePreparacion();
    }

    private void OnEnable()
    {
        EventManager.Subscribe(GameEvents.DoorDestroyed, GameOver);
        EventManager.Subscribe(GameEvents.EnemySpawned, EnemigoAparecio);
        EventManager.Subscribe<GameObject>(GameEvents.EnemyDied, EnemigoMurio);
        EventManager.Subscribe(GameEvents.WaveFinished, OleadaTerminoDeSpawnear);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(GameEvents.DoorDestroyed, GameOver);
        EventManager.Unsubscribe(GameEvents.EnemySpawned, EnemigoAparecio);
        EventManager.Unsubscribe<GameObject>(GameEvents.EnemyDied, EnemigoMurio);
        EventManager.Unsubscribe(GameEvents.WaveFinished, OleadaTerminoDeSpawnear);
    }

    public void IniciarFasePreparacion()
    {
        currentState = GameState.Preparacion;
        Time.timeScale = 1f;

        enemigosVivos = 0;
        enemigosSpawneados = 0;
        oleadaTerminada = false;
        revisarVictoriaPendiente = false;

        if (waveSpawner != null) waveSpawner.gameObject.SetActive(false);
        if (startWaveButton != null) startWaveButton.gameObject.SetActive(true);
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);
    }

    public void IniciarCombate()
    {
        if (currentState != GameState.Preparacion) return;

        currentState = GameState.Combate;

        if (startWaveButton != null) startWaveButton.gameObject.SetActive(false);
        if (waveSpawner != null) waveSpawner.gameObject.SetActive(true);
    }

    private void EnemigoAparecio()
    {
        enemigosVivos++;
        enemigosSpawneados++;
    }

    private void EnemigoMurio(GameObject enemigo)
    {
        enemigosVivos--;
        revisarVictoriaPendiente = true;
    }

    private void OleadaTerminoDeSpawnear()
    {
        oleadaTerminada = true;
        revisarVictoriaPendiente = true;
    }

    // La revision se hace al final del frame, no en el mismo instante de la muerte.
    // Motivo: el divisor spawnea sus crias como reaccion al evento EnemyDied,
    // y si revisaramos en el acto, el contador tocaria cero un instante antes
    // de que las crias se registren y el juego daria la victoria de mas.
    private void LateUpdate()
    {
        if (!revisarVictoriaPendiente) return;

        revisarVictoriaPendiente = false;
        RevisarVictoria();
    }

    private void RevisarVictoria()
    {
        if (currentState != GameState.Combate) return;
        if (enemigosSpawneados <= 0) return;

        if (oleadaTerminada && enemigosVivos <= 0)
        {
            Victory();
        }
    }

    public void Victory()
    {
        if (currentState == GameState.GameOver) return;

        currentState = GameState.GameOver;
        Time.timeScale = 0f;
        if (winPanel != null) winPanel.SetActive(true);
    }

    public void GameOver()
    {
        if (currentState == GameState.GameOver) return;

        currentState = GameState.GameOver;
        Time.timeScale = 0f;
        if (losePanel != null) losePanel.SetActive(true);
    }
}