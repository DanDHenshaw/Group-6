using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Utilities;

[RequireComponent (typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent (typeof(TargetDetector))]
public class EnemyController : Entity, IKnockbackable
{
  [Header("AI Config")]
  [SerializeField] float wanderRadius = 10f;
  [SerializeField, Range(0.001f, 1f)] private float stillThreshold = 0.05f;

  [Header("Combat Config")] 
  [SerializeField] int damage = 10;
  [SerializeField] float attackRange = 2f;
  [SerializeField] float timeBetweenAttacks = 1f;

  public int Damage => damage;

  [Header("References")]
  [SerializeField] NavMeshAgent agent;
  [SerializeField] Rigidbody rigidbody;
  [SerializeField] Animator animator;
  [SerializeField] TargetDetector targetDetector;
  [SerializeField] HealthSystem healthSystem;
  [SerializeField] AudioPlayer player = null;

  StateMachine stateMachine;

  CountdownTimer attackTimer;

  bool isKnockback = false;

  private void Awake()
  {
    agent = GetComponent<NavMeshAgent>();
    rigidbody = GetComponent<Rigidbody>();
    targetDetector = GetComponent<TargetDetector>();
    healthSystem = GetComponent<HealthSystem>();

    if(animator == null)
    {
      animator = GetComponent<Animator>();
    }
  }

  private void Start()
  {
    stateMachine = new StateMachine();

    attackTimer = new CountdownTimer(timeBetweenAttacks);

    targetDetector.attackRange = attackRange;

    var wanderState = new EnemyWanderState(this, animator, agent, wanderRadius);
    var chaseState = new EnemyChaseState(this, animator, agent, targetDetector.Target);
    var attackState = new EnemyAttackState(this, animator, agent, targetDetector.Target);
    var deathState = new EnemyDeathState(this, animator, agent);
    var knockbackState = new EnemyKnockbackState(this, animator);

    At(wanderState, chaseState, new FuncPredicate(() => targetDetector.CanDetectTarget()));
    At(chaseState, wanderState, new FuncPredicate(() => !targetDetector.CanDetectTarget()));
    At(chaseState, attackState, new FuncPredicate(() => targetDetector.CanAttackTarget()));
    At(attackState, chaseState, new FuncPredicate(() => !targetDetector.InAttackTargetRange()));
    Any(deathState, new FuncPredicate(() => healthSystem.IsDead));
    Any(knockbackState, new FuncPredicate(() => isKnockback));
    At(knockbackState, wanderState, new FuncPredicate(() => !isKnockback));

    stateMachine.SetState(wanderState);
  }

  void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
  void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);

  private void Update()
  {
    if(transform.position.y <= -100)
      KillSelf();

    if(agent.enabled)
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

  public void KillSelf() => Destroy(gameObject);

  public void RotateMe()
  {
    transform.LookAt(targetDetector.Target.transform.position);
  }

  public void GetKnockedBack(Vector3 force)
  {
    isKnockback = true;

    StartCoroutine(ApplyKnockback(force));
  }

  private IEnumerator ApplyKnockback(Vector3 force)
  {
    yield return null;
    agent.enabled = false;
    rigidbody.useGravity = true;
    rigidbody.isKinematic = false;
    rigidbody.AddForce(force);

    player?.PlayDamage();

    yield return new WaitForFixedUpdate();
    yield return new WaitUntil(() => rigidbody.velocity.magnitude < stillThreshold || healthSystem.IsDead);
    yield return new WaitForSeconds(0.25f);

    rigidbody.velocity = Vector3.zero;
    rigidbody.angularVelocity = Vector3.zero;
    rigidbody.useGravity = false;
    rigidbody.isKinematic = true;
    agent.Warp(transform.position);
    agent.enabled = true;

    yield return null;

    isKnockback = false;
  }

  public void PlayAttack() => player?.PlayAttack();
  public void PlayDeath() => player?.PlayDeath();
  public void PlayChatter() => player?.PlayChatter();
}
