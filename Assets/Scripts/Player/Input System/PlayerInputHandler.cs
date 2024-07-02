using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Input_System
{
    public class PlayerInputHandler : MonoBehaviour
    {
        private PlayerInput _playerInput;
        private Camera _camera;
        public Vector2 RawMovementInput { get; private set; }
        public Vector2 RawDashDirectionInput { get; private set; }

        public Vector2Int DashDirectionInput { get; private set; }
        public int NormilizedInputX { get; private set; }
        public int NormilizedInputY { get; private set; }
        public bool JumpInput { get; private set; }
        public bool JumpInputStop { get; private set; }
        public bool GrabInput { get; private set; }
        public bool DashInput { get; private set; }
        public bool  DashInputStop { get; private set; }

        [SerializeField] private float inputHoldTime = 0.2f;

        private float _jumpInputStartTime;
        private float _dashInputStartTime;

        private void Start() {

            _playerInput = GetComponent<PlayerInput>();
            _camera = Camera.main;
        }

        private void Update()
        {
            CheckJumpInputHoldTime();
            CheckDashInputHoldTime();
        }

        public void OnDashDirectionInput(InputAction.CallbackContext context) {
            RawDashDirectionInput = context.ReadValue<Vector2>();

            if (_playerInput.currentControlScheme == "Keyboard")
                RawDashDirectionInput = _camera.ScreenToWorldPoint(RawDashDirectionInput) - transform.position;

            DashDirectionInput = Vector2Int.RoundToInt(RawDashDirectionInput.normalized);
        }

        public void OnDashInput(InputAction.CallbackContext context) {
            if (context.started) {
                DashInput = true;
                DashInputStop = false;
                _dashInputStartTime = Time.time;
            }else if (context.canceled) {
                DashInputStop = true;
            }
        }


        public void OnMoveInput(InputAction.CallbackContext context)
        {
            RawMovementInput = context.ReadValue<Vector2>();

            if (Mathf.Abs(RawMovementInput.x) > 0.5f)
                NormilizedInputX = (int)(RawMovementInput * Vector2.right).normalized.x;
            else
                NormilizedInputX = 0;

            if (Mathf.Abs(RawMovementInput.y) > 0.5f)
                NormilizedInputY = (int)(RawMovementInput * Vector2.up).normalized.y;
            else
                NormilizedInputY = 0;
        }

        public void OnJumpInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                JumpInput = true;
                JumpInputStop = false;
                _jumpInputStartTime = Time.time;
            }

            if (context.canceled)
            {
                JumpInputStop = true;
            }
        }

        public void OnGrabInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                GrabInput = true;
            }

            if (context.canceled)
            {
                GrabInput = false;
            }
        }

        public void UseDashInput() => DashInput = false;
        public void UseJumpInput() => JumpInput = false;

        private void CheckDashInputHoldTime() {
            if (Time.time >= _dashInputStartTime + inputHoldTime) {
                DashInput = false;
            }
        }
        private void CheckJumpInputHoldTime()
        {
            if(Time.time >= _jumpInputStartTime + inputHoldTime)
            {
                JumpInput = false;
            }
        }


    }
}
