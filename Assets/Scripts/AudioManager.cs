using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static AudioManager Instance;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip _level1Soundtrack;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        _audioSource = GetComponent<AudioSource>();
    }

    // ____añadido
    void Start()
    {
        StartSoundtrack();
    }
    // ______finalizado

    public void StartSoundtrack()
    {
        _audioSource.clip = _level1Soundtrack;
        _audioSource.Play();
    }

    public void PauseSoundtrack()
    {
        _audioSource.Pause();
    }

    // ____añadido
    public void ResumeSoundtrack()
    {
        _audioSource.UnPause();
    }

    // Para los efectos de sonido (moneda, salto, ataque...)
    public void PlaySFX(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }
    // ______finalizado
}