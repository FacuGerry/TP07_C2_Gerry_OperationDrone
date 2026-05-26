using UnityEngine;
using UnityEngine.AI;

// Warning: la clase está marcada como "partial" pero no existe otra declaración partial en el proyecto. El keyword sobra.
public abstract partial class EnemyStates
{
    public StateType state;
    protected Animator _anim;
    protected Rigidbody _rb;
    protected NpcController _patrol;
    protected NavMeshAgent _agent;
    protected GameObject _player;

    protected static readonly int _state = Animator.StringToHash("State");

    public virtual void Initialize(Animator animator, Rigidbody rigidbody, NpcController npcPatrol, NavMeshAgent agent, GameObject player)
    {
        _anim = animator;
        _rb = rigidbody;
        _patrol = npcPatrol;
        _agent = agent;
        _player = player;
    }

    public virtual void OnEnter()
    {
        // Warning: Debug.Log se ejecuta cada vez que un NPC cambia de estado. Con muchos NPCs en escena la consola se inunda y afecta performance. Envolver en #if UNITY_EDITOR o quitar.
        Debug.Log("Enter to " + state);
    }

    public virtual void OnUpdate() { }

    public virtual void OnExit()
    {
        // Warning: mismo problema que el Debug.Log de OnEnter.
        Debug.Log("Exit from " + state);
    }
}