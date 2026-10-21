using UnityEngine;

public class Mimik : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 20;
    private int _actualHealth;

    //---- AÑADIDO ----
    [SerializeField] private int _damage = 10;
    //---- FIN AÑADIDO ----

    //---- AÑADIDO ----
    private AudioSource _mimikAudioSource;
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;
    //---- FIN AÑADIDO ----

    //---- AÑADIDO ----
    [SerializeField] private AudioClip _mimikSound;
    [SerializeField] private AudioClip _dieSound;
    //---- FIN AÑADIDO ----

    //---- AÑADIDO ----
    void Awake()
    {
        _mimikAudioSource = GetComponent<AudioSource>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
    }
    //---- FIN AÑADIDO ----

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _actualHealth = _maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        _actualHealth -= damage;

        if (_actualHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        //---- CAMBIADO ---- (antes solo tenía: Destroy(gameObject);)
        _mimikAudioSource.PlayOneShot(_dieSound);
        _spriteRenderer.enabled = false;
        _collider.enabled = false;
        Destroy(gameObject, 0.5f);
        //---- FIN CAMBIADO ----
    }

    //---- AÑADIDO ----
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _mimikAudioSource.PlayOneShot(_mimikSound);
        }
    }
    //---- FIN AÑADIDO ----

    //---- AÑADIDO ----
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerScript = collision.gameObject.GetComponent<PlayerController>();
            playerScript.TakeDamage(_damage);
        }
    }
    //---- FIN AÑADIDO ----
}