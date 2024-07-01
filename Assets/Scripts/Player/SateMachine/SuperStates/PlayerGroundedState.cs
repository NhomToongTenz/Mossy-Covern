using Player.Data;
using Player.State;
using UnityEngine;

namespace Player.SateMachine.SuperStates
{
    public class PlayerGroundedState : PlayerState
    {
        protected int xInput;
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
            }
            // else
            // {
            //     player.CheckIfShouldFlip(xInput);
            //     player.SetVelocityX(playerData.movementVelocity * xInput);
            //     player.Anim.SetFloat("xVelocity", Mathf.Abs(player.CurrentVelocity.x));
            // }

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