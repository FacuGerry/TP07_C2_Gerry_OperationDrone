using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcController : MonoBehaviour
{
    public static event Action OnNpcShoot;

    // Warning: campo público; debería ser [SerializeField] private con accessor o propiedad de sólo lectura.
    public List<Vector3> positions = new List<Vector3>();
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _placeToAim;
    [SerializeField] private Animator _anim;
    [SerializeField] private StatsDataSO _data;

    [Header("Gun")]
    [SerializeField] private GameObject _weapon;
    [SerializeField] private Transform _walkPos;
    [SerializeField] private Transform _shootPos;

    [Header("Bullets")]
    [SerializeField] private Transform _bulletShootPos;

    private List<EnemyStates> _states = new List<EnemyStates>();
    private EnemyStates currentState;
    private Rigidbody _rb;
    private NavMeshAgent _agent;

    // Warning: campo público; debería ser propiedad con getter público y setter privado/serializado.
    public bool isEnemy;

    private bool _isShooting;

    private float _shootingSpeed;

    // Suggestion: typo "corroutine" → "coroutine".
    private IEnumerator _corroutineShoot;
    private bool _isPaused = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _agent = GetComponent<NavMeshAgent>();

        _states.Add(new StateIdle());
        _states.Add(new StateWalk());
        _states.Add(new StateShoot());

        foreach (EnemyStates state in _states)
            state.Initialize(_anim, _rb, this, _agent, _player);

        currentState = FindState(StateType.Idle);
        currentState.OnEnter();

    }

    private void Start()
    {
        transform.position = new Vector3(positions[0].x, transform.position.y, positions[0].z);

        SwitchState(FindState(StateType.Walk));

        _agent.speed = _data.speed;
        // Warning: (_data.level / 10) es división entera. Hasta nivel 9 el resto es 0 (sin cambio). Correcto era: (_data.level / 10.0f)
        // Recién en nivel 10 cambia en 1 — los "saltos" son escalonados, no graduales. Si se busca rampa suave, usar float.
        _shootingSpeed = _data.shootingSpeed - (_data.level / 10);
    }

    private void OnEnable()
    {
        PauseGame.OnPause += OnPause_PauseGame;
    }

    private void Update()
    {
        if (!_isPaused)
        {
            if (currentState != null)
                currentState.OnUpdate();

            if (isEnemy)
                CheckForPlayer();

            MoveGun();
        }
    }

    private void OnDisable()
    {
        PauseGame.OnPause -= OnPause_PauseGame;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private IEnumerator Shooting()
    {
        while (_isShooting)
        {
            // Warning: GetPooledObject() puede devolver null y GetRigidbody(null) iterar sobre el pool sin necesidad.
            // Mejor pedir el bullet primero, chequear null y recién ahí pedir el Rigidbody.
            GameObject bullet = Bullets.instance.GetPooledObject();
            Rigidbody rb = Bullets.instance.GetRigidbody(bullet);
            if (bullet != null)
            {
                bullet.transform.position = _bulletShootPos.position;
                bullet.SetActive(true);

                Vector3 playerPos = _placeToAim.transform.position;
                Vector3 bulletDirection = (playerPos - bullet.transform.position).normalized;

                // Warning: _data.shootingSpeed se usa acá como velocidad y abajo como cadencia (WaitForSeconds).
                // El mismo número cumple dos roles distintos; imposible balancear cadencia sin cambiar velocidad.
                rb.linearVelocity = bulletDirection * _data.shootingSpeed;

                // Warning: Debug.Log dentro de la coroutine de disparo se ejecuta cada bala disparada. Con varios enemigos disparando seguido, inunda la consola y degrada performance.
                Debug.Log("Enemy shot a bullet to (" + bulletDirection.x + ", " + bulletDirection.y + ", " + bulletDirection.z + ")");
            }
            
            OnNpcShoot?.Invoke();
            yield return new WaitForSeconds(_shootingSpeed);
        }
        yield return null;
    }

    public void EnableShooting(bool isShooting)
    {
        if (isShooting)
        {
            // Error: el if tiene cuerpo vacío Debería ser "if (_corroutineShoot == null) { ...arrancar... }" y sin else.
            if (_corroutineShoot != null) { }
            else
            {
                _isShooting = true;

                _corroutineShoot = Shooting();
                StartCoroutine(_corroutineShoot);
            }
        }
        else
        {
            _isShooting = false;

            if (_corroutineShoot != null)
                StopCoroutine(_corroutineShoot);

            _corroutineShoot = null;
        }
    }

    private void CheckForPlayer()
    {
        if (Vector3.Distance(transform.position, _player.transform.position) <= _data.distanceToShoot)
            SwitchState(FindState(StateType.Shoot));
        else
            SwitchState(FindState(StateType.Walk));
    }
    
    private void SwitchState(EnemyStates newState)
    {
        if (currentState == newState)
            return;

        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter();
    }

    private EnemyStates FindState(StateType stateToFind)
    {
        foreach (EnemyStates state in _states)
            if (state.state == stateToFind)
                return state;

        return null;
    }

    // Warning: FindState es O(n) sobre _states y se llama DOS veces por frame desde acá (más una vez desde CheckForPlayer).
    // Cachear las referencias a cada estado en Awake (ej: _stateWalk, _stateShoot) o usar Dictionary<StateType, EnemyStates>.
    private void MoveGun()
    {
        if (currentState == FindState(StateType.Walk))
        {
            _weapon.transform.position = _walkPos.position;
            _weapon.transform.localEulerAngles = _walkPos.localEulerAngles;
        }
        if (currentState == FindState(StateType.Shoot))
        {
            _weapon.transform.position = _shootPos.position;
            _weapon.transform.localEulerAngles = _shootPos.localEulerAngles;
        }
    }

    private void OnPause_PauseGame(bool isPaused)
    {
        _isPaused = isPaused;
    }
}
