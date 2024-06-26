using Player.Data;
using Player.SateMachine.SuperStates;
using Player.State;

namespace Player.SateMachine.SupState
{
    public class PlayerJumpState : PlayerAbility
    {
        private int amountOfJumpsLeft;
        public PlayerJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
            amountOfJumpsLeft = playerData.amountOfJumps;
        }

        public override void Enter()
        {
            base.Enter();

            player.SetVelocityY(playerData.jumpVelocity);
            isAbilityDone = true;

            amountOfJumpsLeft--;
            player.InAirState.SetIsJumping();
        }

        public bool CanJump() => amountOfJumpsLeft > 0;

        public void ResetAmountOfJumpsLeft() => amountOfJumpsLeft = playerData.amountOfJumps;

        public void DecreaseAmountOfJumpsLeft() => amountOfJumpsLeft--;
    }
}