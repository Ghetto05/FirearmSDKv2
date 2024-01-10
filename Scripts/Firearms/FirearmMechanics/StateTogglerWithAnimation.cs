using System;
using System.Collections.Generic;
using UnityEngine;
using ThunderRoad;

namespace GhettosFirearmSDKv2
{
    public class StateTogglerWithAnimation : MonoBehaviour
    {
        public enum Actions
        {
            TriggerPull,
            TriggerRelease,
            AlternateUsePress,
            AlternateUseRelease,
            None
        }

        public Actions toggleAction;
        public Handle handle;
        public Item item;
        public Animation animationPlayer;
        public int currentState;
        public string toState1Anim;
        public string toState2Anim;
        public AudioSource[] toState1Sounds;
        public AudioSource[] toState2Sounds;
        public Animator animator;
    }
}
