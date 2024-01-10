using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Firearm components/Magazine well")]
    public class MagazineWell : MonoBehaviour
    {
        public FirearmBase firearm;
        public string acceptedMagazineType;
        public List<string> alternateMagazineTypes;
        public string caliber;
        public List<string> alternateCalibers;
        public Collider loadingCollider;
        public Transform mountPoint;
        public bool canEject = true;
        public bool ejectOnEmpty = false;
        public Magazine currentMagazine;
        public bool mountCurrentMagazine = false;
        public bool spawnMagazineOnAwake = true;
        public Transform ejectDir;
        public float ejectForce = 0.3f;
        public bool tryReleasingBoltIfMagazineIsInserted = false;
        public bool onlyAllowEjectionWhenBoltIsPulled = false;
        public BoltBase.BoltState lockedState;
        public List<Lock> insertionLocks;
        public Transform beltLinkEjectDir;

        public bool IsEmpty()
        {
            if (currentMagazine == null) return true;
            return currentMagazine.cartridges.Count < 1;
        }
    }
}
