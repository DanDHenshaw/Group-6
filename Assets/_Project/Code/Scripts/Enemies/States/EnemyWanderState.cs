using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
public class EnemyWanderState : EnemyBaseState
{
  private readonly NavMeshAgent agent;
  private readonly Vector3 startPoint;
  private readonly float wanderRadius;

  public EnemyWanderState(EnemyController enemy, NavMeshAgent agent, float wanderRadius) : base(enemy) 
  {
    this.agent = agent;
    this.startPoint = enemy.transform.position;
    this.wanderRadius = wanderRadius;
  }

  public override void OnEnter()
  {
    Debug.Log("Wander");
    //animator.CrossFade(WalkHash, crossFadeDuration);
  }

  public override void Update()
  {
    if(HasReachedDestination())
    {
      Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
      randomDirection += startPoint;
      NavMeshHit hit;
      NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1);
      Vector3 finalPosition = hit.position;

      agent.SetDestination(finalPosition);
    }
  }

  private bool HasReachedDestination()
  {
    return !agent.pathPending
      && agent.remainingDistance <= agent.stoppingDistance
      && (!agent.hasPath || agent.velocity.sqrMagnitude == 0.0f);
  }
}