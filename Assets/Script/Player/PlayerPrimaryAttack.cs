using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttack : PlayerState
{
    private int comboCounter;

    private float lastTimeAttacked;
    private float comboWindow = 2;

    public PlayerPrimaryAttack(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    #region Attack Follow Facing Direction
    public override void Enter()
    {
        base.Enter();

        if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
        {
            comboCounter = 0;
        }

        player.anim.SetInteger("ComboCounter", comboCounter);

        float attackDirection = player.facingDirection;

        if (xInput != 0)
        {
            attackDirection = xInput;
        }

        player.SetVelocity(player.attackMovement[comboCounter].x * attackDirection, player.attackMovement[comboCounter].y);

        stateTimer = .1f;
    }
    #endregion

    #region Attack follow Mouse
    /* public override void Enter()
     {
         base.Enter();

         if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
         {
             comboCounter = 0;
         }

         player.anim.SetInteger("ComboCounter", comboCounter);

         Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
         float attackDirection = Mathf.Sign(mousePosition.x - player.transform.position.x);

         player.SetVelocity(player.attackMovement[comboCounter].x * attackDirection, player.attackMovement[comboCounter].y);

         stateTimer = .1f;
     }*/
    #endregion

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", .15f);

        comboCounter++;
        lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer < 0)
        {
            player.ZeroVelocity();  
        }

        if(triggerCalled) 
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
