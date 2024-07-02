using System;
using Player.Data;
using Player.SateMachine.SuperStates;
using Player.State;
using UnityEngine;

namespace Player.SateMachine.SupState
{
    public class PlayerWallJumpState : PlayerAbilityState
    {
        private int _wallJumpDirection;
        private static readonly int YVelocity = Animator.StringToHash("yVelocity");
        private static readonly int XVelocity = Animator.StringToHash("xVelocity");

        public PlayerWallJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();

            player.JumpState.ResetAmountOfJumpsLeft();
            player.SetVelocity(playerData.wallJumpVelocity, playerData.wallJumpAngle, _wallJumpDirection);
            player.CheckIfShouldFlip(_wallJumpDirection);
            player.JumpState.DecreaseAmountOfJumpsLeft();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            player.Anim.SetFloat(YVelocity, player.CurrentVelocity.y);
            player.Anim.SetFloat(XVelocity, MathF.Abs(player.CurrentVelocity.x));

            if (Time.time >= startTime + playerData.wallJumpTime) {
                IsAbilityDone = true;
            }

        }

        public void DetermineWallJumpDirection(bool isTouchingWall)
        {
            if (isTouchingWall)
            {
                _wallJumpDirection = -player.FacingDirection;
            }
            else
            {
                _wallJumpDirection = player.FacingDirection;
            }
        }
    }
}