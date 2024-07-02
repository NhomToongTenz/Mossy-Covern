using Player.Data;
using Player.SateMachine.SuperStates;
using Player.State;

namespace Player.SateMachine.SupState
{
    public class PlayerMoveState: PlayerGroundedState
    {
        public PlayerMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void Exit()
        { }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            player.CheckIfShouldFlip(XInput);

            player.SetVelocityX(playerData.movementVelocity * XInput);

            if (!isExitingState) {
                if (XInput == 0f)
                {
                    stateMachine.ChangeState(player.IdleState);
                }else if (YInput == -1) {
                    stateMachine.ChangeState(player.CrouchMoveState);
                }
            }
        }

        public override void PhysicsUpdate()
        { }
    }
}