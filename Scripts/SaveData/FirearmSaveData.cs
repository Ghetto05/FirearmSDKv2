using System;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    public class FirearmSaveData : ContentCustomData
    {
        public AttachmentTreeNode firearmNode;

        public class AttachmentTreeNode
        {
            public string slot;
            public string attachmentId;
            public List<AttachmentTreeNode> childs;
            public List<SaveNodeValue> values;

            public AttachmentTreeNode()
            {
                childs = new List<AttachmentTreeNode>();
                values = new List<SaveNodeValue>();
            }

            public bool TryGetValue<T>(string id, out T value) where T : SaveNodeValue
            {
                foreach (SaveNodeValue v in values)
                {
                    if (v != null && v.id.Equals(id))
                    {
                        value = v as T;
                        return true;
                    }
                }
                value = default(T);
                return false;
            }

            public T GetOrAddValue<T>(string id, T newObject) where T : SaveNodeValue
            {
                SaveNodeValue value;
                if (TryGetValue(id, out value))
                {
                    return value as T;
                }
                else
                {
                    value = newObject;
                    value.id = id;
                    values.Add(value as T);
                }
                return value as T;
            }

            public void RemoveValue(string id)
            {
                SaveNodeValue vtd = null;
                foreach (SaveNodeValue v in values)
                {
                    if (v != null && v.id.Equals(id))
                    {
                        vtd = v;
                    }
                }
                if (vtd != null) values.Remove(vtd);
            }
        }
    }
}
