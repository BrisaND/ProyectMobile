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

    public void IniciarFasePreparacion()
    {
        currentState = GameState.Preparacion;
        Time.timeScale = 1f;

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

    private void Update()
    {
        if (currentState == GameState.Combate)
        {
            if (waveSpawner != null && waveSpawner.waveFinishedSpawning && EnemyPool.Instance != null && EnemyPool.Instance.GetActiveEnemies().Count == 0)
            {
                Victory();
            }
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