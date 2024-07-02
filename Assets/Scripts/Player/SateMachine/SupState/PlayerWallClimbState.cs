using Player.Data;
using Player.SateMachine.SuperStates;
using Player.State;

namespace Player.SateMachine.SupState
{
    public class PlayerWallClimbState : PlayerTouchingWallState
    {
        public PlayerWallClimbState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!isExitingState) {
                player.SetVelocityY(playerData.wallClimbVelocity);

                if (yInput == 1 && !isExitingState) stateMachine.ChangeState(player.WallGrabState);
            }


        }
    }
}