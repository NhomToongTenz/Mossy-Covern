using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
   public Vector2 RawMovementInput { get; private set; }
   public int NormilizedInputX { get; private set; }
   public int NormilizedInputY { get; private set; }
   public bool JumpInput { get; private set; }
   public bool JumpInputStop { get; private set; }
   public bool GrabInput { get; private set; }
   public bool DashInput { get; private set; }
   public bool  DashInputStop { get; private set; }

   [SerializeField] private float inputHoldTime = 0.2f;

   private float jumpInputStartTime;
   private float dashInputStartTime;

   private void Update()
   {
       CheckJumpInputHoldTime();
       CheckDashInputHoldTime();
   }

   public void OnDashInput(InputAction.CallbackContext context) {
       if (context.started) {
           DashInput = true;
           DashInputStop = false;
           dashInputStartTime = Time.time;
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
           jumpInputStartTime = Time.time;
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
       if (Time.time >= dashInputStartTime + inputHoldTime) {
           DashInput = false;
       }
   }
   private void CheckJumpInputHoldTime()
   {
       if(Time.time >= jumpInputStartTime + inputHoldTime)
       {
           JumpInput = false;
       }
   }


}
