using Player.State;
using UnityEngine;

namespace Player
{
    public class AnimationTriggers : MonoBehaviour
    {
        private Player player => GetComponentInParent<Player>();

        private void AnimationFinishTrigger() => player.AnimationFinishTrigger();
        private void AnimationTrigger() => player.AnimationTrigger();
    }
}