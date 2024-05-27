using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattle : EnemyState
{
    private Transform player;
    private EnemySkeleton enemy;
    private int moveDirection;

    public SkeletonBattle(Enemy _enemyBase, EnemyStateMachine _statemachine, string _animBoolName, EnemySkeleton enemy) : base(_enemyBase, _statemachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(enemy.IsPlayerDetected()) 
        {
            stateTimer = enemy.battleTime;

            if(enemy.IsPlayerDetected().distance < enemy.attackDistance) 
            {
                if(CanAttack()) 
                {
                    stateMachine.ChangeState(enemy.attackState);
                }
            }
        }
        else
        {
            if(stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 7)
            {
                stateMachine.ChangeState(enemy.idleState);
            }
        }

        if(player.position.x > enemy.transform.position.x) 
        {
            moveDirection = 1;
        }
        else if(player.position.x < enemy.transform.position.x) 
        { 
            moveDirection = -1; 
        }

        enemy.SetVelocity(enemy.moveSpeed * moveDirection, rb.velocity.y);
    }

    private bool CanAttack()
    {
        if(Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown) 
        {
            enemy.lastTimeAttacked = Time.time;
            return true; 
        }
        else
        {
            return false;
        }
    }
}
