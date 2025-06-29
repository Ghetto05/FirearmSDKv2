using System;
using UnityEngine;
using ThunderRoad;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace GhettosFirearmSDKv2
{
    public class AttachmentData : CustomData
    {
        public string displayName;
        public string type;
        public string prefabAddress;
        public string iconAddress;
        public string categoryName = "Default";
        public int railLength = 1;
        public int forwardClearance;
        public int rearwardClearance;

        public string GetID()
        {
            return string.IsNullOrWhiteSpace(categoryName) ? "Default" : categoryName;
        }

        public static List<AttachmentData> AllOfType(string requestedType)
        {
            List<AttachmentData> dataList = new List<AttachmentData>();

            foreach (AttachmentData d in Catalog.GetDataList<AttachmentData>())
            {
                if (d.type.Equals(requestedType))
                {
                    dataList.Add(d);
                }
            }

            return dataList;
        }

        public void SpawnAndAttach(AttachmentPoint point, int? railPosition)
        {
            SpawnAndAttach(point, _ => { }, railPosition);
        }

        public void SpawnAndAttach(AttachmentPoint point, Action<Attachment> callback, int? railPosition = null)
        {
            if (point == null)
            {
                return;
            }
            
            var target = !point.usesRail ? 
                point.transform :
                point.railSlots != null ?
                    point.railSlots[railPosition ?? 0] :
                    point.transform;
            Addressables.InstantiateAsync(prefabAddress, target.position, target.rotation, target, false).Completed += (handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    var attachment = handle.Result.GetComponent<Attachment>();
                    point.currentAttachments.Add(attachment);
                    attachment.Data = this;
                    attachment.attachmentPoint = point;
                    attachment.SetRailPos(railPosition ?? 0);
                    callback.Invoke(attachment);
                }
                else
                {
                    Debug.LogWarning("Unable to instantiate attachment " + id + " from address " + prefabAddress);
                    Addressables.ReleaseInstance(handle);
                }
            });
        }
    }
}
