using UnityEngine;

namespace Player.Data
{
    [CreateAssetMenu(fileName = "New Player Data", menuName = "Data/Player Data/Base Data")]
    public class PlayerData : ScriptableObject
    {
        [Header("Player Movement Data")]
        public float movementVelocity = 10f;
        public float jumpVelocity = 10f;
    }
}