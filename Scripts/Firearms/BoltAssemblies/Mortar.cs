using UnityEngine;

namespace GhettosFirearmSDKv2
{
    public class Mortar : BoltBase
    {
        public AudioSource[] cartridgeAttachSounds;
        public AudioSource[] cartridgeDetachSounds;
        public AudioSource[] additionalFireSounds;
        public AudioSource[] cartridgeSlideSounds;
        public AudioSource[] cartridgeDropSounds;
        public string caliber;
        public Collider loadCollider;
        public float dropTime;
        public Transform cartridgeStartPoint;
        public Transform cartridgeEndPoint;
    }
}