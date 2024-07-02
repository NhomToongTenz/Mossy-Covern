using Player.Data;
using Player.State;

namespace Player.SateMachine.SuperStates
{
    public class PlayerTouchingWallState : PlayerState
    {
        protected bool IsGrounded;
        protected bool IsTouchingWall;
        protected bool JumpInput;
        protected int XInput;
        protected int YInput;
        protected bool GrabInput;
        protected bool IsTouchingLedge;
        public PlayerTouchingWallState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            XInput = player.InputHandler.NormilizedInputX;
            YInput = player.InputHandler.NormilizedInputY;
            GrabInput = player.InputHandler.GrabInput;
            JumpInput = player.InputHandler.JumpInput;

            if (JumpInput) {
                player.WallJumpState.DetermineWallJumpDirection(IsTouchingWall);
                stateMachine.ChangeState(player.WallJumpState);
            }

            if (IsGrounded && !GrabInput)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else if(!IsTouchingWall || (XInput != player.FacingDirection && !GrabInput))
            {
                stateMachine.ChangeState(player.InAirState);
            }else if (!IsTouchingWall && !IsTouchingLedge) {
                stateMachine.ChangeState(player.LedgeClimbState);
            }
        }

        public override void DoChecks()
        {
            base.DoChecks();

            IsGrounded = player.CheckIfGrounded();
            IsTouchingWall = player.CheckIfTouchingWall();
            IsTouchingLedge = player.CheckIfTouchingLedge();

            if (IsTouchingWall && !IsTouchingLedge) {
                player.LedgeClimbState.SetDetectedPosition(player.transform.position);
            }
        }

    }
}