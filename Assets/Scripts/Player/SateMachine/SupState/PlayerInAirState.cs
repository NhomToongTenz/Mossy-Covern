using Player.Data;
using Player.State;
using UnityEngine;

namespace Player.SateMachine.SupState
{
    public class PlayerInAirState : PlayerState
    {
        private int _xInput;
        private bool _jumpInput;
        private bool _jumpInputStop;
        private bool _isGrounded;
        private bool _isTouchingWall;
        private bool _isTouchingWallBack;
        private bool _oldIsTouchingWall;
        private bool _oldIsTouchingWallBack;
        private bool _coyoteTime;
        private bool _isTouchingLedge;
        private bool _wallJumpCoyoteTime;
        private bool _isJumping;
        private bool _grabInput;

        private float _startWallJumpCoyoteTime;

        private static readonly int YVelocity = Animator.StringToHash("yVelocity");
        private static readonly int XVelocity = Animator.StringToHash("xVelocity");

        public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            CheckCoyoteTime();
            CheckWallJumpCoyoteTime();

            _xInput = player.InputHandler.NormilizedInputX;
            _jumpInput = player.InputHandler.JumpInput;
            _jumpInputStop = player.InputHandler.JumpInputStop;
            _grabInput = player.InputHandler.GrabInput;

            CheckJumpMultiplier();

            if(_isGrounded && player.CurrentVelocity.y < 0.01f)
                stateMachine.ChangeState(player.LandState);
            else if (_isTouchingWall && !_isTouchingLedge && !_isGrounded) {
                stateMachine.ChangeState(player.LedgeClimbState);
            }
            else if (_jumpInput && _isTouchingWall || _isTouchingWallBack || _wallJumpCoyoteTime) {
                StopWallJumpCoyoteTime();
                _isTouchingWall = player.CheckIfTouchingWall();
                player.WallJumpState.DetermineWallJumpDirection(_isTouchingWall);
                stateMachine.ChangeState(player.WallJumpState);
            }
            else if (_jumpInput && player.JumpState.CanJump()) {
                stateMachine.ChangeState(player.JumpState);
            }
            else if (_isTouchingWall && _grabInput && _isTouchingLedge) {
                stateMachine.ChangeState(player.WallGrabState);
            }
            else if (_isTouchingWall && !_isGrounded && player.CurrentVelocity.y <= 0 &&
                     _xInput == player.FacingDirection) {
                stateMachine.ChangeState(player.WallSlideState);
            }
            else {
                player.CheckIfShouldFlip(_xInput);
                player.SetVelocityX(playerData.movementVelocity * _xInput);

                player.Anim.SetFloat(YVelocity, player.CurrentVelocity.y);
                player.Anim.SetFloat(XVelocity, Mathf.Abs(player.CurrentVelocity.x));
            }
        }

        private void CheckJumpMultiplier()
        {
            if(_isJumping)
                if(_jumpInputStop)
                {
                    player.SetVelocityY(player.CurrentVelocity.y * playerData.variableJumpHeightMultiplier);
                    _isJumping = false;
                }
                else if(player.CurrentVelocity.y <= 0)
                    _isJumping = false;
        }

        public override void Exit() {
            base.Exit();

            _oldIsTouchingWall = false;
            _oldIsTouchingWallBack = false;
            _isTouchingWall = false;
            _isTouchingWallBack = false;
        }

        public override void DoChecks()
        {
            base.DoChecks();

            _oldIsTouchingWall = _isTouchingWall;
            _oldIsTouchingWallBack = _isTouchingWallBack;

            _isGrounded = player.CheckIfGrounded();
            _isTouchingWall = player.CheckIfTouchingWall();
            _isTouchingWallBack = player.CheckIfTouchingWallBack();
            _isTouchingLedge = player.CheckIfTouchingLedge();

            if (_isTouchingWall && !_isTouchingLedge) {
                player.LedgeClimbState.SetDetectedPosition(player.transform.position);
            }

            if (!_wallJumpCoyoteTime && !_isTouchingWall && !_isTouchingWallBack &&
                (_oldIsTouchingWall || _oldIsTouchingWallBack)) {
                StartWallJumpCoyoteTime();
            }

        }

        private void CheckCoyoteTime()
        {
            if (_coyoteTime && Time.time >= startTime + playerData.coyoteTime)
            {
                _coyoteTime = false;
                player.JumpState.DecreaseAmountOfJumpsLeft();
            }
        }

        private void CheckWallJumpCoyoteTime() {
            if (_wallJumpCoyoteTime && Time.time > _startWallJumpCoyoteTime + playerData.coyoteTime) {
                _wallJumpCoyoteTime = false;
            }
        }

        public void StartWallJumpCoyoteTime() {
            _wallJumpCoyoteTime = true;
            _startWallJumpCoyoteTime = Time.time;
        }

        public void StopWallJumpCoyoteTime() => _wallJumpCoyoteTime = false;
        public void StartCoyoteTime() => _coyoteTime = true;

        public void SetIsJumping() => _isJumping = true;
    }
}