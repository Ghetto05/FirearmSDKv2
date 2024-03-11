using System;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    public class BoltAttacher : MonoBehaviour
    {
        public Attachment attachment;
        public Transform boltChild;
        public Transform boltRBChild;
        public List<GhettoHandle> additionalBoltHandles;
        public bool disableDefaultBoltHandles;
    }
}