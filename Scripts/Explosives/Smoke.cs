using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GhettosFirearmSDKv2.Explosives
{
    [AddComponentMenu("Firearm SDK v2/Explosives/Types/Smoke")]
    public class Smoke : Explosive
    {
        public float range;
        public float emissionDuration;
        public float duration;
        public AudioSource loop;
        public ParticleSystem particle;

        void Awake()
        {
        }
    }
}