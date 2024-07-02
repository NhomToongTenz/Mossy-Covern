using Player.Data;
using Player.Input_System;
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
        public PlayerWallSlideState WallSlideState { get; private set; }
        public PlayerWallGrabState WallGrabState { get; private set; }
        public PlayerWallClimbState WallClimbState { get; private set; }
        public PlayerWallJumpState WallJumpState { get; private set; }
        public PlayerLedgeClimbState LedgeClimbState { get; private set; }
        public PlayerDashState DashState { get; private set; }
        public PlayerCrouchMoveState CrouchMoveState { get; private set; }
        public PlayerCrouchIdle CrouchIdleState { get; private set; }

        public PlayerAttackState PrimaryAttackState { get; private set; }
        public PlayerAttackState SecondaryAttackState { get; private set; }
        #endregion

        #region Components

        public CapsuleCollider2D MovementCollider { get; private set; }
        public Animator Anim { get; private set; }
        public PlayerInputHandler InputHandler { get; private set; }
        public Rigidbody2D Rb { get; private set; }
        public Vector2 CurrentVelocity { get; private set; }
        public Transform DashDirectionIndicator { get; private set; }

        #endregion

        #region Check transform

        [SerializeField] private Transform groundCheck;
        [SerializeField] private Transform wallCheck;
        [SerializeField] private Transform ledgeCheck;
        [SerializeField] private Transform ceilingCheck;

        #endregion

        #region Others variables

        public int FacingDirection { get; private set; } = 1;

        [SerializeField] private PlayerData playerData;

        private Vector2 _workspace;

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
            WallSlideState = new PlayerWallSlideState(this, StateMachine, playerData, "wallSlide");
            WallGrabState = new PlayerWallGrabState(this, StateMachine, playerData, "wallGrab");
            WallClimbState = new PlayerWallClimbState(this, StateMachine, playerData, "wallClimb");
            WallJumpState = new PlayerWallJumpState(this, StateMachine, playerData, "inAir");
            LedgeClimbState = new PlayerLedgeClimbState(this, StateMachine, playerData, "ledgeClimbState");
            DashState = new PlayerDashState(this, StateMachine, playerData, "inAir");
            CrouchIdleState = new PlayerCrouchIdle(this, StateMachine, playerData, "crouchIdle");
            CrouchMoveState = new PlayerCrouchMoveState(this, StateMachine, playerData, "crouchMove");
            PrimaryAttackState = new PlayerAttackState(this, StateMachine, playerData, "attack");
            SecondaryAttackState = new PlayerAttackState(this, StateMachine, playerData, "attack");

        }

        void Start()
        {
            Anim = GetComponentInChildren<Animator>();
            InputHandler = GetComponent<PlayerInputHandler>();
            Rb = GetComponent<Rigidbody2D>();
            DashDirectionIndicator = transform.Find("DashDirectionIndicator");
            MovementCollider = GetComponent<CapsuleCollider2D>();

            StateMachine.Initialize(IdleState);
        }

        void Update()
        {
            CurrentVelocity = Rb.velocity;
            StateMachine.CurrentState.LogicUpdate();
        }

        void FixedUpdate()
        {
            StateMachine.CurrentState.PhysicsUpdate();
        }

        #endregion

        #region Set Functions

        public void SetVelocity(float velocity, Vector2 direction) {
            _workspace = direction * velocity;
            Rb.velocity = _workspace;
            CurrentVelocity = _workspace;
        }


        public void SetVelocityZero() {
            Rb.velocity = Vector2.zero;
            CurrentVelocity = Vector2.zero;
        }
        public void SetVelocity(float velocity, Vector2 angle, int direction)
        {
            angle.Normalize();
            _workspace.Set(angle.x * velocity * direction, angle.y * velocity);
            //Debug.Log(workspace);
            Rb.velocity = _workspace;
            CurrentVelocity = _workspace;
        }

        public void SetVelocityX(float velocity)
        {
            _workspace.Set(velocity,CurrentVelocity.y);
            Rb.velocity = _workspace;
            CurrentVelocity = _workspace;
        }

        public void SetVelocityY(float velocity)
        {
            _workspace.Set(CurrentVelocity.x,velocity);
            Rb.velocity = _workspace;
            CurrentVelocity = _workspace;
        }

        #endregion

        #region Check Functions
        public bool CheckForCeiling() => Physics2D.OverlapCircle(ceilingCheck.position,playerData.groundCheckRadius, playerData.whatIsGround);
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

        public void SetColliderHeight(float height) {
            Vector2 center = MovementCollider.offset;
            _workspace.Set(MovementCollider.size.x, height);

            center.y += (height - MovementCollider.size.y) / 2;

            MovementCollider.size = _workspace;
            MovementCollider.offset = center;
        }

        public Vector2 DetermineCornerPosition() {
            RaycastHit2D xHit = Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDirection
                                                , playerData.wallCheckDistance, playerData.whatIsGround);
            float xDist = xHit.distance;

            _workspace.Set((xDist + 0.015f) * FacingDirection, 0f);
            RaycastHit2D yHit = Physics2D.Raycast(ledgeCheck.position + (Vector3)(_workspace), Vector2.down
                                                , ledgeCheck.position.y - wallCheck.position.y + 0.015f
                                                , playerData.whatIsGround);
            float yDist = yHit.distance;

            _workspace.Set(wallCheck.position.x + (xDist * FacingDirection), ledgeCheck.position.y - yDist);
            return _workspace;
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