using System;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;
using UnityEngine.Serialization;

namespace GhettosFirearmSDKv2
{
    public class BoltAttacher : MonoBehaviour
    {
        public Attachment attachment;
        public Transform boltChild;
        [FormerlySerializedAs("boltRBChild")]
        public Transform boltRbChild;
        public List<GhettoHandle> additionalBoltHandles;
        public bool disableDefaultBoltHandles;
    }
}