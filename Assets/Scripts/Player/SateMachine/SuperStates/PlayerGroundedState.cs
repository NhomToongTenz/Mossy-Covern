using Player.Data;
using Player.State;
using UnityEngine;

namespace Player.SateMachine.SuperStates
{
    public class PlayerGroundedState : PlayerState
    {
        protected int xInput;
        protected bool dashInput;
        bool JumpInput;
        bool isGrounded;
        private bool grabInput;
        private bool isTouchingWall;
        private bool isTouchingLedge;
        public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();

            player.JumpState.ResetAmountOfJumpsLeft();
            player.DashState.ResetCanDash();
        }

        public override void Exit()
        {
            base.Exit();

        }


        public override void LogicUpdate()
        {
            base.LogicUpdate();

            xInput = player.InputHandler.NormilizedInputX;
            JumpInput = player.InputHandler.JumpInput;
            grabInput = player.InputHandler.GrabInput;
            dashInput = player.InputHandler.DashInput;

            if (JumpInput && player.JumpState.CanJump())
            {
                stateMachine.ChangeState(player.JumpState);
            }else if (!isGrounded)
            {
                player.InAirState.StartCoyoteTime();
                player.JumpState.DecreaseAmountOfJumpsLeft();
                stateMachine.ChangeState(player.InAirState);
            }else if (isTouchingWall && grabInput && isTouchingLedge)
            {
                stateMachine.ChangeState(player.WallGrabState);
            }else if (dashInput && player.DashState.CheckIfCanDash()) {
                stateMachine.ChangeState(player.DashState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        public override void DoChecks()
        {
            base.DoChecks();
            isGrounded = player.CheckIfGrounded();
            isTouchingWall = player.CheckIfTouchingWall();
            isTouchingLedge = player.CheckIfTouchingLedge();

        }
    }
}