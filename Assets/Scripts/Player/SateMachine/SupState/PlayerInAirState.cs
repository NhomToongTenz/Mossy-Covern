using Player.Data;
using Player.State;
using UnityEngine;

namespace Player.SateMachine.SupState
{
    public class PlayerInAirState : PlayerState
    {
        private int xInput;
        private bool jumpInput;
        private bool jumpInputStop;
        private bool isGrounded;
        private bool isTouchingWall;
        private bool coyoteTime;
        private bool isJumping;
        private bool grabInput;
        public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
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

            CheckCoyoteTime();

            xInput = player.InputHandler.NormilizedInputX;
            jumpInput = player.InputHandler.JumpInput;
            jumpInputStop = player.InputHandler.JumpInputStop;
            grabInput = player.InputHandler.GrabInput;

            CheckJumpMultiplier();

            if(isGrounded && player.CurrentVelocity.y < 0.01f)
                stateMachine.ChangeState(player.LandState);
            else if (jumpInput && player.JumpState.CanJump())
            {
                player.InputHandler.UseJumpInput();
                stateMachine.ChangeState(player.JumpState);
            }
            else if(isTouchingWall && grabInput)
            {
                stateMachine.ChangeState(player.WallGrabState);
            }
            else if (isTouchingWall && !isGrounded && player.CurrentVelocity.y <= 0 && xInput == player.FacingDirection)
            {
                stateMachine.ChangeState(player.WallSlideState);
            }
            else
            {
                player.CheckIfShouldFlip(xInput);
                player.SetVelocityX(playerData.movementVelocity * xInput);

                player.Anim.SetFloat("yVelocity", player.CurrentVelocity.y);
                player.Anim.SetFloat("xVelocity", Mathf.Abs(player.CurrentVelocity.x));
            }
        }

        private void CheckJumpMultiplier()
        {
            if(isJumping)
                if(jumpInputStop)
                {
                    player.SetVelocityY(player.CurrentVelocity.y * playerData.variableJumpHeightMultiplier);
                    isJumping = false;
                }
                else if(player.CurrentVelocity.y <= 0)
                    isJumping = false;
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

        private void CheckCoyoteTime()
        {
            if (coyoteTime && Time.time >= startTime + playerData.coyoteTime)
            {
                coyoteTime = false;
                player.JumpState.DecreaseAmountOfJumpsLeft();
            }
        }

        public void StartCoyoteTime() => coyoteTime = true;

        public void SetIsJumping() => isJumping = true;
    }
}