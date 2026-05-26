using UnityEngine;
using UnityEngine.AI;

public class StateWalk : EnemyStates
{
    private int _index = 0;

    public override void Initialize(Animator animator, Rigidbody rigidbody, NpcController npcPatrol, NavMeshAgent agent, GameObject player)
    {
        base.Initialize(animator, rigidbody, npcPatrol, agent, player);
        state = StateType.Walk;
    }
    
    public override void OnEnter()
    {
        base.OnEnter();
        _agent.SetDestination(_patrol.positions[_index]);
        _anim.SetInteger(_state, (int)state);
    }

    public override void OnUpdate()
    {
        float distance = Vector3.Distance(_patrol.transform.position, _patrol.positions[_index]);
        // Warning: magic number 0.5f como umbral de "llegó al waypoint". Extraer a constante o campo serializado.
        if (distance < 0.5f)
        {
            _index++;
            if (_index > _patrol.positions.Count - 1)
                _index = 0;
            _agent.SetDestination(_patrol.positions[_index]);
        }
    }
}
