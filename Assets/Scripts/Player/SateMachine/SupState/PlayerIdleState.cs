using Player.Data;
using Player.SateMachine.SuperStates;
using Player.State;

namespace Player.SateMachine.SupState
{
    public class PlayerIdleState : PlayerGroundedState
    {
        public PlayerIdleState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            player.SetVelocityX(0f);
        }

        public override void Exit()
        { }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!isExitingState) {
                if (XInput != 0f)
                {
                    stateMachine.ChangeState(player.MoveState);
                }else if (YInput == -1) {
                    stateMachine.ChangeState(player.CrouchIdleState);
                }
            }
        }

        public override void PhysicsUpdate()
        { }
    }
}