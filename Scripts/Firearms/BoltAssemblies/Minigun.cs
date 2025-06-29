using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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

        [FormerlySerializedAs("RevUpSound")]
        public AudioSource revUpSound;
        [FormerlySerializedAs("RevDownSound")]
        public AudioSource revDownSound;
        [FormerlySerializedAs("RotatingLoop")]
        public AudioSource rotatingLoop;
        [FormerlySerializedAs("RotatingLoopPlusFiring")]
        public AudioSource rotatingLoopPlusFiring;
    }
}
