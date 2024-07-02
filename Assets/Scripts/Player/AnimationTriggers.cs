using UnityEngine;

namespace Player
{
    public class AnimationTriggers : MonoBehaviour
    {
        private Player Player => GetComponentInParent<Player>();

        private void AnimationFinishTrigger() => Player.AnimationFinishTrigger();
        private void AnimationTrigger() => Player.AnimationTrigger();
    }
}