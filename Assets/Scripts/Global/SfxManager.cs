using UnityEngine;

public class SfxManager : MonoBehaviour
{
    // Suggestion: singleton expuesto como campo público static. Convención más segura:
    // "public static SfxManager Instance { get; private set; }" para que nadie de afuera pueda sobreescribirlo.
    public static SfxManager instance;

    [Header("Sources")]
    [SerializeField] private AudioSource _sfx;
    [SerializeField] private AudioSource _ui;

    [Header("Player")]
    [SerializeField] private AudioClip _playerShoot;
    [SerializeField] private AudioClip _playerSecondShoot;
    [SerializeField] private AudioClip _playerDamaged;
    [SerializeField] private AudioClip _playerDie;

    [Header("Enemies")]
    [SerializeField] private AudioClip _enemyShoot;
    [SerializeField] private AudioClip _enemyDamaged;
    [SerializeField] private AudioClip _enemyDie;

    [Header("UI")]
    [SerializeField] private AudioClip _btnHover;
    [SerializeField] private AudioClip _btnClick;
    
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        HealthSystem.OnPlayerDamaged += OnPlayerDamaged_PlayClip;
        HealthSystem.OnPlayerDie += OnPlayerDie_PlayClip;

        NpcHealthSystem.OnNpcDamaged += OnNpcDamaged_PlayClip;
        NpcHealthSystem.OnNpcDie += OnNpcDie_PlayClip;

        PlayerShoot.OnPlayerShoot += OnPlayerShoot_PlayClip;
        PlayerShoot.OnPlayerSecondShoot += OnPlayerSecondShoot_PlayClip;

        NpcController.OnNpcShoot += OnEnemyShoot_PlayClip;

        // Warning: la clase referenciada se llama UiButtonHoverSFXEvent pero el archivo es UiBtnHoverSFXEvent.cs.
        // Unity tira warning por el mismatch entre nombre de archivo y nombre de clase.
        UiButtonHoverSFXEvent.OnButtonHover += OnButtonHover_PlayClip;
        UiButtonHoverSFXEvent.OnButtonClick += OnButtonClick_PlayClip;
    }

    private void OnDisable()
    {
        HealthSystem.OnPlayerDamaged -= OnPlayerDamaged_PlayClip;
        HealthSystem.OnPlayerDie -= OnPlayerDie_PlayClip;

        NpcHealthSystem.OnNpcDamaged -= OnNpcDamaged_PlayClip;
        NpcHealthSystem.OnNpcDie -= OnNpcDie_PlayClip;

        PlayerShoot.OnPlayerShoot -= OnPlayerShoot_PlayClip;
        PlayerShoot.OnPlayerSecondShoot -= OnPlayerSecondShoot_PlayClip;

        NpcController.OnNpcShoot -= OnEnemyShoot_PlayClip;

        UiButtonHoverSFXEvent.OnButtonHover -= OnButtonHover_PlayClip;
        UiButtonHoverSFXEvent.OnButtonClick -= OnButtonClick_PlayClip;
    }

    private void OnPlayerShoot_PlayClip()
    {
        _sfx.PlayOneShot(_playerShoot);
    }

    private void OnPlayerSecondShoot_PlayClip()
    {
        _sfx.PlayOneShot(_playerSecondShoot);
    }

    private void OnEnemyShoot_PlayClip()
    {
        _sfx.PlayOneShot(_enemyShoot);
    }

    private void OnPlayerDamaged_PlayClip()
    {
        _sfx.PlayOneShot(_playerDamaged);
    }

    private void OnPlayerDie_PlayClip()
    {
        _sfx.PlayOneShot(_playerDie);
    }

    private void OnNpcDamaged_PlayClip()
    {
        _sfx.PlayOneShot(_enemyDamaged);
    }

    private void OnNpcDie_PlayClip(bool isEnemy)
    {
        _sfx.PlayOneShot(_enemyDie);
    }

    private void OnButtonHover_PlayClip()
    {
        _ui.PlayOneShot(_btnHover);
    }

    private void OnButtonClick_PlayClip()
    {
        _ui.PlayOneShot(_btnClick);
    }
}
