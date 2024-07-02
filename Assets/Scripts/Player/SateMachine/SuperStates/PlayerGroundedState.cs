using Player.Data;
using Player.Input_System;
using Player.State;

namespace Player.SateMachine.SuperStates
{
    public class PlayerGroundedState : PlayerState
    {
        protected int XInput;
        protected int YInput;
        protected bool DashInput;

        protected bool IsTouchingCeiling;

        bool _jumpInput;
        bool _isGrounded;
        private bool _grabInput;
        private bool _isTouchingWall;
        private bool _isTouchingLedge;
        public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();

            player.JumpState.ResetAmountOfJumpsLeft();
            player.DashState.ResetCanDash();
        }


        public override void LogicUpdate()
        {
            base.LogicUpdate();

            XInput = player.InputHandler.NormilizedInputX;
            YInput = player.InputHandler.NormilizedInputY;
            _jumpInput = player.InputHandler.JumpInput;
            _grabInput = player.InputHandler.GrabInput;
            DashInput = player.InputHandler.DashInput;

            if (player.InputHandler.AttackInputs[(int)CombatInputs.Primary] && !IsTouchingCeiling) {
                stateMachine.ChangeState(player.PrimaryAttackState);
            }
            else if (player.InputHandler.AttackInputs[(int)CombatInputs.Secondary] && !IsTouchingCeiling) {
                stateMachine.ChangeState(player.SecondaryAttackState);
            }
            else if (_jumpInput && player.JumpState.CanJump()) {
                stateMachine.ChangeState(player.JumpState);
            }
            else if (!_isGrounded) {
                player.InAirState.StartCoyoteTime();
                player.JumpState.DecreaseAmountOfJumpsLeft();
                stateMachine.ChangeState(player.InAirState);
            }
            else if (_isTouchingWall && _grabInput && _isTouchingLedge) {
                stateMachine.ChangeState(player.WallGrabState);
            }
            else if (DashInput && player.DashState.CheckIfCanDash() && !IsTouchingCeiling) {
                stateMachine.ChangeState(player.DashState);
            }
        }

        public override void DoChecks()
        {
            base.DoChecks();
            _isGrounded = player.CheckIfGrounded();
            _isTouchingWall = player.CheckIfTouchingWall();
            _isTouchingLedge = player.CheckIfTouchingLedge();
            IsTouchingCeiling = player.CheckForCeiling();

        }
    }
}