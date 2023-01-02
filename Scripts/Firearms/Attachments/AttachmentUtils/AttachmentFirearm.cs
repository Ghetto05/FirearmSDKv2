using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThunderRoad;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Attachments/Attachment based firearm")]
    public class AttachmentFirearm : FirearmBase
    {
        public Attachment attachment;
        public Handle fireHandle;
    }
}
