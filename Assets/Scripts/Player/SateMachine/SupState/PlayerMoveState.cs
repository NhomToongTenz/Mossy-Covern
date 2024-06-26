using Player.Data;
using Player.SateMachine.SuperStates;
using Player.State;
using UnityEngine;

namespace Player.SateMachine.SupState
{
    public class PlayerMoveState: PlayerGroundedState
    {
        public PlayerMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            player.SetVelocityX(playerData.movementVelocity * xInput);

            if (xInput == 0f)
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        public override void DoChecks()
        {
            base.DoChecks();
        }
    }
}