using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Bolt assemblies/Barrel tilt-lock")]
    public class BarrelLocker : MonoBehaviour
    {
        public BoltBase bolt;
        public Transform barrel;
        public Transform lockedParent;
        public Transform openedParent;
        public BoltBase.BoltState lockedState;
    }
}