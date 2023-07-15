using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Bolt assemblies/Open bolt")]
    public class OpenBolt : BoltBase
    {
        public Rigidbody rb;
        public Transform obj;
        public Transform chargingHandle;

        public List<AudioSource> pullSounds;
        public List<AudioSource> pullSoundsHeld;
        public List<AudioSource> pullSoundsNotHeld;
        public List<AudioSource> rackSounds;
        public List<AudioSource> rackSoundsHeld;
        public List<AudioSource> rackSoundsNotHeld;
        public List<AudioSource> searCatchSounds;
        public List<AudioSource> chargingHandlePullSounds;
        public List<AudioSource> chargingHandleRackSounds;

        public Transform startPoint;
        public Transform searPoint;
        public Transform endPoint;
        public Transform hammerCockPoint;
        public Transform roundMount;

        public Hammer hammer;

        public float roundEjectForce;
        public Transform roundEjectDir;
        public Transform roundEjectPoint;
        public bool lockIfNoMagazineFound = false;
    }
}
