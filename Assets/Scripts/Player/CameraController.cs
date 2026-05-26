using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Keys")]
    [SerializeField] private KeyBindingsSO _keys;

    [Header("Rotation")]
    [SerializeField] private float _rotationSpeedHor = 750;
    [SerializeField] private float _rotationSpeedVer = 750;
    [SerializeField] private float _rotationMinVer = -25;
    [SerializeField] private float _rotationMaxVer = 75;

    [Header("Camera Positions")]
    [SerializeField] private Transform _cameraPos1stPerson;
    [SerializeField] private Transform _cameraPos3rdPerson;

    [Header("Camera")]
    [SerializeField] private GameObject _camera;

    [Header("Camera Movement")]
    [SerializeField] private float _time;

    private bool _isChanging = false;
    private bool _canMove = true;
    private bool _isPaused = false;

    private void Awake()
    {
        transform.localEulerAngles = Vector3.zero;
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

    private void Update()
    {
        if (_canMove && !_isPaused)
        {
            if (Input.GetKeyUp(_keys.changePOV))
                _isChanging = !_isChanging;

            ChangePOV();
            Rotation();
        }
    }

    private void ChangePOV()
    {
        if (_isChanging)
        {
            if (_camera.transform != _cameraPos1stPerson)
            {
                _camera.transform.position = Vector3.Lerp(_camera.transform.position, _cameraPos1stPerson.position, _time * Time.deltaTime);
                _camera.transform.localEulerAngles = Vector3.Lerp(_camera.transform.localEulerAngles, _cameraPos1stPerson.localEulerAngles, _time * Time.deltaTime);
            }
        }
        else
        {
            if (_camera.transform != _cameraPos3rdPerson)
            {
                _camera.transform.position = Vector3.Lerp(_camera.transform.position, _cameraPos3rdPerson.position, _time * Time.deltaTime);
                _camera.transform.localEulerAngles = Vector3.Lerp(_camera.transform.localEulerAngles, _cameraPos3rdPerson.localEulerAngles, _time * Time.deltaTime);
            }
        }
    }

    private void Rotation()
    {
        Vector3 rotation = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);

        rotation.x *= _rotationSpeedVer;
        rotation.y *= _rotationSpeedHor;
        rotation *= Time.deltaTime;

        transform.localEulerAngles += rotation;

        float rotationX = transform.localEulerAngles.x;

        // Error: la condición es imposible. Con _rotationMaxVer=75 y _rotationMinVer=-25, ningún número puede ser
        // simultáneamente mayor a 75 Y menor a -25. El operador && debería ser ||. El clamp vertical NUNCA se ejecuta.
        if ((rotationX > _rotationMaxVer) && (rotationX < _rotationMinVer))
        {
            if ((rotationX - _rotationMaxVer) < (_rotationMinVer - rotationX))
                rotationX = _rotationMaxVer;
            else
                rotationX = _rotationMinVer;

            transform.localEulerAngles = new Vector3(rotationX, transform.localEulerAngles.y, 0);
        }
    }

    private void OnPlayerDie_StopMovement()
    {
        _canMove = false;
    }

    private void OnPause_PauseGame(bool isPaused)
    {
        _isPaused = isPaused;
    }
}
