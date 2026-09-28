using UnityEngine;

public class Heart : MonoBehaviour 

{ 

    private AudioSource _heartAudioSource; 
    private SpriteRenderer _spriteRenderer; 
    private Collider2D _collider;  

    
    [SerializeField] private int healAmount = 10; 

    [SerializeField] private AudioClip _hearthSound; 
    
    void PlaySFX() 

    {
     _heartAudioSource.PlayOneShot(_hearthSound);
    }

    void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            PlayerController _playerScript = collision.GetComponent<PlayerController>(); 
            _playerScript.AddHealth(healAmount); 

        PlaySFX();
            _spriteRenderer.enabled = false; 
            _collider.enabled = false; 
            Destroy(gameObject, 0.5f);
        } 
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    

}
