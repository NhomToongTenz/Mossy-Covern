using Player.Data;
using Player.State;
using UnityEngine;

namespace Player.SateMachine.SupState
{
    public class PlayerLedgeClimbState : PlayerState
    {
        private Vector2 detectedPos;
        private Vector2 cornerPos;
        private Vector2 startPos;
        private Vector2 stopPos;

        private bool isHanging;
        private bool isClimbing;

        private int xInput;
        private int yInput;
        public PlayerLedgeClimbState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

        public void SetDetectedPosition(Vector2 pos) => detectedPos = pos;

        public override void Enter() {
            base.Enter();

            player.SetVelocityZero();
            player.transform.position = detectedPos;
            cornerPos = player.DetermineCornerPosition();

            startPos.Set(cornerPos.x - (player.FacingDirection * playerData.startOffset.x)
                       , cornerPos.y - playerData.startOffset.y);
            stopPos.Set(cornerPos.x + (player.FacingDirection * playerData.stopOffset.x)
                      , cornerPos.y + playerData.stopOffset.y);

            player.transform.position = startPos;

        }

        public override void LogicUpdate() {
            base.LogicUpdate();

            if (isAnimationFinished) {
                stateMachine.ChangeState(player.IdleState);
            }
            else {
                xInput = player.InputHandler.NormilizedInputX;
                yInput = player.InputHandler.NormilizedInputY;

                player.SetVelocityZero();
                player.transform.position = startPos;

                if (xInput == player.FacingDirection && isHanging && !isClimbing) {
                    isClimbing = true;
                    player.Anim.SetBool("climbLegde", true);
                }
                else if (yInput == -1 && isHanging && !isClimbing) {
                    stateMachine.ChangeState(player.InAirState);
                }
            }
        }

        public override void Exit() {
            base.Exit();

            isHanging = false;

            if (isClimbing) {
                player.transform.position = stopPos;
                isClimbing = false;
            }
        }

        public override void FinishAnimationTrigger() {
            base.FinishAnimationTrigger();
            player.Anim.SetBool("climbLegde", false);
        }

        public override void AnimationTrigger() {
            base.AnimationTrigger();

            isHanging = true;
        }
    }
}