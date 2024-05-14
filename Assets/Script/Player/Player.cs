using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerStateMachine stateMachine { get; private set; }

    public PlayerIdle IdleState { get; private set; }
    public PlayerMove MoveState { get; private set; }

    private void Awake()
    {
        stateMachine = new PlayerStateMachine();        

        IdleState = new PlayerIdle(this, stateMachine, "Idle");
        MoveState = new PlayerMove(this, stateMachine, "Move");
    }

    private void Start()
    {
        stateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        stateMachine.currState.Update();
    }

}
