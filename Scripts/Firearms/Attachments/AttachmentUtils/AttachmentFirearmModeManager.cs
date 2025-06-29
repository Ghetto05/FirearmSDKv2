using ThunderRoad;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    public class AttachmentFirearmModeManager : MonoBehaviour
    {
        public AttachmentFirearm attachmentFirearm;
        public bool addAttachmentFirearmMode;
        [CatalogPicker(new[] { Category.HandPose })]
        public string replacementTriggerHandlePose;
        [CatalogPicker(new[] { Category.HandPose })]
        public string replacementTriggerHandleTargetPose;
        public string[] allowReplacementOnItems;
    }
}