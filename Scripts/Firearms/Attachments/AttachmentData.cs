using UnityEngine;
using ThunderRoad;
using System.Collections.Generic;
using System.Collections;
using System;

namespace GhettosFirearmSDKv2
{
    public class AttachmentData : CustomData
    {
        public string displayName;
        public string type;
        public string prefabAddress;
        public string iconAddress;
        public string categoryName = "Default";

        public string GetID()
        {
            if (string.IsNullOrWhiteSpace(categoryName)) return "Default";
            else return categoryName;
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
    }
}
