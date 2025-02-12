﻿using UnityEngine;


public abstract class EnemyBaseState : IState
{
  protected readonly EnemyController enemy;
  protected readonly Animator animator;

  protected readonly int IdleHash = Animator.StringToHash("idle");
  protected readonly int WalkHash = Animator.StringToHash("walk");
  protected readonly int RunHash = Animator.StringToHash("run");
  protected readonly int AttackHash = Animator.StringToHash("attack");
  protected readonly int DeathHash = Animator.StringToHash("death");

  protected const float crossFadeDuration = 0.1f;

  protected EnemyBaseState(EnemyController enemy, Animator animator)
  {
    this.enemy = enemy;
    this.animator = animator;
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
