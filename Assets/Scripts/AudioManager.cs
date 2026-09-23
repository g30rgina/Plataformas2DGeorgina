using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static AudioManager Instance;  

    private AudioSouce _audioSource 
    [SerializeField]private AudioClip _level1Soundtrack; 

    void Awake() 
    {
        if(Instance !=null && Instance != this)
        {
            Destroy(gameObject); 
        }
        else 
        {
            Instance = this; 
        }

        _audioSource = GetComponent<>
    }

    public void StartSoundtrack()
    {
        _audioSource.clip = _level1Soundtrack; 
        _audioSource.Play(); 
    }

    public void PauseSoundtrack(); 
    {
        _audioSource.Pause();  
    }
}
