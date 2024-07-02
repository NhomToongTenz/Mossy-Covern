using Player.Data;
using Player.SateMachine.SuperStates;
using Player.State;

namespace Player.SateMachine.SupState
{
    public class PlayerWallSlideState: PlayerTouchingWallState
    {
        public PlayerWallSlideState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!isExitingState) {
                player.SetVelocityY(-playerData.wallSlideVelocity);

                if (grabInput && yInput == 0 && !isExitingState) {
                    stateMachine.ChangeState(player.WallGrabState);
                }
            }
        }
    }
}