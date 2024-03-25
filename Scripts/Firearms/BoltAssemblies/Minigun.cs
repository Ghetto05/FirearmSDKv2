using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Bolt assemblies/Minigun")]
    public class Minigun : BoltBase
    {
        public bool revOnTrigger;
        public bool loopingMuzzleFlash;
        
        public float[] barrelAngles;
        public Transform roundMount;
        public Cartridge loadedCartridge;
        public Transform roundEjectPoint;
        public Transform roundEjectDir;
        public float roundEjectForce;
        public Transform barrel;

        public AudioSource RevUpSound;
        public AudioSource RevDownSound;
        public AudioSource RotatingLoop;
        public AudioSource RotatingLoopPlusFiring;
    }
}
