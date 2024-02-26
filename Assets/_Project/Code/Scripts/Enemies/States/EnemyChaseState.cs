using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseState : EnemyBaseState
{
  private readonly NavMeshAgent agent;
  private readonly Transform target;

  public EnemyChaseState(EnemyController enemy, NavMeshAgent agent, Transform target) : base(enemy)
  {
    this.agent = agent;
    this.target = target;
  }

  public override void OnEnter()
  {
    animator.CrossFade(RunHash, crossFadeDuration);
  }

  public override void Update()
  {
    agent.SetDestination(target.position);
  }
}
