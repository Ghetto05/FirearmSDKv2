using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Attachments/Systems/Magazines/Magazine well collider switcher")]
    public class MagazineWellColliderReplacer : MonoBehaviour
    {
        public Attachment attachment;
        public Collider newCollider;
        public Transform newMount;
        [HideInInspector]
        public Collider oldCollider;
        [HideInInspector]
        public Transform oldMount;
    }
}
