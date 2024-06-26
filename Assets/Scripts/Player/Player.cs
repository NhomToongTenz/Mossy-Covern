using System;
using Player.Data;
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
        #endregion

        #region Components

        public Animator Anim { get; private set; }
        public PlayerInputHandler InputHandler { get; private set; }
        public Rigidbody2D RB { get; private set; }
        public Vector2 CurrentVelocity { get; private set; }

        #endregion

        #region Check transform

        [SerializeField] private Transform groundCheck;

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

        public bool CheckIfGrounded() => Physics2D.OverlapCircle(groundCheck.position, playerData.groundCheckRadius, playerData.whatIsGround);

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
        }

        #endregion

        #region Other Functions
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