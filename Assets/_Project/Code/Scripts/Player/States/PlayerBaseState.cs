using UnityEngine;

public abstract class PlayerBaseState : IState
{
  protected readonly PlayerController playerController;
  protected readonly Animator animator;

  protected static readonly int LocomotionHash = Animator.StringToHash("Locomotion");
  protected static readonly int JumpHash = Animator.StringToHash("Jump");

  protected const float crossFadeDuration = 0.1f;

  protected PlayerBaseState(PlayerController playerController)
  {
    this.playerController = playerController;
    animator = playerController.GetComponent<Animator>();
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
