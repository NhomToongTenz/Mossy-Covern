using Player.AfterImage;
using Player.Data;
using Player.SateMachine.SuperStates;
using Player.State;
using UnityEngine;
namespace Player.SateMachine.SupState
{
    public class PlayerDashState : PlayerAbilityState
    {
        public bool CanDash { get; private set; }
        private bool _dashInputStop;
        private bool _isHolding;

        private float _lashDashTime;


        private Vector2 _dashDirection;
        private Vector2 _dashDirectionInput;
        private Vector2 _lastAIPos;
        private static readonly int YVelocity = Animator.StringToHash("yVelocity");
        private static readonly int XVelocity = Animator.StringToHash("xVelocity");

        public PlayerDashState(Player player, PlayerStateMachine stateMachine, PlayerData playerData
                             , string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

        public bool CheckIfCanDash() => CanDash && Time.time >= _lashDashTime + playerData.dashCooldown;
        public void ResetCanDash() => CanDash = true;

        public override void Enter() {
            base.Enter();
            CanDash = false;
            player.InputHandler.UseDashInput();

            _isHolding = true;
            _dashDirection = Vector2.right * player.FacingDirection;

            Time.timeScale = playerData.holdTimeScale;
            startTime = Time.unscaledTime;

            player.DashDirectionIndicator.gameObject.SetActive(true);
        }

        public override void LogicUpdate() {
            base.LogicUpdate();

            if (!isExitingState) {
                player.Anim.SetFloat(YVelocity, player.CurrentVelocity.y);
                player.Anim.SetFloat(XVelocity, Mathf.Abs(player.CurrentVelocity.x));

                if (_isHolding) {
                    _dashDirectionInput = player.InputHandler.DashDirectionInput;
                    _dashInputStop = player.InputHandler.DashInputStop;

                    if (_dashDirectionInput != Vector2.zero) {
                        _dashDirection = _dashDirectionInput;
                        _dashDirection.Normalize();
                    }

                    float angle = Vector2.SignedAngle(Vector2.right, _dashDirection);
                    player.DashDirectionIndicator.rotation = Quaternion.Euler(0f, 0f, angle - 45f);

                    if (_dashInputStop || Time.unscaledTime >= startTime + playerData.maxHoldTime) {
                        _isHolding = false;
                        Time.timeScale = 1f;
                        startTime = Time.time;
                        player.CheckIfShouldFlip(Mathf.RoundToInt(_dashDirection.x));
                        player.Rb.drag = playerData.drag;
                        player.SetVelocity(playerData.dashVelocity, _dashDirection);
                        player.DashDirectionIndicator.gameObject.SetActive(false);
                        PlaceAfterImage();
                    }
                }
                else {
                    player.SetVelocity(playerData.dashVelocity, _dashDirection);
                    CheckIfShouldPlaceAfterImage();
                    if (Time.time >= startTime + playerData.dashTime) {
                        player.Rb.drag = 0f;
                        IsAbilityDone = true;
                        _lashDashTime = Time.time;
                    }
                }
            }
        }

        private void CheckIfShouldPlaceAfterImage() {
            if (Vector2.Distance(player.transform.position, _lastAIPos) >= playerData.distBetweenAfterImages) {
                PlaceAfterImage();
            }
        }

        private void PlaceAfterImage() {
            PlayerAfterImagePool.Instance.GetFromPool();
            _lastAIPos = player.transform.position;
        }

        public override void Exit() {
            base.Exit();
            if (player.CurrentVelocity.y > 0) {
                player.SetVelocityY(player.CurrentVelocity.y * playerData.dashEndYMultiplier);
            }
        }
    }

}
