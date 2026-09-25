using UnityEngine;

// Ahora el audio se entera solo. Nadie le pide que suene:
// escucha los eventos del juego y responde.
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Efectos de Sonido")]
    public AudioClip shootSound;
    public AudioClip hitSound;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnEnable()
    {
        EventManager.Subscribe(GameEvents.TurretShot, SonarDisparo);
        EventManager.Subscribe(GameEvents.EnemyDamaged, SonarImpacto);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(GameEvents.TurretShot, SonarDisparo);
        EventManager.Unsubscribe(GameEvents.EnemyDamaged, SonarImpacto);
    }

    private void SonarDisparo()
    {
        PlaySound(shootSound);
    }

    private void SonarImpacto()
    {
        PlaySound(hitSound);
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }
}