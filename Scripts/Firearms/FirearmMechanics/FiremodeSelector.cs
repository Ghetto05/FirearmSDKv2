using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Firearm components/Fire mode selector")]
    public class FiremodeSelector : MonoBehaviour
    {
        public FirearmBase firearm;
        public Attachment attachment;
        [FormerlySerializedAs("SafetySwitch")]
        public Transform safetySwitch;
        [FormerlySerializedAs("SafePosition")]
        public Transform safePosition;
        [FormerlySerializedAs("SemiPosition")]
        public Transform semiPosition;
        [FormerlySerializedAs("BurstPosition")]
        public Transform burstPosition;
        [FormerlySerializedAs("AutoPosition")]
        public Transform autoPosition;
        public Transform attachmentFirearmPosition;
        public AudioSource switchSound;
        public FirearmBase.FireModes[] firemodes;
        public float[] fireRates;
        public Transform[] irregularPositions;
        public Hammer hammer;
        public bool allowSwitchingModeIfHammerIsUncocked = true;
        public bool onlyAllowSwitchingIfBoltHasState;
        public BoltBase.BoltState switchAllowedState;
    }
}
