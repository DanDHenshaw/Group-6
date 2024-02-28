public class PlayerLocomotionState : PlayerBaseState
{
  public PlayerLocomotionState(PlayerController playerController) : base(playerController) { }

  public override void OnEnter()
  {
    animator.CrossFade(LocomotionHash, crossFadeDuration);
  }

  public override void FixedUpdate()
  {
    // call player move
  }
}
