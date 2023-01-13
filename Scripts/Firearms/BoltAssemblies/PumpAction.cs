using System.Collections;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Bolt assemblies/Pump action")]
    public class PumpAction : BoltBase
    {
        public Rigidbody rb;
        public Transform bolt;
        public Transform startPoint;
        public Transform endPoint;
        public Transform roundEjectPoint;
        public Transform roundLoadPoint;
        public List<AttachmentPoint> onBoltPoints;
        public List<Handle> boltHandles;

        public float pointTreshold = 0.004f;

        public AudioSource[] rackSounds;
        public AudioSource[] pullSounds;

        public bool slamFire;

        public float roundEjectForce = 0.6f;
        public Transform roundEjectDir;

        public Transform roundMount;
        public Cartridge loadedCartridge;
    }
}
