public class EnemyKnockbackState : EnemyBaseState
{
  public EnemyKnockbackState(EnemyController enemy) : base(enemy) {}

  public override void OnEnter()
  {
    animator.CrossFade(IdleHash, crossFadeDuration);
  }
}
