using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Firearm components/Trigger")]
    public class Trigger : MonoBehaviour
    {
        public FirearmBase firearm;
        public Attachment attachment;

        public Transform trigger;
        public Transform idlePosition;
        public Transform pulledPosition;

        public AudioSource pullSound;
        public AudioSource resetSound;

        public float onTriggerWeight = 0.8f;
    }
}
