using System;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    public static event Action<int> OnScoreUpdated;

    [SerializeField] private int _pointsForEnemy = 100;
    [SerializeField] private int _pointsForCitizen = 50;
    [SerializeField] private ScoreSO _data;

    private void Start()
    {
        OnScoreUpdated?.Invoke(_data.score);
    }

    private void OnEnable()
    {
        NpcHealthSystem.OnNpcDie += OnNpcKilled_ChangeScore;
    }

    private void OnDisable()
    {
        NpcHealthSystem.OnNpcDie -= OnNpcKilled_ChangeScore;
    }

    private void OnNpcKilled_ChangeScore(bool isEnemy)
    {
        // Warning: _data.score se está escribiendo en un ScriptableObject. Persiste entre seciones
        if (isEnemy)
            _data.score += _pointsForEnemy;
        else
            _data.score -= _pointsForCitizen;

        if (_data.score >= _data.maxScore)
            _data.score = _data.maxScore;

        OnScoreUpdated?.Invoke(_data.score);
    }
}
