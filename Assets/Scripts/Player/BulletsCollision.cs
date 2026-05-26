using UnityEngine;

// Error: el nombre de la clase está mal escrito ("BuleltsCollision") y NO coincide con el nombre del archivo ("BulletsCollision.cs").
// Unity tira warning de mismatch entre nombre de archivo y nombre de clase.
// Suggestion: el bool _isPlayerBullet es un "boolean parameter is a code smell". Si crecen los tipos de bala, se vuelve un if/else
// en cascada. Mejor dos componentes separados (PlayerBulletCollision / EnemyBulletCollision) o un enum BulletOwner.
public class BuleltsCollision : MonoBehaviour
{
    [SerializeField] private StatsDataSO _data;
    [SerializeField] private bool _isPlayerBullet;

    private void OnTriggerEnter(Collider collision)
    {
        if (_isPlayerBullet)
        {
            if (collision.gameObject.TryGetComponent(out NpcHealthSystem npc))
                npc.OnBulletShot_TakeDamage(_data.shootingDamage);
        }
        else
        {
            if (collision.gameObject.TryGetComponent(out HealthSystem player))
                player.TakeDamage(_data.shootingDamage + _data.level);
        }

        gameObject.SetActive(false);
    }
}