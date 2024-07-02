using Player.Data;
using Player.SateMachine.SuperStates;
using Player.State;
using UnityEngine;

namespace Player.SateMachine.SupState
{
    public class PlayerWallGrabState : PlayerTouchingWallState
    {
        private Vector2  _holdPosition;
        public PlayerWallGrabState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _holdPosition = player.transform.position;

            HoldPosition();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!isExitingState) {
                HoldPosition();
                if (YInput > 0) {
                    stateMachine.ChangeState(player.WallClimbState);
                }
                else if (YInput < 0 || !GrabInput) {
                    stateMachine.ChangeState(player.WallSlideState);
                }
            }
        }

        private void HoldPosition()
        {
            player.transform.position = _holdPosition;

            player.SetVelocityY(0);
            player.SetVelocityX(0);
        }

    }
}