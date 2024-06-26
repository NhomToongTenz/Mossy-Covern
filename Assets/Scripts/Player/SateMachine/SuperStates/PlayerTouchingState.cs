using Player.Data;
using Player.State;
using UnityEngine;

namespace Player.SateMachine.SuperStates
{
    public class PlayerTouchingState : PlayerState
    {
        protected bool isGrounded;
        protected bool isTouchingWall;
        protected int xInput;
        protected int yInput;
        protected bool grabInput;
        public PlayerTouchingState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
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

            if (isGrounded && !grabInput)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else if(!isTouchingWall || (xInput != player.FacingDirection && !grabInput))
            {
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
            isTouchingWall = player.CheckIfTouchingWall();
        }

        public override void AnimationTrigger()
        {
            base.AnimationTrigger();
        }

        public override void FinishAnimationTrigger()
        {
            base.FinishAnimationTrigger();
        }
    }
}