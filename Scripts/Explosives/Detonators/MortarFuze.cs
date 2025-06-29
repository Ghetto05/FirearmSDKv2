using GhettosFirearmSDKv2.Explosives;
using ThunderRoad;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    public class MortarFuze : MonoBehaviour
    {
        public enum Modes
        {
            Impact,
            Proximity,
            Delay
        }

        public GameObject manager;
        public Explosive explosive;

        public Collider[] impactColliders;
        public Transform proximitySource;
        public MortarFuzeMode[] modes;
        public Transform selector;
        public Transform[] selectorPositions;
        public Handle selectorHandle;
        public AudioSource[] selectorSounds;

        public float minimumArmingSpeed;
        public float armingDistance;
    }
}