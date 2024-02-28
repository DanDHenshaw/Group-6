using UnityEngine;

public class EnemyKnockbackState : EnemyBaseState
{
  public EnemyKnockbackState(EnemyController enemy, Animator animator) : base(enemy, animator) {}

  public override void OnEnter()
  {
    animator.CrossFade(IdleHash, crossFadeDuration);
  }
}
