using UnityEngine;


public abstract class EnemyBaseState : IState
{
  protected readonly EnemyController enemy;
  protected readonly Animator animator;

  protected readonly int IdleHash = Animator.StringToHash("Idle");
  protected readonly int WalkHash = Animator.StringToHash("Walk");
  protected readonly int RunHash = Animator.StringToHash("Run");
  protected readonly int AttackHash = Animator.StringToHash("Attack");
  protected readonly int DeathHash = Animator.StringToHash("Death");

  protected const float crossFadeDuration = 0.1f;

  protected EnemyBaseState(EnemyController enemy)
  {
    this.enemy = enemy;
    animator = enemy.GetComponent<Animator>();
  }

  public virtual void OnEnter()
  {
    // noop
  }

  public virtual void Update()
  {
    // noop
  }

  public virtual void FixedUpdate()
  {
    // noop
  }

  public virtual void OnExit()
  {
    // noop
  }
}
