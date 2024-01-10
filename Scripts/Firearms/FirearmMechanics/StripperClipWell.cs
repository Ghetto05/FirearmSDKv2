using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    public class StripperClipWell : MonoBehaviour
    {
        public string clipType;
        public MagazineWell magazineWell;
        public Transform mountPoint;
        public Collider mountCollider;
        public BoltBase bolt;
        public BoltBase.BoltState allowedState;
        public bool alwaysAllow;
        public AttachmentPoint[] blockingAttachmentPoints;
    }
}
