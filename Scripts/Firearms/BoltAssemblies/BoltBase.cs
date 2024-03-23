using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    public class BoltBase : MonoBehaviour
    {
        public int boltIndex = -1;
        public FirearmBase firearm;
        public bool fireOnTriggerPress = true;
        public ReciprocatingBarrel reciprocatingBarrel;
        public ParticleSystem[] breachSmokeEffects;
        public float cyclePercentage;

        public virtual void TryFire()
        {
            
        }

        public delegate void OnReachedEndDelegate();
        public event OnReachedEndDelegate OnReachedEndEvent;

        public delegate void OnLockedDelegate();
        public event OnLockedDelegate OnLockedEvent;

        public enum BoltState
        {
            Locked,
            Front,
            Moving,
            Back,
            LockedBack
        }

        public static Vector3 GrandparentLocalPosition(Transform child, Transform grandparent)
        {
            return grandparent.InverseTransformPoint(child.position);
        }
    }
}
