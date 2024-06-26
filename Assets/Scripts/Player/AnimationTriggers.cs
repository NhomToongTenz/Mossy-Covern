using Player.State;
using UnityEngine;

namespace Player
{
    public class AnimationTriggers : MonoBehaviour
    {
        private Player player => GetComponentInParent<Player>();

        private void AnimationTriggerEnter() => player.AnimationFinishTrigger();

    }
}