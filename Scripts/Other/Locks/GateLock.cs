using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThunderRoad;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Locks/Gate lock")]
    public class GateLock : Lock
    {
        public Item item;
        public Attachment attachment;
        public List<Handle> handles;
        public bool useFireHandle;

        public Transform gate;
        public Transform locked;
        public Transform unlocked;

        public List<AudioSource> openSounds;
        public List<AudioSource> closeSounds;
    }
}
