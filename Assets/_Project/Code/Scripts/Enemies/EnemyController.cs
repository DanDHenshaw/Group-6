using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof(NavMeshAgent))]
[RequireComponent (typeof(PlayerDetector))]
public partial class EnemyController : Entity
{
  [Header("Config")]
  [SerializeField] float wanderRadius = 10f;
  [SerializeField] float attackRadius = 5f;

  [Header("References")]
  [SerializeField] NavMeshAgent agent;
  [SerializeField] Animator animator;
  [SerializeField] PlayerDetector playerDetector;

  StateMachine stateMachine;

  private void Awake()
  {
    agent = GetComponent<NavMeshAgent>();
    //animator = GetComponent<Animator>();
    playerDetector = GetComponent<PlayerDetector>();
  }

  private void Start()
  {
    stateMachine = new StateMachine();

    var wanderState = new EnemyWanderState(this, agent, wanderRadius);
    var chaseState = new EnemyChaseState(this, agent, playerDetector.Target);
    var attackState = new EnemyAttackState(this, agent, playerDetector.Target, attackRadius);

    At(wanderState, chaseState, new FuncPredicate(() => playerDetector.CanDetectPlayer()));
    At(chaseState, wanderState, new FuncPredicate(() => !playerDetector.CanDetectPlayer()));
    At(chaseState, attackState, new FuncPredicate(() => 
      Vector3.Distance(transform.position, playerDetector.Target.position) < attackRadius));
    At(attackState, chaseState, new FuncPredicate(() =>
      Vector3.Distance(transform.position, playerDetector.Target.position) > attackRadius));

    stateMachine.SetState(wanderState);
  }

  void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
  void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);

  private void Update()
  {
    stateMachine.Update();
  }

  private void FixedUpdate()
  {
    stateMachine.FixedUpdate();
  }
}
