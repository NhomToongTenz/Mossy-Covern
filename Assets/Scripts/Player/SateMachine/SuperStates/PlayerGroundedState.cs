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

            if (JumpInput && player.JumpState.CanJump())
            {
                player.InputHandler.UseJumpInput();
                stateMachine.ChangeState(player.JumpState);
            }else if (!isGrounded)
            {
                player.InAirState.StartCoyoteTime();
                player.JumpState.DecreaseAmountOfJumpsLeft();
                stateMachine.ChangeState(player.InAirState);
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
        }
    }
}