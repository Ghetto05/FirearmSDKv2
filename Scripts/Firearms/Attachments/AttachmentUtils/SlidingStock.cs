using UnityEngine;

namespace GhettosFirearmSDKv2
{
    public class SlidingStock : MonoBehaviour
    {
        public GameObject manager;
        public GhettoHandle handle;
        public Transform axis;
        public Transform forwardEnd;
        public Transform rearwardEnd;
        public bool usePositions;
        public Transform[] positions;
        public AudioSource[] unlockSounds;
        public AudioSource[] lockSounds;
    }
}