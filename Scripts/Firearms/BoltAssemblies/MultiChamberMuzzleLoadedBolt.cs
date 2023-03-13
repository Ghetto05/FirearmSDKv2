using System.Collections;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Bolt assemblies/Muzzle loaded, multi chamber, self cocking")]
    public class MultiChamberMuzzleLoadedBolt : BoltBase
    {
        public bool ejectOnFire;

        public List<AudioSource> ejectSounds;
        public List<AudioSource> insertSounds;

        public List<string> calibers;
        public List<Transform> muzzles;
        public List<Transform> actualMuzzles;
        public List<ParticleSystem> muzzleFlashes;
        public List<ParticleSystem> actualMuzzleFlashes;
        public List<Hammer> hammers;
        public List<Transform> mountPoints;
        public List<Collider> loadColliders;
        public List<Transform> ejectDirections;
        public List<Transform> ejectPoints;
        public List<float> ejectForces;
    }
}