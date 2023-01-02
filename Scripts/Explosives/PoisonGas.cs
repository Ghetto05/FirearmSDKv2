using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GhettosFirearmSDKv2.Explosives
{
    [AddComponentMenu("Firearm SDK v2/Explosives/Types/Poison gas")]
    public class PoisonGas : Explosive
    {
        public float range;
        public float duration;
        public float emissionDuration;
        public float damagePerSecond;
        public AudioSource loop;
        public ParticleSystem particle;
    }
}