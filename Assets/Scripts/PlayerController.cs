using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour

{ 

   // [SerializeField]private int _maxHealth = 100; 

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



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    [SerializeField] private AudioClip _jumpSound; 
    [SerializeField] private AudioClip _attackSound;
    [SerializeField] private AudioClip _hurtSound;
    [SerializeField] private AudioClip _deathSound;

    //------------------------añadido nuevo 
    [SerializeField] private _invulneableTime = 1f; 
    private float _invulnerableTimer = 0f;

    //------------------------añadido nuevo 
    [SerializeField] private _invulneableTime = 1f; 
    private float _invulnerableTimer = 0f;

    [SerializeField] private float _fallLimit = -10f;

    private bool _isDead = false;
    //------------------------añadido nuevo 


    private AudioSource _playerAudioSource; 

     [SerializeField] private int _maxHealth = 100; 
     [SerializeField] private int _actualHealth;
   
    void Awake()
    {
         _rigidbody2D = GetComponent <Rigidbody2D>();  
         _animator = GetComponent <Animator>();  
         _playerAudioSource = GetComponent<AudioSource>();


         _moveAction = InputSystem.actions["Move"];  
         _jumpAction = InputSystem.actions["Jump"];  
         _attackAction = InputSystem.actions["Attack"];   
         _pauseAction = InputSystem.actions["Pause"];   
    }

      void Start()
    {
        //_actualHealth = _maxHealth; 
    }
    // Update is called once per frame

    void Update()
    {

          if(_pauseAction.WasPressedThisFrame()) 
        {  
            GameManager.Instance.Pause(); 
        }  

        if(GameManager.Instance.IsPaused()) 
        {  
            return; 
        }

        //------------------------añadido nuevo 
        if (_isDead)
        {
            return;
        }

        if (transform.position.y < _fallLimit)
        {
            Die();
            return;
        }

        if (_invulnerableTimer > 0)
        {
            _invulnerableTimer -= Time.deltaTime;
        }

         //------------------------añadido nuevo  

        _moveInput = _moveAction.ReadValue<Vector2>(); 

        if(_moveInput.x < 0)
        { 
            transform.rotation = Quaternion.Euler(0, 180, 0);
            _animator.SetBool("IsRunning", true); 
        }
        else if(_moveInput.x > 0) 
        { 
            transform.rotation = Quaternion.Euler(0, 0, 0); 
            _animator.SetBool("IsRunning", true); 
        }
        else 
        { 
            _animator.SetBool("IsRunning", false); 
        }


        if(_jumpAction.WasPressedThisFrame() && IsGrounded())
        {  
            Jump();
        } 

        if(_attackAction.WasPressedThisFrame() && IsGrounded())
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

      //  _playerAudioSource.PlayOneShot(_attackSound); 
        PlaySFX(_attackSound);


        //------------------------añadido nuevo  
        Collider2D[] colliders2D = Physics2D.OverlapCircleAll(_attackHitBox.position, _hitBoxRadius);
        //------------------------añadido nuevo  
        foreach (Collider2D enemy in colliders2D)
        { 
          if(enemy.gameObject.layer == 7)  
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
            if(item.gameObject.layer == 6) 
            {  
                return true;
            }
      
        }  
        return false; 

   
       
    }
     void PlaySFX(AudioClip clip) 
    { 
       _playerAudioSource.PlayOneShot(clip);     
    }  

    public void AddHealth(int healAmount) 
    { 
       _actualHealth += healAmount; 

       if(_actualHealth > _maxHealth)  
        { 
         _actualHealth = _maxHealth; 
        }   

    }
    //------------------------añadido nuevo  
    public void TakeDamage(int damage)
        {
        if (_isDead || _invulnerableTimer > 0)
        {
            return;
        }

        _actualHealth -= damage;
        _invulnerableTimer = _invulnerableTime;

        if (_actualHealth <= 0)
        {
            Die();
        }
        else
        {
            PlaySFX(_hurtSound);
        }
    }

    void Die()
    {
        _isDead = true;
        _actualHealth = 0;
        _moveInput = Vector2.zero;

        PlaySFX(_deathSound);

        GameManager.Instance.GameOver();
    }

    public int GetHealth()
    {
        return _actualHealth;
    }

    public int GetMaxHealth()
    {
        return _maxHealth;
    }
    //------------------------añadido nuevo  
    


    void OnDrawGizmos()
        { 
            Gizmos.color = Color.red; 
            Gizmos.DrawWireSphere(_groundSensor.position, _sensorSize);  

            Gizmos.color = Color.yellow; 
            Gizmos.DrawWireSphere(_groundSensor.position, _hitBoxRadius);  
        }  
}  