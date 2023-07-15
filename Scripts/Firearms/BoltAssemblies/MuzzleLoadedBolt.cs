using System.Collections;
using UnityEngine;
using ThunderRoad;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Bolt assemblies/Muzzle loaded, self cocking")]
    public class MuzzleLoadedBolt : BoltBase
    {
        public bool ejectCasingOnReleaseButton = true;
        public Cartridge loadedCartridge;
        public Transform roundMount;
        public AudioSource[] ejectSounds;

        public Transform roundEjectPoint;
        public float roundEjectForce;
        public Transform roundEjectDir;
        public bool ejectOnFire = false;

        public Hammer hammer;
    }
}