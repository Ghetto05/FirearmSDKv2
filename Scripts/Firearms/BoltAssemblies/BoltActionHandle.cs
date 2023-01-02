using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Bolt assemblies/Bolt action handle")]
    public class BoltActionHandle : MonoBehaviour
    {
        public BoltBase bolt;
        public Transform handle;
        public Transform lockedPosition;
        public Transform openedPosition;

        public List<AudioSource> handleUpSounds;
        public List<AudioSource> handleDownSounds;
    }
}
