using System;
using UnityEngine;

// Suggestion: esta clase y HealthSystem (del Player) tienen lógica casi idéntica (vida, daño, evento Damaged, evento Die).
// Extraer una clase base abstracta o una interface IDamageable para evitar duplicación.
public class NpcHealthSystem : MonoBehaviour
{
    public static event Action<bool> OnNpcDie;
    public static event Action OnNpcDamaged;

    [SerializeField] private StatsDataSO _data;
    // Warning: Baja - _player es [SerializeField] pero NUNCA se lee. Eliminar el campo o usarlo donde corresponda.
    [SerializeField] private PlayerShoot _player;

    private NpcController _controller;
    private int _life;

    private void Awake()
    {
        _controller = GetComponent<NpcController>();
    }

    private void Start()
    {
        // Warning: (_data.level / 10) es división entera. Hasta level=9 da 0 → factor 1 (vida normal).
        _life = _data.life * ((_data.level / 10) + 1);
    }

    private void TakeDamage(int damage)
    {
        OnNpcDamaged?.Invoke();
        _life -= damage;
        if (_life <= 0)
        {
            _life = 0;
            NpcDie();
        }
    }

    public void OnNormalShot_TakeDamage(int damage)
    {
        TakeDamage(damage);
    }

    public void OnBulletShot_TakeDamage(int damage)
    {
        TakeDamage(damage);
    }

    private void NpcDie()
    {
        // Warning: Debug.Log en runtime. Envolver en #if UNITY_EDITOR o quitar para build.
        if (_controller.isEnemy)
            Debug.Log("Killed an enemy");
        else
            Debug.Log("Killed a citizen");

        OnNpcDie?.Invoke(_controller.isEnemy);

        gameObject.SetActive(false);
    }
}