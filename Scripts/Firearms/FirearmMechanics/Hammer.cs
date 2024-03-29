using System.Collections.Generic;
using UnityEngine;
using ThunderRoad;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Firearm components/Hammer")]
    public class Hammer : MonoBehaviour
    {
        [HideInInspector]
        public Item item;
        public Firearm firearm;
        public Transform hammer;
        public Transform idlePosition;
        public Transform cockedPosition;
        public List<AudioSource> hitSounds;
        public List<AudioSource> cockSounds;
        public bool hasDecocker = false;
        public bool allowManualCock = false;
        public bool allowCockUncockWhenSafetyIsOn = true;

        private void Awake()
        {
            hammer.localPosition = cockedPosition.localPosition;
            hammer.localEulerAngles = cockedPosition.localEulerAngles;
        }

        public void Pull()
        {
            hammer.localPosition = cockedPosition.localPosition;
            hammer.localEulerAngles = cockedPosition.localEulerAngles;
        }

        public void Fire(bool silent = false)
        {
            hammer.localPosition = idlePosition.localPosition;
            hammer.localEulerAngles = idlePosition.localEulerAngles;
        }
    }
}
