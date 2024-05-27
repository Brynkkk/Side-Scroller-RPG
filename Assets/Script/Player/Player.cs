using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Entity
{
    public bool isBusy { get; private set; }

    [Header("Move Info")]
    public float moveSpeed;
    public float jumpForce;

    [Header("Dash Info")]
    public float dashSpeed;
    public float dashDuration;
    public float dashDirection {  get; private set; }

    [Header("Attack Details")]
    public Vector2[] attackMovement;
    public float counterAttackDuration = .2f;

    #region States
    public PlayerStateMachine stateMachine { get; private set; }

    public PlayerIdle idleState { get; private set; }
    public PlayerMove moveState { get; private set; }
    public PlayerJump jumpState { get; private set; }
    public PlayerAerial airState { get; private set; }
    public PlayerDash dashState { get; private set; }
    public PlayerWallSlide wallSlideState { get; private set; }
    public PlayerWallJump wallJumpState { get; private set; }

    public PlayerPrimaryAttack primaryAttack { get; private set; }
    public PlayerCounterAttack counterAttack { get; private set; }

    public PlayerDead deadState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new PlayerStateMachine();        

        idleState = new PlayerIdle(this, stateMachine, "Idle");
        moveState = new PlayerMove(this, stateMachine, "Move");
        jumpState = new PlayerJump(this, stateMachine, "Jump");
        airState = new PlayerAerial(this, stateMachine, "Jump");
        dashState = new PlayerDash(this, stateMachine, "Dash");
        wallSlideState = new PlayerWallSlide(this, stateMachine, "WallSlide");
        wallJumpState = new PlayerWallJump(this, stateMachine, "Jump");

        primaryAttack = new PlayerPrimaryAttack(this, stateMachine, "Attack");
        counterAttack = new PlayerCounterAttack(this, stateMachine, "CounterAttack");

        deadState = new PlayerDead(this, stateMachine, "Die");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currState.Update();

        checkDashInput();
    }

    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;

        yield return new WaitForSeconds(_seconds);

        isBusy = false;
    }

    public void AnimationTrigger() => stateMachine.currState.AnimationFinishTrigger();

    private void checkDashInput()
    {
        if(IsWallDetected())
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.CanUseSkill())
        {
            dashDirection = Input.GetAxisRaw("Horizontal");

            if (dashDirection == 0)
            {
                dashDirection = facingDirection;
            }

            stateMachine.ChangeState(dashState);
        }

    }

    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deadState);
    }
}
