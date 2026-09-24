using UnityEngine;

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

    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}