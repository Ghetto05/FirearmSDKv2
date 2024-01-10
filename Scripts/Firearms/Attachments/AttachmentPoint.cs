using EasyButtons;
using System.Collections.Generic;
using ThunderRoad;
using UnityEditor;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Attachments/Attachment point")]
    public class AttachmentPoint : MonoBehaviour
    {
        [TypePicker(TypePicker.Types.Attachment)]
        public string type;
        public List<string> alternateTypes;
        public string id;
        [HideInInspector]
        public Firearm parentFirearm;
        public Attachment currentAttachment;
        public string defaultAttachment;
        public GameObject disableOnAttach;
        public GameObject enableOnAttach;
        public List<Collider> attachColliders;

#if UNITY_EDITOR
        [Button]
        public void MarkShortScope()
        {
            if (alternateTypes == null) alternateTypes = new List<string>();
            type = "Picatinny Scope Short";
            if (alternateTypes.Contains("Picatinny Scope Short")) alternateTypes.Remove("Picatinny Scope Short");
            if (alternateTypes.Contains("Picatinny Scope")) alternateTypes.Remove("Picatinny Scope");
            if (alternateTypes.Contains("Picatinny Scope Long")) alternateTypes.Remove("Picatinny Scope Long");
            EditorUtility.SetDirty(this.gameObject);
        }

        [Button]
        public void MarkMediumScope()
        {
            if (alternateTypes == null) alternateTypes = new List<string>();
            type = "Picatinny Scope Short";
            if (alternateTypes.Contains("Picatinny Scope Short")) alternateTypes.Remove("Picatinny Scope Short");
            if (!alternateTypes.Contains("Picatinny Scope")) alternateTypes.Add("Picatinny Scope");
            if (alternateTypes.Contains("Picatinny Scope Long")) alternateTypes.Remove("Picatinny Scope Long");
            EditorUtility.SetDirty(this.gameObject);
        }

        [Button]
        public void MarkLongScope()
        {
            if (alternateTypes == null) alternateTypes = new List<string>();
            type = "Picatinny Scope Short";
            if (alternateTypes.Contains("Picatinny Scope Short")) alternateTypes.Remove("Picatinny Scope Short");
            if (!alternateTypes.Contains("Picatinny Scope")) alternateTypes.Add("Picatinny Scope");
            if (!alternateTypes.Contains("Picatinny Scope Long")) alternateTypes.Add("Picatinny Scope Long");
            EditorUtility.SetDirty(this.gameObject);
        }
#endif
    }
}