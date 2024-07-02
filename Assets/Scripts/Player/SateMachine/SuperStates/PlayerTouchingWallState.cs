using Player.Data;
using Player.State;
using UnityEngine;

namespace Player.SateMachine.SuperStates
{
    public class PlayerTouchingWallState : PlayerState
    {
        protected bool isGrounded;
        protected bool isTouchingWall;
        protected bool jumpInput;
        protected int xInput;
        protected int yInput;
        protected bool grabInput;
        protected bool isTouchingLedge;
        public PlayerTouchingWallState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            xInput = player.InputHandler.NormilizedInputX;
            yInput = player.InputHandler.NormilizedInputY;
            grabInput = player.InputHandler.GrabInput;
            jumpInput = player.InputHandler.JumpInput;

            if (jumpInput) {
                player.WallJumpState.DetermineWallJumpDirection(isTouchingWall);
                stateMachine.ChangeState(player.WallJumpState);
            }

            if (isGrounded && !grabInput)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else if(!isTouchingWall || (xInput != player.FacingDirection && !grabInput))
            {
                stateMachine.ChangeState(player.InAirState);
            }else if (!isTouchingWall && !isTouchingLedge) {
                stateMachine.ChangeState(player.LedgeClimbState);
            }
        }

        public override void DoChecks()
        {
            base.DoChecks();

            isGrounded = player.CheckIfGrounded();
            isTouchingWall = player.CheckIfTouchingWall();
            isTouchingLedge = player.CheckIfTouchingLedge();

            if (isTouchingWall && !isTouchingLedge) {
                player.LedgeClimbState.SetDetectedPosition(player.transform.position);
            }
        }

    }
}