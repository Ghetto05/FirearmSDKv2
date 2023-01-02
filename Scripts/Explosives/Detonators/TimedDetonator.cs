using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GhettosFirearmSDKv2.Explosives;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Explosives/Detonators/Time based")]
    public class TimedDetonator : MonoBehaviour
    {
        public Explosive explosive;
        public bool startAtAwake;
        public float delay;

        public void Arm()
        {
        }
    }
}
