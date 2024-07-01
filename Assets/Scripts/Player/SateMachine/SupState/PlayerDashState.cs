using Player.Data;
using Player.SateMachine.SuperStates;
using Player.State;
using UnityEngine;
namespace Player.SateMachine.SupState
{
    public class PlayerDashState : PlayerAbilityState
    {
        public bool CanDash { get; private set; }

        private float lashDashTime;

        public PlayerDashState(Player player, PlayerStateMachine stateMachine, PlayerData playerData
                             , string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

        public bool CheckIfCanDash() => CanDash && Time.time >= lashDashTime + playerData.dashCooldown;
        public void ResetCanDash() => CanDash = true;

        public override void Enter() {
            base.Enter();
        }

        public override void LogicUpdate() {
            base.LogicUpdate();
        }

        public override void Exit() {
            base.Exit();
        }
    }

}
