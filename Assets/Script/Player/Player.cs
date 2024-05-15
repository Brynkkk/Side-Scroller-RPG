using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Move Info")]
    public float moveSpeed;
    public float jumpForce;

    [Header("Dash Info")]
    [SerializeField] private float dashCooldown;
    private float dashUsageTimer;
    public float dashSpeed;
    public float dashDuration;
    public float dashDirection {  get; private set; }

    [Header("Collision Info")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;

    public int facingDirection { get; private set; } = 1;
    private bool facingRight = true;

    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }

    #endregion

    #region States
    public PlayerStateMachine stateMachine { get; private set; }

    public PlayerIdle idleState { get; private set; }
    public PlayerMove moveState { get; private set; }
    public PlayerJump jumpState { get; private set; }
    public PlayerAerial airState { get; private set; }
    public PlayerDash dashState { get; private set; }
    public PlayerWallSlide wallSlideState { get; private set; }
    #endregion

    private void Awake()
    {
        stateMachine = new PlayerStateMachine();        

        idleState = new PlayerIdle(this, stateMachine, "Idle");
        moveState = new PlayerMove(this, stateMachine, "Move");
        jumpState = new PlayerJump(this, stateMachine, "Jump");
        airState = new PlayerAerial(this, stateMachine, "Jump");
        dashState = new PlayerDash(this, stateMachine, "Dash");
        wallSlideState = new PlayerWallSlide(this, stateMachine, "WallSlide");
    }

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        stateMachine.currState.Update();

        checkDashInput();
    }

    private void checkDashInput()
    {
        dashUsageTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashUsageTimer < 0)
        {
            dashUsageTimer = dashCooldown;
            dashDirection = Input.GetAxisRaw("Horizontal");

            if (dashDirection == 0)
            {
                dashDirection = facingDirection;
            }

            stateMachine.ChangeState(dashState);
        }

    }

    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        rb.velocity = new Vector2 (_xVelocity, _yVelocity);

        FlipController(_xVelocity);
    }

    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);

    public void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
    }

    public void Flip()
    {
        facingDirection = facingDirection * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    public void FlipController(float _x)
    {
        if(_x > 0 && !facingRight)
        {
            Flip();
        }
        else if(_x < 0 && facingRight) 
        {
            Flip();
        }
    }
}
