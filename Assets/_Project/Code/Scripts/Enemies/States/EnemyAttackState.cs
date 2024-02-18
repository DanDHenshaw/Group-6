using UnityEngine;
using UnityEngine.AI;

public class EnemyAttackState : EnemyBaseState
{
  private readonly NavMeshAgent agent;
  private readonly Transform target;
  private readonly float attackRadius;

  public EnemyAttackState(EnemyController enemy, NavMeshAgent agent, Transform target, float attackRadius) : base(enemy)
  {
    this.agent = agent;
    this.target = target;
    this.attackRadius = attackRadius;
  }

  public override void OnEnter()
  {
    Debug.Log("Attack");
    //animator.CrossFade(AttackHash, crossFadeDuration);
  }

  public override void Update()
  {
    if (Vector3.Distance(enemy.transform.position, target.position) < attackRadius)
    {
      agent.SetDestination(enemy.transform.position);
    }
  }
}