using UnityEngine;

[CreateAssetMenu(fileName = "NpcData", menuName = "Npc/NpcData")]

public class StatsDataSO : ScriptableObject
{
    // Warning: level es incrementado por LevelManager en runtime; al persistir el SO entre Play sessions, queda acumulado.
    public int level;

    [Header("Stats")]
    public int life;
    public float speed;

    [Header("Player")]
    public float movementSpeedHor;
    public float movementSpeedVer;
    public float maxSpeed;

    [Header("Attacking")]
    // Warning: shootingSpeed se usa con doble rol en NpcController (velocidad de la bala Y cadencia del WaitForSeconds).
    public float distanceToShoot;
    public float shootingSpeed;
    public int shootingDamage;
}