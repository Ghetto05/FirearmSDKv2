using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Firearm components/Chamber gravity ejector")]
    public class GravtiyEjector : MonoBehaviour
    {
        public BoltBase bolt;
        public Transform direction;
        public float maximumAngle;
        public List<Lock> locks;
    }
}
