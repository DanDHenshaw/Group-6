using UnityEngine;
using UnityEngine.AI;
using Utilities;

[RequireComponent (typeof(NavMeshAgent))]
[RequireComponent (typeof(TargetDetector))]
[RequireComponent(typeof(Animator))]
public class EnemyController : Entity
{
  [Header("AI Config")]
  [SerializeField] float wanderRadius = 10f;

  [Header("Combat Config")] 
  [SerializeField] int damage = 10;
  [SerializeField] float attackRange = 2f;
  [SerializeField] float timeBetweenAttacks = 1f;

  public int Damage => damage;

  [Header("References")]
  [SerializeField] NavMeshAgent agent;
  [SerializeField] Animator animator;
  [SerializeField] TargetDetector targetDetector;

  StateMachine stateMachine;

  CountdownTimer attackTimer;

  private void Awake()
  {
    agent = GetComponent<NavMeshAgent>();
    animator = GetComponent<Animator>();
    targetDetector = GetComponent<TargetDetector>();
  }

  private void Start()
  {
    stateMachine = new StateMachine();

    attackTimer = new CountdownTimer(timeBetweenAttacks);

    targetDetector.attackRange = attackRange;

    var wanderState = new EnemyWanderState(this, agent, wanderRadius);
    var chaseState = new EnemyChaseState(this, agent, targetDetector.Target);
    var attackState = new EnemyAttackState(this, agent, targetDetector.Target);

    At(wanderState, chaseState, new FuncPredicate(() => targetDetector.CanDetectTarget()));
    At(chaseState, wanderState, new FuncPredicate(() => !targetDetector.CanDetectTarget()));
    At(chaseState, attackState, new FuncPredicate(() => targetDetector.CanAttackTarget()));
    At(attackState, chaseState, new FuncPredicate(() => !targetDetector.InAttackTargetRange()));


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

    attackTimer.Tick(Time.deltaTime);
  }

  public void Attack(int attackHash)
  {
    if(attackTimer.IsRunning) return;

    attackTimer.Start();

    animator.Play(attackHash, -1, 0f);
  }

  public void DamagePlayer()
  {
    targetDetector.Target.GetComponent<HealthSystem>().TakeDamage(damage);
  }
}
