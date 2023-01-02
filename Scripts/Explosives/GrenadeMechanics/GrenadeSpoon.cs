using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThunderRoad;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Explosives/Detonators/Grenade spoon")]
    public class GrenadeSpoon : MonoBehaviour
    {
        public Explosives.Explosive explosive;
        public float fuseTime;
        public Transform startPosition;
        public Transform endPosition;
        public Rigidbody body;
        public float deployForce;
        public Transform forceDir;
        public List<Lock> locks;
        public Item grenadeItem;
        public float deployTime;
        public AudioSource[] deploySounds;
    }
}