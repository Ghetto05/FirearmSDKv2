using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThunderRoad;

namespace GhettosFirearmSDKv2.Explosives
{
    [AddComponentMenu("Firearm SDK v2/Explosives/Types/Simple explosive")]
    public class SimpleExplosive : Explosive
    {
        public ExplosiveData data;
        public AudioSource[] audioEffects;
        public ParticleSystem effect;
        public bool destroyItem = true;

        public override void ActualDetonate()
        {
        }
    }
}
