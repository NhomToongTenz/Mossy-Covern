using Player.Data;
using Player.SateMachine.SuperStates;
using Player.State;

namespace Player.SateMachine.SupState
{
    public class PlayerCrouchIdle : PlayerGroundedState
    {
        public PlayerCrouchIdle(Player player, PlayerStateMachine stateMachine, PlayerData playerData
                              , string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

        public override void Enter() {
            base.Enter();
            player.SetVelocityZero();
            player.SetColliderHeight(playerData.crouchColliderHeight);
        }

        public override void Exit() {
            player.SetColliderHeight(playerData.standColliderHeight);
        }

        public override void LogicUpdate() {
            base.LogicUpdate();

            if (!isExitingState) {
                if (XInput != 0) {
                    stateMachine.ChangeState(player.CrouchMoveState);
                }else if (YInput != -1 && !IsTouchingCeiling) {
                    stateMachine.ChangeState(player.IdleState);
                }
            }
        }
    }
}
