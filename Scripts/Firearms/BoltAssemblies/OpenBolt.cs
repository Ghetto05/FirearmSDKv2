using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Bolt assemblies/Open bolt")]
    public class OpenBolt : BoltBase
    {
        public List<AttachmentPoint> onBoltPoints;
        public bool lockIfNoMagazineFound = false;
        public bool catchBoltIfMagazineIsEmpty = false;
        public Rigidbody rigidBody;
        public Transform bolt;
        public Transform chargingHandle;
        public Transform startPoint;
        public Transform endPoint;
        public Transform searPoint;
        public Transform roundLoadPoint;
        public Transform hammerCockPoint;
        public Transform roundMount;
        public Cartridge loadedCartridge;

        public ConstantForce force;
        public float pointTreshold = 0.004f;

        public AudioSource[] rackSounds;
        public AudioSource[] pullSounds;
        public AudioSource[] chargingHandleRackSounds;
        public AudioSource[] chargingHandlePullSounds;
        public AudioSource[] rackSoundsHeld;
        public AudioSource[] pullSoundsHeld;
        public AudioSource[] rackSoundsNotHeld;
        public AudioSource[] pullSoundsNotHeld;

        public float roundEjectForce = 0.6f;
        public Transform roundEjectDir;
        public Transform roundEjectPoint;
        public float startTimeOfMovement = 0f;
        public Hammer hammer;
    }
}
