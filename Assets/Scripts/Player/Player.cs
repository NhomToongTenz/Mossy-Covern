using System;
using Player.Data;
using Player.SateMachine.SuperStates;
using Player.SateMachine.SupState;
using Player.State;
using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour
    {
        #region States

        public PlayerStateMachine StateMachine { get; private set; }

        public PlayerIdleState IdleState { get; private set; }
        public PlayerMoveState MoveState { get; private set; }
        public PlayerJumpState JumpState { get; private set; }
        public PlayerLandState LandState { get; private set; }
        public PlayerInAirState InAirState { get; private set; }
        public PlayerTouchingWallState TouchingWallState { get; private set; }
        public PlayerWallSlideState WallSlideState { get; private set; }
        public PlayerWallGrabState WallGrabState { get; private set; }
        public PlayerWallClimbState WallClimbState { get; private set; }
        public PlayerWallJumpState WallJumpState { get; private set; }
        public PlayerLedgeClimbState LedgeClimbState { get; private set; }
        public PlayerDashState DashState { get; private set; }
        #endregion

        #region Components

        public Animator Anim { get; private set; }
        public PlayerInputHandler InputHandler { get; private set; }
        public Rigidbody2D RB { get; private set; }
        public Vector2 CurrentVelocity { get; private set; }

        #endregion

        #region Check transform

        [SerializeField] private Transform groundCheck;
        [SerializeField] private Transform wallCheck;
        [SerializeField] private Transform ledgeCheck;

        #endregion

        #region Others variables

        public int FacingDirection { get; private set; } = 1;

        [SerializeField] private PlayerData playerData;

        private Vector2 workspace;

        #endregion

        #region Unity Callback Functions

        private void Awake()
        {
            StateMachine = new PlayerStateMachine();

            IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
            MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
            JumpState = new PlayerJumpState(this, StateMachine, playerData, "inAir");
            LandState = new PlayerLandState(this, StateMachine, playerData, "land");
            InAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");
            TouchingWallState = new PlayerTouchingWallState(this, StateMachine, playerData, "touching");
            WallSlideState = new PlayerWallSlideState(this, StateMachine, playerData, "wallSlide");
            WallGrabState = new PlayerWallGrabState(this, StateMachine, playerData, "wallGrab");
            WallClimbState = new PlayerWallClimbState(this, StateMachine, playerData, "wallClimb");
            WallJumpState = new PlayerWallJumpState(this, StateMachine, playerData, "inAir");
            LedgeClimbState = new PlayerLedgeClimbState(this, StateMachine, playerData, "ledgeClimbState");
            DashState = new PlayerDashState(this, StateMachine, playerData, "inAir");

        }

        void Start()
        {
            Anim = GetComponentInChildren<Animator>();
            InputHandler = GetComponent<PlayerInputHandler>();
            RB = GetComponent<Rigidbody2D>();

            StateMachine.Initialize(IdleState);
        }

        void Update()
        {
            CurrentVelocity = RB.velocity;
            StateMachine.CurrentState.LogicUpdate();
        }

        void FixedUpdate()
        {
            StateMachine.CurrentState.PhysicsUpdate();
        }

        #endregion

        #region Set Functions

        public void SetVelocityZero() {
            RB.velocity = Vector2.zero;
            CurrentVelocity = Vector2.zero;
        }
        public void SetVelocity(float velocity, Vector2 angle, int direction)
        {
            angle.Normalize();
            workspace.Set(angle.x * velocity * direction, angle.y * velocity);
            //Debug.Log(workspace);
            RB.velocity = workspace;
            CurrentVelocity = workspace;
        }

        public void SetVelocityX(float velocity)
        {
            workspace.Set(velocity,CurrentVelocity.y);
            RB.velocity = workspace;
            CurrentVelocity = workspace;
        }

        public void SetVelocityY(float velocity)
        {
            workspace.Set(CurrentVelocity.x,velocity);
            RB.velocity = workspace;
            CurrentVelocity = workspace;
        }

        #endregion

        #region Check Functions

        public bool CheckIfTouchingLedge() => Physics2D.Raycast(ledgeCheck.position, Vector2.right * FacingDirection
                                                             , playerData.wallCheckDistance, playerData.whatIsGround);
        public bool CheckIfGrounded() => Physics2D.OverlapCircle(groundCheck.position, playerData.groundCheckRadius, playerData.whatIsGround);
        public bool CheckIfTouchingWallBack() => Physics2D.Raycast(wallCheck.position, Vector2.right * -FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
        public bool CheckIfTouchingWall() => Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);

        public void CheckIfShouldFlip(int xInput)
        {
            if (xInput != 0 && xInput != FacingDirection)
            {
                Flip();
            }
        }

        public void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(groundCheck.position, playerData.groundCheckRadius);
            Gizmos.DrawLine(wallCheck.position
                          , wallCheck.position + Vector3.right * FacingDirection * playerData.wallCheckDistance);
            Gizmos.DrawLine(ledgeCheck.position
                          , ledgeCheck.position + Vector3.right * FacingDirection * playerData.wallCheckDistance);

        }

        #endregion

        #region Other Functions

        public Vector2 DetermineCornerPosition() {
            RaycastHit2D xHit = Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDirection
                                                , playerData.wallCheckDistance, playerData.whatIsGround);
            float xDist = xHit.distance;

            workspace.Set(xDist * FacingDirection, 0f);
            RaycastHit2D yHit = Physics2D.Raycast(ledgeCheck.position + (Vector3)(workspace), Vector2.down
                                                , ledgeCheck.position.y - wallCheck.position.y
                                                , playerData.whatIsGround);
            float yDist = yHit.distance;

            workspace.Set(wallCheck.position.x + (xDist * FacingDirection), ledgeCheck.position.y - yDist);
            return workspace;
        }
        public void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();
        public void AnimationFinishTrigger() => StateMachine.CurrentState.FinishAnimationTrigger();

        private void Flip()
        {
            FacingDirection *= -1;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }

        #endregion
    }
}