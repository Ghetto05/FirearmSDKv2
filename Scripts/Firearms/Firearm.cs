﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;
using UnityEditor;
using EasyButtons;
using GhettosFirearmSDKv2.Attachments;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Firearm")]
    [RequireComponent(typeof(Item))]
    public class Firearm : FirearmBase, IAttachmentManager
    {
        #region TheftPrevention

        [HideInInspector]
        public bool GhettoMade_23103012730712037091283 = true;

        #endregion
        
        public List<AttachmentPoint> attachmentPoints;
#if UNITY_EDITOR
        [Button]
        public void SetHandles()
        {
            GhettoHandle main = GhettoHandle.ReplaceHandle(item.mainHandleLeft);
            main.type = GhettoHandle.HandleType.MainGrip;
            item.mainHandleLeft = main;
            item.mainHandleRight = item.mainHandleLeft;

            if (bolt != null)
            {
                foreach (Handle h in bolt.gameObject.GetComponentsInChildren<Handle>())
                {
                    GhettoHandle gg = GhettoHandle.ReplaceHandle(h);
                    gg.type = GhettoHandle.HandleType.Bolt;
                }
            }
            EditorUtility.SetDirty(gameObject);
        }

        [Button]
        public void SetupColliderGroup()
        {
            if (GetComponentInChildren<ColliderGroup>() == null)
            {
                GameObject body = new GameObject("Body");
                body.transform.SetParent(transform);
                ColliderGroup colliderGroup = body.AddComponent<ColliderGroup>();
                GameObject blunt = new GameObject("Blunt");
                blunt.transform.SetParent(transform);
                Damager damager = blunt.AddComponent<Damager>();
                damager.colliderGroup = colliderGroup;
                GameObject mechanics = new GameObject("Mechanics");
                mechanics.transform.SetParent(colliderGroup.transform);
                GameObject colliders = new GameObject("Colliders");
                colliders.transform.SetParent(colliderGroup.transform);
            }
        }

        [Button]
        public void SetAudioSourceMixers()
        {
            Util.FixAudioSources(gameObject);
            EditorUtility.SetDirty(gameObject);
        }

        [Button]
        public void FindAllAttachmentPoints()
        {
            attachmentPoints = new List<AttachmentPoint>();
            foreach (AttachmentPoint point in this.gameObject.GetComponentsInChildren<AttachmentPoint>())
            {
                if (!attachmentPoints.Contains(point)) attachmentPoints.Add(point);
            }
            EditorUtility.SetDirty(gameObject);
        }

        [Button]
        public void AddAnchorsAndFixPreview()
        {
            item = this.gameObject.GetComponent<Item>();
            item.holderPoint.localEulerAngles = new Vector3(0, 0, 90);

            if (!HasAnchor("HolderRackTopAnchor"))
            {
                Transform trans = new GameObject("HolderRackTopAnchor").transform;
                trans.parent = this.transform;
                trans.localPosition = Vector3.zero;
                trans.localEulerAngles = Vector3.zero;
                Item.HolderPoint holPoint = new Item.HolderPoint(trans, "HolderRackTopAnchor");
                item.additionalHolderPoints.Add(holPoint);
            }
            if (!HasAnchor("Ghetto's Firearm SDK V2 vice anchor"))
            {
                Transform trans = new GameObject("Ghetto's Firearm SDK V2 vice anchor").transform;
                trans.parent = this.transform;
                trans.localPosition = Vector3.zero;
                trans.localEulerAngles = Vector3.zero;
                Item.HolderPoint holPoint = new Item.HolderPoint(trans, "Ghetto's Firearm SDK V2 vice anchor");
                item.additionalHolderPoints.Add(holPoint);
            }
            if (!HasAnchor("Ghetto's Firearm SDK V2 gun case anchor"))
            {
                Transform trans = new GameObject("Ghetto's Firearm SDK V2 gun case anchor").transform;
                trans.parent = this.transform;
                trans.localPosition = Vector3.zero;
                trans.localEulerAngles = Vector3.zero;
                Item.HolderPoint holPoint = new Item.HolderPoint(trans, "Ghetto's Firearm SDK V2 gun case anchor");
                item.additionalHolderPoints.Add(holPoint);
            }
            item.preview.transform.localEulerAngles = new Vector3(8.88f, -115.118f, 0f);
        }

        private void Reset()
        {
            AddAnchorsAndFixPreview();
            if (hitscanMuzzle == null)
            {
                Transform mademuzzle = new GameObject("HitscanMuzzle").transform;
                mademuzzle.SetParent(this.transform);
                mademuzzle.localPosition = Vector3.zero;
                mademuzzle.localEulerAngles = Vector3.zero;
                hitscanMuzzle = mademuzzle;
            }
            if (TryGetComponent(out Item i))
            {
                if (string.IsNullOrWhiteSpace(i.itemId)) i.itemId = gameObject.name;
            }
        }
#endif
        private bool HasAnchor(string s)
        {
            return item.additionalHolderPoints.Any(point => point.anchorName.Equals(s));
        }

        public List<AttachmentPoint> AttachmentPoints { get; set; }
        public List<Attachment> CurrentAttachments { get; set; }
        public event IAttachmentManager.Collision OnCollision;
        public event IAttachmentManager.AttachmentAdded OnAttachmentAdded;
        public event IAttachmentManager.AttachmentRemoved OnAttachmentRemoved;
    }
}
