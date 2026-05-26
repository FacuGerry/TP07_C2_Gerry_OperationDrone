using System;
using UnityEngine;

// Suggestion: esta clase y NpcHealthSystem tienen lógica casi idéntica (vida, daño, eventos Damaged/Die).
// Extraer una clase base abstracta HealthBase o una interface IDamageable para evitar duplicación.
public class HealthSystem : MonoBehaviour
{
    public static event Action<int, int> OnUpdateLife;
    public static event Action OnPlayerDamaged;
    public static event Action OnPlayerDie;

    [SerializeField] private StatsDataSO _data;

    private int _maxHealth;
    private int _health;

    private void Start()
    {
        _maxHealth = _data.life;
        _health = _maxHealth;
        OnUpdateLife?.Invoke(_health, _maxHealth);
    }

    private void OnEnable()
    {
        CollisionController.OnPlayerCrashed += OnPlayerCrashed_ReduceHealth;
    }

    private void OnDisable()
    {
        CollisionController.OnPlayerCrashed -= OnPlayerCrashed_ReduceHealth;
    }
    
    private void OnPlayerCrashed_ReduceHealth(int damage)
    {
        ChangeHealth(damage);
    }

    public void TakeDamage(int damage)
    {
        ChangeHealth(damage);
    }

    private void ChangeHealth(int damage)
    {
        OnPlayerDamaged?.Invoke();
        _health -= damage;
        
        Debug.Log("Player damaged by " + damage + " points");
        if (_health <= 0)
        {
            _health = 0;
            OnPlayerDie?.Invoke();
        }
        OnUpdateLife?.Invoke(_health, _maxHealth);
    }
}
