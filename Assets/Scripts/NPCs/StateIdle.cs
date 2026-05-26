using UnityEngine;
using UnityEngine.AI;

// Warning: StateIdle se agrega al list de estados y se entra en el Awake de NpcController, pero CheckForPlayer
// sólo alterna entre Walk y Shoot — nunca se vuelve a Idle. Tampoco setea el parámetro del Animator como Walk/Shoot.
// Si Idle no se va a usar, sacarlo del enum y del list para evitar confusión.
public class StateIdle : EnemyStates
{
    public override void Initialize(Animator animator, Rigidbody rigidbody, NpcController npcPatrol, NavMeshAgent agent, GameObject player)
    {
        base.Initialize(animator, rigidbody, npcPatrol, agent, player);
        state = StateType.Idle;
    }
}
