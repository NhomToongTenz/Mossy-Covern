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

   [SerializeField] private float inputHoldTime = 0.2f;
   private float jumpInputStartTime;


   private void Update()
   {
         CheckJumpInputHoldTime();
   }

   public void OnMoveInput(InputAction.CallbackContext context)
   {
       RawMovementInput = context.ReadValue<Vector2>();
       NormilizedInputX = (int)(RawMovementInput * Vector2.right).normalized.x;
       NormilizedInputY = (int)(RawMovementInput * Vector2.up).normalized.y;

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


   public void UseJumpInput() => JumpInput = false;

   private void CheckJumpInputHoldTime()
   {
       if(Time.time >= jumpInputStartTime + inputHoldTime)
       {
           JumpInput = false;
       }
   }


}