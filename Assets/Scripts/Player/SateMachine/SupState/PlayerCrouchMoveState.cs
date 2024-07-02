using Player.Data;
using Player.SateMachine.SuperStates;
using Player.State;

namespace Player.SateMachine.SupState
{
    public class PlayerCrouchMoveState : PlayerGroundedState
    {
        public PlayerCrouchMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

        public override void Enter() {
            base.Enter();
            player.SetColliderHeight(playerData.crouchColliderHeight);
        }

        public override void Exit() {
            player.SetColliderHeight(playerData.standColliderHeight);
        }

        public override void LogicUpdate() {
            base.LogicUpdate();

            if (!isExitingState) {
                player.SetVelocityX(playerData.crouchMovementVelocity * player.FacingDirection);
                player.CheckIfShouldFlip(XInput);
                if (XInput == 0) {
                    stateMachine.ChangeState(player.CrouchIdleState);
                }else if (YInput != -1 && !IsTouchingCeiling) {
                    stateMachine.ChangeState(player.MoveState);
                }
            }
        }
    }
}
