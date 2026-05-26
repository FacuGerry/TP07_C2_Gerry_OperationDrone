using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public static event Action OnPlayerShoot;
    public static event Action OnPlayerSecondShoot;

    [SerializeField] private KeyBindingsSO _keys;
    [SerializeField] private Transform _shootingPos;
    [SerializeField] private float _normalBulletDistance;

    [Header("Second bullet stats")]
    [SerializeField] private List<GameObject> _bullets = new List<GameObject>();
    [SerializeField] private float _bulletDuration;
    [SerializeField] private float _bulletDistance;
    [SerializeField] private int _bulletDamage;
    [SerializeField] private GameObject _cheatLine;

    private List<BulletMovement> _bulletMovements = new List<BulletMovement>();
    private bool _isShooting = false;
    private bool _startedShooting = false;

    private bool _isPaused = false;
    // Suggestion: typo "corroutine" → "coroutine". Aparece repetido en varios scripts del proyecto.
    private IEnumerator _corroutineShoot;

    private void Awake()
    {
        foreach (GameObject bullet in _bullets)
            _bulletMovements.Add(bullet.GetComponent<BulletMovement>());
    }

    private void OnEnable()
    {
        PauseGame.OnPause += OnPause_PauseGame;
    }

    private void Update()
    {
        if (!_isPaused)
        {
            if (Input.GetKey(_keys.shoot))
                _isShooting = true;

            if (Input.GetKeyDown(_keys.secondShoot))
                SecondShoot();

            if (Input.GetKeyDown(_keys.showCheat))
                ShowCheat();

            if (Input.GetKeyUp(_keys.shoot))
            {
                _isShooting = false;
                _startedShooting = false;
            }

            if (_isShooting && !_startedShooting)
            {
                _startedShooting = true;
                if (_corroutineShoot != null)
                    StopCoroutine(_corroutineShoot);

                _corroutineShoot = Shooting();
                StartCoroutine(_corroutineShoot);
            }

            // Warning: este bloque se ejecuta TODOS los frames mientras no se esté disparando. Llama a StopCoroutine
            // sobre una coroutine que ya está frenada cientos de veces por segundo. Unity lo tolera pero es trabajo en vano.
            // Mover el StopCoroutine al evento GetKeyUp del shoot (que ya está arriba).
            if (!_isShooting)
                if (_corroutineShoot != null)
                    StopCoroutine(_corroutineShoot);
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
            OnPlayerShoot?.Invoke();
            RaycastHit ray;
            // Warning: _shootingPos ya es un Transform; ".transform" es redundante. Usar directamente _shootingPos.position.
            if (Physics.Raycast(_shootingPos.transform.position, transform.forward, out ray, _normalBulletDistance))
                if (ray.collider != null && ray.collider.TryGetComponent(out NpcHealthSystem npc))
                {
                    npc.OnNormalShot_TakeDamage(_bulletDamage);
                    // Warning: Debug.Log dentro de la coroutine de disparo. Cada hit lo loguea.
                    Debug.Log("Hit an NPC");
                }
            // Warning: magic number 0.3f como cadencia del disparo. Debería ser un campo serializado configurable y no un magic number!!
            yield return new WaitForSeconds(0.3f);
        }
    }

    private void SecondShoot()
    {
        for (int i = 0; i < _bullets.Count; i++)
        {
            if (!_bullets[i].activeInHierarchy)
            {
                // NEW METHOD
                _bullets[i].SetActive(true);
                _bulletMovements[i].Shoot(_shootingPos.position, _bulletDistance, _bulletDuration, gameObject);
                OnPlayerSecondShoot?.Invoke();
                return;
            }
        }
    }

    private void ShowCheat()
    {
        _cheatLine.SetActive(!_cheatLine.activeInHierarchy);
    }

    private void OnPause_PauseGame(bool isPaused)
    {
        _isPaused = isPaused;
    }
}
