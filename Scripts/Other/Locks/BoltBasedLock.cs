using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Locks/Bolt based")]
    public class BoltBasedLock : Lock
    {
        public enum Filters
        {
            LockAlwaysExcept,
            LockOnlyWhen
        }

        public Filters filter;
        public BoltBase.BoltState requiredState;
        public BoltBase bolt;
    }
}
