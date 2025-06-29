using System;
using EasyButtons;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Attachments/Attachment point")]
    public class AttachmentPoint : MonoBehaviour
    {
        private const float PicatinnySlotDistance = 0.01f;
        private const float MlokSlotDistance = 0.04f;
        private const float KeyModSlotDistance = 0.02f;
        
        [TypePicker(TypePicker.Types.Attachment)]
        public string type;
        public List<string> alternateTypes;
        public string id;
        public List<Attachment> currentAttachments = new();
        public string defaultAttachment;
        public GameObject disableOnAttach;
        public GameObject enableOnAttach;
        public List<Collider> attachColliders;

        [Space]
        public bool usesRail;
        [TypePicker(TypePicker.Types.RailTypes)]
        public string railType;
        public List<Transform> railSlots;
        public bool requiredToFire;
        public bool dummyMuzzleSlot;
        public bool useOverrideMagazineAttachmentType;

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

        [Button]
        public void GetRailSlots()
        {
            railSlots = transform.Cast<Transform>().OrderBy(x => int.Parse(x.name)).ToList();
            EditorUtility.SetDirty(this.gameObject);
        }

        [Button]
        public void SetUpRail(int slotCount)
        {
            foreach (var t in transform.Cast<Transform>().ToArray())
            {
                DestroyImmediate(t.gameObject);
            }

            float desiredDistance = 
                railType.Equals("Picatinny") ? PicatinnySlotDistance :
                railType.Equals("MLok") ? MlokSlotDistance :
                railType.Equals("KeyMod") ? KeyModSlotDistance : 0f;
            for (int i = 0; i < slotCount; i++)
            {
                GameObject slot = new GameObject(i.ToString());
                slot.transform.SetParent(transform);
                slot.transform.SetLocalPositionAndRotation(Vector3.forward * i * desiredDistance, Quaternion.identity);
            }
            
            GetRailSlots();
        }

        private void OnDrawGizmos()
        {
            if (!usesRail)
                return;
            switch (railType)
            {
                case "Picatinny":
                    railSlots.ForEach(DrawPicatinnyReference);
                    break;
                case "MLok":
                    railSlots.ForEach(DrawMLokReference);
                    break;
                case "KeyMod":
                    railSlots.ForEach(DrawKeyModReference);
                    break;
            }
        }

        private static void DrawPicatinnyReference(Transform slot)
        {
            Gizmos.matrix = slot.localToWorldMatrix;
            Gizmos.color = Color.red;
            Gizmos.DrawCube(Vector3.zero, new Vector3(0.005f, 0f, 0.01f));
            Gizmos.DrawWireCube(Vector3.down * 0.0015f, new Vector3(0.0212f, 0.003f, 0.00525f));
            Gizmos.color = Color.green;
            Gizmos.DrawLine(Vector3.zero, Vector3.up * 0.01f);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(Vector3.zero, Vector3.forward * 0.002625f);
        }

        private static void DrawMLokReference(Transform slot)
        {
            Gizmos.matrix = slot.localToWorldMatrix;
            Gizmos.color = Color.red;
            Gizmos.DrawCube(Vector3.zero, new Vector3(0.0212f, 0f, 0.00525f));
            Gizmos.DrawWireCube(Vector3.down * 0.0015f, new Vector3(0.007f, 0.003f, 0.032f));
            Gizmos.color = Color.green;
            Gizmos.DrawLine(Vector3.zero, Vector3.up * 0.01f);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(Vector3.zero, Vector3.forward * 0.016f);
        }

        private void DrawKeyModReference(Transform slot)
        {
            Gizmos.matrix = slot.localToWorldMatrix;
            Gizmos.color = Color.red;
            Gizmos.DrawCube(Vector3.zero, new Vector3(0.02f, 0f, 0.003f));
            Gizmos.DrawWireMesh(CylinderMesh, new Vector3(0, -0.001f, 0), Quaternion.identity, new Vector3(0.0098f, 0.001f, 0.0098f));
            Gizmos.DrawWireMesh(CylinderMesh, new Vector3(0, -0.001f, 0.01f), Quaternion.identity, new Vector3(0.0059f, 0.001f, 0.0059f));
            Gizmos.DrawWireCube(new Vector3(0, -0.001f, 0.005f), new Vector3(0.0059f, 0.002f, 0.01f));
            Gizmos.color = Color.green;
            Gizmos.DrawLine(Vector3.zero, Vector3.up * 0.01f);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(Vector3.zero, Vector3.forward * 0.016f);
        }

        private static Mesh CylinderMesh => Resources.GetBuiltinResource<Mesh>("New-Cylinder.fbx");
#endif
    }
}