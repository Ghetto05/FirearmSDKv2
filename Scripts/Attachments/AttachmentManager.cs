using System.Collections.Generic;
using EasyButtons;
using ThunderRoad;
using UnityEditor;
using UnityEngine;

namespace GhettosFirearmSDKv2.Attachments
{
    public class AttachmentManager : MonoBehaviour, IAttachmentManager
    {
        public Item Item => item;
        public Transform Transform => transform;
        public FirearmSaveData SaveData { get; set; }

        public List<AttachmentPoint> AttachmentPoints
        {
            get => attachmentPoints;
            set => attachmentPoints = value;
        }
        public List<Attachment> CurrentAttachments { get; set; }

        public Item item;
        public List<AttachmentPoint> attachmentPoints;

        public event IAttachmentManager.Collision OnCollision;
        public event IAttachmentManager.AttachmentAdded OnAttachmentAdded;
        public event IAttachmentManager.AttachmentRemoved OnAttachmentRemoved;
        
#if UNITY_EDITOR
        [Button]
        public void FindAllAttachmentPoints()
        {
            attachmentPoints = new List<AttachmentPoint>();
            foreach (var point in gameObject.GetComponentsInChildren<AttachmentPoint>())
            {
                if (!attachmentPoints.Contains(point)) attachmentPoints.Add(point);
            }
            EditorUtility.SetDirty(gameObject);
        }
#endif
    }
}