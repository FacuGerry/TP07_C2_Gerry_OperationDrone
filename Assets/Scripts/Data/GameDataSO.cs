using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "GameSettings/GameData")]

public class GameDataSO : ScriptableObject
{
    // Warning: estos valores son mutados en runtime por LevelManager.BuffEnemiesAndReload (minEnemies++/maxEnemies++).
    // Como los ScriptableObject persisten entre Play sessions en el editor, los valores se acumulan en cada playtest.
    public int minEnemies;
    public int maxEnemies;
}
