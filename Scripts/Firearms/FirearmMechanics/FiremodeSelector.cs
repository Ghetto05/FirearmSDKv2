using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}
