using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThunderRoad;
using UnityEditor;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Attachments/Attachment based firearm")]
    public class AttachmentFirearm : FirearmBase
    {
        public Attachment attachment;
        public Handle fireHandle;

#if UNITY_EDITOR
        [EasyButtons.Button]
        public void SetHandles()
        {
            GhettoHandle main = GhettoHandle.ReplaceHandle(fireHandle);
            main.type = GhettoHandle.HandleType.Foregrip;
            fireHandle = main;

            if (bolt != null)
            {
                foreach (Handle h in bolt.gameObject.GetComponentsInChildren<Handle>())
                {
                    GhettoHandle gg = GhettoHandle.ReplaceHandle(h);
                    gg.type = GhettoHandle.HandleType.Bolt;
                }
            }

            attachment.FindAllHandles();
            EditorUtility.SetDirty(gameObject);
        }
#endif
    }
}
