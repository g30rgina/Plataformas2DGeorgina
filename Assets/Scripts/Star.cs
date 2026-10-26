// ____añadido (script nuevo entero, igual que Coin pero suma estrellas)
using UnityEngine;

public class Star : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private CircleCollider2D _collider;

    [SerializeField] private AudioClip _starAudio;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<CircleCollider2D>();
    }

    void PlaySFX()
    {
        AudioManager.Instance.PlaySFX(_starAudio);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlaySFX();
            GameManager.Instance.AddStar();
            _spriteRenderer.enabled = false;
            _collider.enabled = false;
            Destroy(gameObject, 0.5f);
        }
    }
}
// ______finalizado