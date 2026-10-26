using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour

{

    [SerializeField] private float _movementSpeed = 4.5f;

    [SerializeField] private float _jumpHeight = 2;

    [SerializeField] private Transform _groundSensor;

    private InputAction _pauseAction;

    private Rigidbody2D _rigidbody2D;

    private Animator _animator;

    private InputAction _moveAction;

    private Vector2 _moveInput;

    private InputAction _jumpAction;

    private InputAction _attackAction;

    [SerializeField] private float _sensorSize = 1;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private int _attackDamage = 7;
    [SerializeField] private Transform _attackHitBox;
    [SerializeField] private float _hitBoxRadius = 1f;

    [SerializeField] private AudioClip _jumpSound;
    [SerializeField] private AudioClip _attackSound;

    //---- AÑADIDO ----
    [SerializeField] private AudioClip _hurtSound;
    //---- FIN AÑADIDO ----

    //---- AÑADIDO ----
    [SerializeField] private AudioClip _deathSound;
    //---- FIN AÑADIDO ----

    //---- AÑADIDO ----
    [SerializeField] private float _invulnerableTime = 1f;
    private float _invulnerableTimer = 0f;
    //---- FIN AÑADIDO ----

    //---- AÑADIDO ----
    [SerializeField] private float _fallLimit = -10f;
    //---- FIN AÑADIDO ----

    //---- AÑADIDO ----
    private bool _isDead = false;
    //---- FIN AÑADIDO ----

    // ____añadido
    [SerializeField] private float _attackLockTime = 0.4f;
    private float _attackLockTimer = 0f;
    // ______finalizado

    private AudioSource _playerAudioSource;

    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private int _actualHealth;

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _playerAudioSource = GetComponent<AudioSource>();


        _moveAction = InputSystem.actions["Move"];
        _jumpAction = InputSystem.actions["Jump"];
        _attackAction = InputSystem.actions["Attack"];
        _pauseAction = InputSystem.actions["Pause"];
    }

    void Start()
    {
        //---- CAMBIADO ---- (antes tenía // delante)
        _actualHealth = _maxHealth;
        //---- FIN CAMBIADO ----

        // ____añadido
        GameManager.Instance.UpdateHealthUI(_actualHealth, _maxHealth);
        // ______finalizado
    }

    void Update()
    {

        if (_pauseAction.WasPressedThisFrame())
        {
            GameManager.Instance.Pause();
        }

        if (GameManager.Instance.IsPaused())
        {
            return;
        }

        //---- AÑADIDO ----
        if (_isDead)
        {
            return;
        }
        //---- FIN AÑADIDO ----

        //---- AÑADIDO ----
        if (transform.position.y < _fallLimit)
        {
            Die();
            return;
        }
        //---- FIN AÑADIDO ----

        //---- AÑADIDO ----
        if (_invulnerableTimer > 0)
        {
            _invulnerableTimer -= Time.deltaTime;
        }
        //---- FIN AÑADIDO ----


        _moveInput = _moveAction.ReadValue<Vector2>();

        // ____añadido
        if (_attackLockTimer > 0)
        {
            _attackLockTimer -= Time.deltaTime;
            _moveInput = Vector2.zero;
        }
        // ______finalizado

        if (_moveInput.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            _animator.SetBool("IsRunning", true);
        }
        else if (_moveInput.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            _animator.SetBool("IsRunning", true);
        }
        else
        {
            _animator.SetBool("IsRunning", false);
        }


        if (_jumpAction.WasPressedThisFrame() && IsGrounded())
        {
            Jump();
        }

        if (_attackAction.WasPressedThisFrame() && IsGrounded())
        {
            Attack();
        }


        _animator.SetBool("IsJumping", !IsGrounded());
    }

    void FixedUpdate()
    {
        _rigidbody2D.linearVelocity = new Vector2(_moveInput.x * _movementSpeed, _rigidbody2D.linearVelocity.y);
    }

    void Jump()
    {
        _rigidbody2D.AddForce(Vector2.up * Mathf.Sqrt(_jumpHeight * -2 * Physics2D.gravity.y), ForceMode2D.Impulse);
        // _playerAudioSource.PlayOneShot(_jumpSound); 
        PlaySFX(_jumpSound);
    }

    void Attack()
    {
        _animator.SetTrigger("IsAttacking");

        // ____añadido
        if (_moveInput.x == 0)
        {
            _attackLockTimer = _attackLockTime;
        }
        // ______finalizado

        //  _playerAudioSource.PlayOneShot(_attackSound); 
        PlaySFX(_attackSound);

        //---- CAMBIADO ---- (antes: OverlapCircleAll(_groundSensor.position, _sensorSize))
        Collider2D[] colliders2D = Physics2D.OverlapCircleAll(_attackHitBox.position, _hitBoxRadius);
        //---- FIN CAMBIADO ----

        foreach (Collider2D enemy in colliders2D)
        {
            if (enemy.gameObject.layer == 7)
            {
                Mimik enemyScript = enemy.GetComponent<Mimik>();
                enemyScript.TakeDamage(_attackDamage);
            }
        }
    }

    bool IsGrounded()
    {
        Collider2D[] colliders2D = Physics2D.OverlapCircleAll(_groundSensor.position, _sensorSize);

        foreach (Collider2D item in colliders2D)
        {
            if (item.gameObject.layer == 6)
            {
                return true;
            }

        }
        return false;
    }

    void PlaySFX(AudioClip clip)
    {
        // ____cambiado (antes: _playerAudioSource.PlayOneShot(clip);)
        AudioManager.Instance.PlaySFX(clip);
        // ______finalizado
    }

    public void AddHealth(int healAmount)
    {
        _actualHealth += healAmount;

        if (_actualHealth > _maxHealth)
        {
            _actualHealth = _maxHealth;
        }

        // ____añadido
        GameManager.Instance.UpdateHealthUI(_actualHealth, _maxHealth);
        // ______finalizado
    }

    //---- AÑADIDO ----
    public void TakeDamage(int damage)
    {
        if (_isDead || _invulnerableTimer > 0)
        {
            return;
        }

        _actualHealth -= damage;
        _invulnerableTimer = _invulnerableTime;

        // ____añadido
        GameManager.Instance.UpdateHealthUI(_actualHealth, _maxHealth);
        // ______finalizado

        if (_actualHealth <= 0)
        {
            Die();
        }
        else
        {
            PlaySFX(_hurtSound);
        }
    }
    //---- FIN AÑADIDO ----

    //---- AÑADIDO ----
    void Die()
    {
        _isDead = true;
        _actualHealth = 0;
        _moveInput = Vector2.zero;

        // ____añadido
        GameManager.Instance.UpdateHealthUI(_actualHealth, _maxHealth);
        // ______finalizado

        PlaySFX(_deathSound);

        GameManager.Instance.GameOver();
    }
    //---- FIN AÑADIDO ----

    //---- AÑADIDO ----
    public int GetHealth()
    {
        return _actualHealth;
    }
    //---- FIN AÑADIDO ----

    //---- AÑADIDO ----
    public int GetMaxHealth()
    {
        return _maxHealth;
    }
    //---- FIN AÑADIDO ----


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_groundSensor.position, _sensorSize);

        Gizmos.color = Color.yellow;
        //---- CAMBIADO ---- (antes: _groundSensor.position)
        Gizmos.DrawWireSphere(_attackHitBox.position, _hitBoxRadius);
        //---- FIN CAMBIADO ----
    }
}