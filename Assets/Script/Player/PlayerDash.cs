using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : PlayerState
{
    public PlayerDash(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = player.dashDuration;
    }

    public override void Exit()
    {
        base.Exit();

        player.SetVelocity(0, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(player.dashSpeed * player.dashDirection, 0);

        if (stateTimer < 0) 
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
