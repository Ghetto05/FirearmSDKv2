using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Switches/Fire mode based")]
    public class FireModeBasedSwitch : MonoBehaviour
    {
        public FirearmBase firearm;
        public Attachment attachment;
        public UnityEvent onSafe;
        public UnityEvent onSemi;
        public UnityEvent onBurst;
        public UnityEvent onAuto;
    }
}
