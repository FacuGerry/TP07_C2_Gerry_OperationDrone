using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private StatsDataSO _npcData;
    [SerializeField] private GameDataSO _gameData;
    [SerializeField] private string _sceneToLoad = "Gameplay";
    [SerializeField] private List<NpcController> _npcList = new List<NpcController>();

    // Warning: campo público; debería ser [SerializeField] private o propiedad con getter público.
    public int enemies = 0;

    // Suggestion: typo "corroutine" → "coroutine". Aparece repetido en varios scripts del proyecto.
    private IEnumerator _corroutineCreating;
    private void Start()
    {
        if (_corroutineCreating != null)
            StopCoroutine(_corroutineCreating);

        _corroutineCreating = CreatingEnemies();
        StartCoroutine(_corroutineCreating);
    }

    private void OnEnable()
    {
        NpcHealthSystem.OnNpcDie += OnNpcDie_CheckForWin;
    }

    private void OnDisable()
    {
        NpcHealthSystem.OnNpcDie -= OnNpcDie_CheckForWin;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private IEnumerator CreatingEnemies()
    {
        // Suggestion: paréntesis innecesarios en (_gameData.maxEnemies + 1). Random.Range(int,int) ya es exclusivo en el max.
        int numberOfEnemies = Random.Range(_gameData.minEnemies, (_gameData.maxEnemies + 1));
        
        while (enemies < numberOfEnemies)
        {
            int randomEnemy = Random.Range(0, _npcList.Count);
            if (!_npcList[randomEnemy].isEnemy)
            {
                _npcList[randomEnemy].isEnemy = true;
                enemies++;
            }
            yield return null;
        }
        // Suggestion: Baja - este "yield return null" final no aporta nada. La coroutine ya terminó al salir del while.
        yield return null;
    }

    private void OnNpcDie_CheckForWin(bool isEnemy)
    {
        if (isEnemy)
            enemies--;

        if (enemies <= 0)
            BuffEnemiesAndReload();
    }

    private void BuffEnemiesAndReload()
    {
        // Error: _npcData.level, _gameData.minEnemies y _gameData.maxEnemies viven en ScriptableObjects.
        // Los SO persisten entre Play sessions en el editor, así que estos incrementos quedan acumulados entre playtests
        // (cada vez que entrás a Play, los enemigos arrancan más fuertes que la corrida anterior).
        // Hay que resetear estos valores al inicio del juego o mover el estado a un componente runtime.
        _npcData.level++;

        _gameData.minEnemies++;
        _gameData.maxEnemies++;

        if (_gameData.minEnemies > _npcList.Count)
            _gameData.minEnemies = _npcList.Count;

        if (_gameData.maxEnemies > _npcList.Count)
            _gameData.maxEnemies = _npcList.Count;

        SceneManager.LoadScene(_sceneToLoad);
    }
}