using Player.Data;
using UnityEngine;

namespace Player.State
{
    public abstract class PlayerState
    {
        protected Player player;
        protected PlayerStateMachine stateMachine;
        protected PlayerData playerData;

        protected float startTime;

        private string animBoolName;

        public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
        {
            this.player = player;
            this.stateMachine = stateMachine;
            this.playerData = playerData;
            this.animBoolName = animBoolName;
        }

        public virtual void Enter()
        {
            DoChecks();
            startTime = Time.time;
            player.Anim.SetBool(animBoolName, true);
        }

        public virtual void Exit()
        {
            player.Anim.SetBool(animBoolName, false);
        }

        public virtual void LogicUpdate()
        {

        }

        public virtual void PhysicsUpdate()
        {
            DoChecks();
        }

        public virtual void DoChecks()
        {

        }

    }
}