using System;
using Player.Data;
using Player.SateMachine.SuperStates;
using Player.State;
using UnityEngine;

namespace Player.SateMachine.SupState
{
    public class PlayerWallJumpState : PlayerAbilityState
    {
        private int wallJumpDirection;
        public PlayerWallJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();

            player.JumpState.ResetAmountOfJumpsLeft();
            player.SetVelocity(playerData.wallJumpVelocity, playerData.wallJumpAngle, wallJumpDirection);
            player.CheckIfShouldFlip(wallJumpDirection);
            player.JumpState.DecreaseAmountOfJumpsLeft();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            player.Anim.SetFloat("yVelocity", player.CurrentVelocity.y);
            player.Anim.SetFloat("xVelocity", MathF.Abs(player.CurrentVelocity.x));

            if (Time.time >= startTime + playerData.wallJumpTime) {
                IsAbilityDone = true;
            }

        }

        public void DetermineWallJumpDirection(bool isTouchingWall)
        {
            if (isTouchingWall)
            {
                wallJumpDirection = -player.FacingDirection;
            }
            else
            {
                wallJumpDirection = player.FacingDirection;
            }
        }
    }
}