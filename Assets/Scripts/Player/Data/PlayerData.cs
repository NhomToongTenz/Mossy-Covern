using UnityEngine;

namespace Player.Data
{
    [CreateAssetMenu(fileName = "New Player Data", menuName = "Data/Player Data/Base Data")]
    public class PlayerData : ScriptableObject
    {
        [Header("Player Movement Data")]
        public float movementVelocity = 3f;

        [Header("Player Jump Data")]
        public float jumpVelocity = 10f;
        public int amountOfJumps = 1;

        [Header("In Air Variables")]
        public float coyoteTime = 0.2f;
        public float variableJumpHeightMultiplier = 0.5f;

        [Header("Wall Slide Variables")]
        public float wallSlideVelocity = 3f;

        [Header("Wall Climb Variables")]
        public float wallClimbVelocity = 3f;

        [Header("Check Variables")]
        public float groundCheckRadius = 0.3f;
        public float wallCheckDistance = 0.5f;
        public LayerMask whatIsGround;
    }
}