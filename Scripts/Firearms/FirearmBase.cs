using System;
using UnityEngine;
using ThunderRoad;
using System.Collections.Generic;

namespace GhettosFirearmSDKv2
{
    public class FirearmBase : AIFireable
    {
        public bool setUpForHandPose = false;
        public Item item;
        public bool disableMainFireHandle = false;
        public List<Handle> additionalTriggerHandles;
        [HideInInspector]
        public bool triggerState;
        public BoltBase bolt;
        public MagazineWell magazineWell;
        public Transform hitscanMuzzle;
        public Transform actualHitscanMuzzle;
        public bool integrallySuppressed;
        public AudioSource[] fireSounds;
        public AudioSource[] suppressedFireSounds;
        public FireModes fireMode;
        public ParticleSystem defaultMuzzleFlash;
        public int burstSize = 3;
        public float roundsPerMinute;
        [HideInInspector]
        public float lastPressTime = 0f;
        [HideInInspector]
        public float longPressTime = 0.5f;
        public float recoilModifier = 1f;
        [HideInInspector]
        public bool countingForLongpress = false;
        public string defaultAmmoItem;

        public enum FireModes
        {
            Safe,
            Semi,
            Burst,
            Auto,
            AttachmentFirearm
        }

        public void OnCollisionEnter(Collision collision)
        {
            OnCollisionEvent?.Invoke(collision);
        }

        public delegate void OnCollision(Collision collision);
        public event OnCollision OnCollisionEvent;
    }
}
