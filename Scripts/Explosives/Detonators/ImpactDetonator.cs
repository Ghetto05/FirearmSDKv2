using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GhettosFirearmSDKv2.Explosives;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Explosives/Detonators/Impact based")]
    public class ImpactDetonator : Explosive
    {
        public Explosive explosive;
        public Collider[] triggers;
        public float delay;
        public bool startAtAwake;
        public float minimumArmingTime;
        public float selfDestructDelay;
    }
}
