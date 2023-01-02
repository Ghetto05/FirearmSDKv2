using System;
using ThunderRoad;
using UnityEngine;
using System.Collections.Generic;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Locks/Grenade pin")]
    public class GreanadePin : Lock
    {
        public AudioSource[] pullSounds;
        bool broken = false;
        public Item parentItem;
        public Handle handle;
        public Joint joint;
        public float breakForce;
    }
}
