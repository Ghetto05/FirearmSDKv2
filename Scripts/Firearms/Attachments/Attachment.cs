﻿using ThunderRoad;
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using UnityEditor;
using EasyButtons;
using System.Linq;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Attachments/Base attachment")]
    public class Attachment : MonoBehaviour
    {
        public List<Handle> additionalTriggerHandles;
        [HideInInspector]
        public AttachmentPoint attachmentPoint;
        public List<AttachmentPoint> attachmentPoints = new List<AttachmentPoint>();
        public List<Handle> handles = new List<Handle>();
        public ColliderGroup colliderGroup;
        public string ColliderGroupId = "PropMetal";
        public List<ColliderGroup> alternateGroups;
        public List<string> alternateGroupsIds;
        [Space]
        public bool isSuppressing;
        public bool multiplyDamage = false;
        public float damageMultiplier = 1f;
        [HideInInspector]
        public Texture2D icon;
        private Preview preview;
        public bool overridesMuzzleFlash;
        public ParticleSystem newFlash;
        public Transform minimumMuzzlePosition;
        [Header("Default damager id: Mace1H")]
        public List<Damager> damagers = new List<Damager>(); 
        public List<String> damagerIds = new List<string>();
        public List<Renderer> nonLightVolumeRenderers;

        public List<UnityEvent> OnAttachEvents;
        public List<UnityEvent> OnDetachEvents;
#if UNITY_EDITOR

        [Button]
        public void SetAudioSourceMixers()
        {
            Util.FixAudioSources(gameObject);
        }

        [Button]
        public void SetupColliderGroupAndBluntDamager()
        {
            if (colliderGroup == null)
            {
                GameObject colliderGroupObj = new GameObject("Body");
                colliderGroupObj.transform.SetParent(transform);
                colliderGroupObj.transform.localPosition = Vector3.zero;
                colliderGroupObj.transform.localEulerAngles = Vector3.zero;
                colliderGroup = colliderGroupObj.AddComponent<ColliderGroup>();
            }

            if (damagers.Count < 1)
            {
                GameObject damagerObj = new GameObject("Blunt");
                damagerObj.transform.SetParent(transform);
                damagerObj.transform.localPosition = Vector3.zero;
                damagerObj.transform.localEulerAngles = Vector3.zero;
                Damager damager = damagerObj.AddComponent<Damager>();
                damager.colliderGroup = colliderGroup;
                damagers.Add(damager);
                damagerIds.Add("Mace1H");
            }
            EditorUtility.SetDirty(gameObject);
        }

        [Button]
        public void FindAllAttachmentPoints()
        {
            attachmentPoints = new List<AttachmentPoint>();
            foreach (AttachmentPoint point in gameObject.GetComponentsInChildren<AttachmentPoint>())
            {
                if (!attachmentPoints.Contains(point)) attachmentPoints.Add(point);
            }
            EditorUtility.SetDirty(gameObject);
        }

        [Button]
        public void FindAllHandles()
        {
            handles = new List<Handle>();

            foreach (Handle point in gameObject.GetComponentsInChildren<Handle>())
            {
                if (!handles.Contains(point)) handles.Add(point);
            }
            EditorUtility.SetDirty(gameObject);
        }

        [Button]
        public void SetAllHandlesAsForegrip()
        {
            FindAllHandles();

            List<Handle> oldAdditional = additionalTriggerHandles.ToList();
            additionalTriggerHandles = new List<Handle>();
            foreach (Handle handle in handles)
            {
                if (!oldAdditional.Contains(handle))
                {
                    GhettoHandle.ReplaceHandle(handle);
                }
                else
                {
                    GhettoHandle h = GhettoHandle.ReplaceHandle(handle);
                    h.type = GhettoHandle.HandleType.MainGrip;
                    additionalTriggerHandles.Add(h);
                }
            }


            FindAllHandles();
            EditorUtility.SetDirty(gameObject);
        }

        private void Reset()
        {
            GameObject prev = new GameObject("Preview");
            prev.transform.SetParent(transform);
            prev.transform.localPosition = Vector3.zero;
            prev.transform.localEulerAngles = new Vector3(8.88f, -115.118f, 0f);
            preview = prev.AddComponent<Preview>();

            if (minimumMuzzlePosition == null)
            {
                minimumMuzzlePosition = new GameObject("MinMuz").transform;
                minimumMuzzlePosition.SetParent(transform);
                minimumMuzzlePosition.localPosition = Vector3.zero;
                minimumMuzzlePosition.localEulerAngles = Vector3.zero;
            }
        }
#endif
        public void AlingnWithPoint()
        {
        }

        public void Detach()
        {
        }
    }
}
