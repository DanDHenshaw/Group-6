public class PlayerJumpState : PlayerBaseState
{
  public PlayerJumpState(PlayerController playerController) : base(playerController) { }

  public override void OnEnter()
  {
    animator.CrossFade(JumpHash, crossFadeDuration);
  }

  public override void FixedUpdate()
  {
    // player jump
  }
}
