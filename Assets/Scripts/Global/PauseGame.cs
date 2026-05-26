using System;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public static event Action<bool> OnPause;
    [SerializeField] private KeyBindingsSO _keys;
    private bool _isPaused = false;

    private void Start()
    {
        _isPaused = false;
        Time.timeScale = 1.0f;
        OnPause?.Invoke(_isPaused);
    }

    private void OnEnable()
    {
        UiPauseMenu.OnBackClicked += OnBackClicked_Unpause;
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(_keys.pause) || Input.GetKeyDown(_keys.pause2) || Input.GetKeyDown(_keys.pause3))
            ChangePause();
    }

    private void OnDisable()
    {
        // Error: acá se está suscribiendo (+=) en vez de desuscribir (-=). Debería ser "-= OnBackClicked_Unpause".
        UiPauseMenu.OnBackClicked += OnBackClicked_Unpause;
    }

    private void ChangePause()
    {
        _isPaused = !_isPaused;
        OnPause?.Invoke(_isPaused);
        Time.timeScale = _isPaused ? 0.0f : 1.0f;
    }

    private void OnBackClicked_Unpause()
    {
        ChangePause();
    }
}