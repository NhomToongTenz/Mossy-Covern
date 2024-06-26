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

   public void OnMoveInput(InputAction.CallbackContext context)
   {
       RawMovementInput = context.ReadValue<Vector2>();
       NormilizedInputX = (int)(RawMovementInput * Vector2.right).normalized.x;
       NormilizedInputY = (int)(RawMovementInput * Vector2.up).normalized.y;

   }


}
