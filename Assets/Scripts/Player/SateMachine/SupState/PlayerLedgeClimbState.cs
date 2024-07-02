using Player.Data;
using Player.State;
using UnityEngine;

namespace Player.SateMachine.SupState
{
    public class PlayerLedgeClimbState : PlayerState
    {
        private Vector2 _detectedPos;
        private Vector2 _cornerPos;
        private Vector2 _startPos;
        private Vector2 _stopPos;

        private bool _isHanging;
        private bool _isClimbing;
        private bool _jumpInput;
        private bool _isTouchingCeiling;

        private int _xInput;
        private int _yInput;
        private static readonly int ClimbLedge = Animator.StringToHash("climbLedge");
        private static readonly int IsTouchingCeiling = Animator.StringToHash("isTouchingCeiling");

        public PlayerLedgeClimbState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

        public void SetDetectedPosition(Vector2 pos) => _detectedPos = pos;

        public override void Enter() {
            base.Enter();

            player.SetVelocityZero();
            player.transform.position = _detectedPos;
            _cornerPos = player.DetermineCornerPosition();

            _startPos.Set(_cornerPos.x - (player.FacingDirection * playerData.startOffset.x)
                       , _cornerPos.y - playerData.startOffset.y);
            _stopPos.Set(_cornerPos.x + (player.FacingDirection * playerData.stopOffset.x)
                      , _cornerPos.y + playerData.stopOffset.y);

            player.transform.position = _startPos;

        }

        public override void LogicUpdate() {
            base.LogicUpdate();

            if (isAnimationFinished) {
                if (_isTouchingCeiling) {
                    stateMachine.ChangeState(player.CrouchIdleState);
                }
                else {
                    stateMachine.ChangeState(player.IdleState);
                }

            }
            else {
                _xInput = player.InputHandler.NormilizedInputX;
                _yInput = player.InputHandler.NormilizedInputY;
                _jumpInput = player.InputHandler.JumpInput;

                player.SetVelocityZero();
                player.transform.position = _startPos;

                if (_xInput == player.FacingDirection && _isHanging && !_isClimbing) {
                    CheckForSpace();
                    _isClimbing = true;
                    player.Anim.SetBool(ClimbLedge, true);
                }
                else if (_yInput == -1 && _isHanging && !_isClimbing) {
                    stateMachine.ChangeState(player.InAirState);
                }else if (_jumpInput && !_isClimbing) {
                    player.WallJumpState.DetermineWallJumpDirection(true);
                    stateMachine.ChangeState(player.WallJumpState);
                }
            }
        }

        public override void Exit() {
            base.Exit();

            _isHanging = false;

            if (_isClimbing) {
                player.transform.position = _stopPos;
                _isClimbing = false;
            }
        }

        public override void FinishAnimationTrigger() {
            base.FinishAnimationTrigger();
            player.Anim.SetBool(ClimbLedge, false);
        }

        public override void AnimationTrigger() {
            base.AnimationTrigger();

            _isHanging = true;
        }

        private void CheckForSpace() {
            _isTouchingCeiling =
                Physics2D.Raycast(_cornerPos + (Vector2.up * 0.015f) +
                                  (Vector2.right * (player.FacingDirection * 0.015f)), Vector2.up
                                , playerData.standColliderHeight, playerData.whatIsGround);
            player.Anim.SetBool(IsTouchingCeiling, _isTouchingCeiling);
        }
    }
}