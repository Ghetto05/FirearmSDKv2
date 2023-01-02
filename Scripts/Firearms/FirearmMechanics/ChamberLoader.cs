using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Firearm components/Chamber loader")]
    public class ChamberLoader : MonoBehaviour
    {
        public string caliber;
        public Collider loadCollider;
        public BoltBase boltToBeLoaded;
        public Lock lockingCondition;
        public FirearmBase firearm;
        public AudioSource[] insertSounds;
    }
}
