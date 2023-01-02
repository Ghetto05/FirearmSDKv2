using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GhettosFirearmSDKv2.Explosives
{
    [AddComponentMenu("Firearm SDK v2/Explosives/Types/Flash bang")]
    public class Flashbang : Explosive
    {
        public AudioSource[] audioEffects;
        public ParticleSystem effect;
        public float range;
        public float time;
    }
}
