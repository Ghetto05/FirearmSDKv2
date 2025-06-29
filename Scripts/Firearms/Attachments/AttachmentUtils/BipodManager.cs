using System.Collections.Generic;
using GhettosFirearmSDKv2.Attachments;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Attachments/Systems/Bipod Manager")]
    public class BipodManager : MonoBehaviour
    {
        public Firearm firearm;
        public AttachmentManager attachmentManager;
        public Attachment attachment;
        
        public List<Bipod> bipods;
        public List<Transform> groundFollowers;
        public float linearRecoilModifier;
        public float muzzleRiseModifier;
    }
}
