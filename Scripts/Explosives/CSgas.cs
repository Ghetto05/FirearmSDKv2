using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace GhettosFirearmSDKv2.Explosives
{
    [AddComponentMenu("Firearm SDK v2/Explosives/Types/CS gas")]
    public class CSgas : Explosive
    {
        public float range;
        public float duration;
        public float emissionDuration;
        public AudioSource loop;
        public ParticleSystem particle;
        public GameObject volume;
    }
}