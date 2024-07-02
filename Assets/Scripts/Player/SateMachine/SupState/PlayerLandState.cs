using Player.Data;
using Player.SateMachine.SuperStates;
using Player.State;

namespace Player.SateMachine.SupState
{
    public class PlayerLandState : PlayerGroundedState
    {
        public PlayerLandState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if(isExitingState)
                return;

            if(XInput != 0)
                stateMachine.ChangeState(player.MoveState);
            else if(isAnimationFinished)
                stateMachine.ChangeState(player.IdleState);

        }
    }
}