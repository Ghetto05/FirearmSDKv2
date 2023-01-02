using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Firearm components/Bolt release button")]
    public class BoltReleaseButton : MonoBehaviour
    {
        public FirearmBase firearm;
        public Transform button;
        public Transform uncaughtPosition;
        public Transform caughtPosition;
        public Collider release;

        public delegate void OnReleaseDelegate();
        public event OnReleaseDelegate OnReleaseEvent;
    }
}
