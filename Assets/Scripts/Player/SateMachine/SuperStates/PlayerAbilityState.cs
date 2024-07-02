using Player.Data;
using Player.State;

namespace Player.SateMachine.SuperStates
{
    public class PlayerAbilityState : PlayerState
    {
        protected bool IsAbilityDone;

        private bool _isGrounded;
        public PlayerAbilityState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            IsAbilityDone = false;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (IsAbilityDone)
            {
                if (_isGrounded && player.CurrentVelocity.y < 0.01f)
                    stateMachine.ChangeState(player.IdleState);
                else
                    stateMachine.ChangeState(player.InAirState);
            }
        }

        public override void DoChecks()
        {
            base.DoChecks();

            _isGrounded = player.CheckIfGrounded();
        }
    }
}