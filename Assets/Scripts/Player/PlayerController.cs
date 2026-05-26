using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Keys")]
    [SerializeField] private KeyBindingsSO _keys;

    [Header("Movement")]
    [SerializeField] private StatsDataSO _data;

    // Warning: Una propiedad pública NO debería empezar con underscore;
    // el prefijo "_" es para campos privados. Debería llamarse "Rb" o "Rigidbody" en PascalCase.
    public Rigidbody _rb { get; private set; }

    private bool _isAlive;
    private bool _isPaused = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _isAlive = true;
    }

    private void OnEnable()
    {
        HealthSystem.OnPlayerDie += OnPlayerDie_StopMovement;
        PauseGame.OnPause += OnPause_PauseGame;
    }

    private void OnDisable()
    {
        HealthSystem.OnPlayerDie -= OnPlayerDie_StopMovement;
        PauseGame.OnPause -= OnPause_PauseGame;
    }

    private void FixedUpdate()
    {
        if (_isAlive && !_isPaused)
        {
            MovementHor();
            MovementVer();
            CheckSpeed();
        }
    }
    
    private void MovementHor()
    {
        Vector3 direction = Vector3.zero;

        if (Input.GetKey(_keys.forward))
            direction = new Vector3(transform.forward.x, 0, transform.forward.z);

        if (Input.GetKey(_keys.left))
            direction = new Vector3(-transform.right.x, 0, -transform.right.z);

        if (Input.GetKey(_keys.backward))
            direction = new Vector3(-transform.forward.x, 0, -transform.forward.z);

        if (Input.GetKey(_keys.right))
            direction = new Vector3(transform.right.x, 0, transform.right.z);

        _rb.AddForce(direction * _data.movementSpeedHor, ForceMode.Force);
    }

    private void MovementVer()
    {
        if (Input.GetKey(_keys.up))
            _rb.AddForce(Vector3.up * _data.movementSpeedVer, ForceMode.Force);

        if (Input.GetKey(_keys.down))
            _rb.AddForce(Vector3.down * _data.movementSpeedVer, ForceMode.Force);
    }

    // Error: cada if reescribe el Vector3 entero con sólo un eje clampeado. Si dos ejes (ej: x e z) superan maxSpeed.
    // Más limpio: usar Mathf.Clamp o Vector3.ClampMagnitude 
    private void CheckSpeed()
    {
        if (_rb.linearVelocity.x >= _data.maxSpeed)
            _rb.linearVelocity = new Vector3(_data.maxSpeed, _rb.linearVelocity.y, _rb.linearVelocity.z);

        if (_rb.linearVelocity.y >= _data.maxSpeed)
            _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, _data.maxSpeed, _rb.linearVelocity.z);

        if (_rb.linearVelocity.z >= _data.maxSpeed)
            _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, _rb.linearVelocity.y, _data.maxSpeed);

        if (_rb.linearVelocity.x <= -_data.maxSpeed)
            _rb.linearVelocity = new Vector3(-_data.maxSpeed, _rb.linearVelocity.y, _rb.linearVelocity.z);

        if (_rb.linearVelocity.y <= -_data.maxSpeed)
            _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, -_data.maxSpeed, _rb.linearVelocity.z);

        if (_rb.linearVelocity.z <= -_data.maxSpeed)
            _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, _rb.linearVelocity.y, -_data.maxSpeed);
    }

    private void OnPlayerDie_StopMovement()
    {
        _isAlive = false;
    }

    private void OnPause_PauseGame(bool isPaused)
    {
        _isPaused = isPaused;
    }
}