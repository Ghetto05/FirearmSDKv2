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
        public Transform SafetySwitch;
        public Transform SafePosition;
        public Transform SemiPosition;
        public Transform BurstPosition;
        public Transform AutoPosition;
        public Transform AttachmentFirearmPosition;
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
