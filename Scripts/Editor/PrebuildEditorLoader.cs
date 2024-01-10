using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThunderRoad;
using UnityEditor;
using System.Linq;
using System.IO;
using Newtonsoft.Json;

namespace GhettosFirearmSDKv2
{
    public class PrebuildEditorLoader : MonoBehaviour
    {
#if UNITY_EDITOR
        public string path;
        public string prebuiltsPath;
        [Space]
        [Space]
        public string prebuiltID;

        [EasyButtons.Button]
        public void LoadSpecific()
        {
            foreach (GunLockerSaveData selectedData in Prebuilts().Where(selectedData => selectedData.id.Equals(prebuiltID)))
            {
                try
                {
                    if (transform.Find(selectedData.itemId) != null) continue;
                    GameObject fobj = (GameObject)PrefabUtility.InstantiatePrefab(Catalog.EditorLoad<GameObject>(LoadItemData(selectedData.itemId).prefabAddress));
                    fobj.transform.SetParent(transform);
                    Firearm f = fobj.GetComponent<Firearm>();

                    if (selectedData.dataList.Any(d => d.GetType() == typeof(FirearmSaveData)))
                    {
                        FirearmSaveData data = (FirearmSaveData)selectedData.dataList.First(d => d.GetType() == typeof(FirearmSaveData));
                        foreach (FirearmSaveData.AttachmentTreeNode node in data.firearmNode.childs)
                        {
                            Setup(node, f.attachmentPoints);
                        }
                    }
                }
                catch (Exception)
                {
                    Debug.LogError("Failed to load " + selectedData.displayName);
                }
            }
        }

        [EasyButtons.Button]
        public void LoadAll()
        {
            foreach (GunLockerSaveData selectedData in Prebuilts())
            {
                try
                {
                    if (transform.Find(selectedData.itemId) != null) continue;
                    GameObject fobj = (GameObject)PrefabUtility.InstantiatePrefab(Catalog.EditorLoad<GameObject>(LoadItemData(selectedData.itemId).prefabAddress));
                    fobj.transform.SetParent(transform);
                    Firearm f = fobj.GetComponent<Firearm>();

                    if (selectedData.dataList.Any(d => d.GetType() == typeof(FirearmSaveData)))
                    {
                        FirearmSaveData data = (FirearmSaveData)selectedData.dataList.First(d => d.GetType() == typeof(FirearmSaveData));
                        foreach (FirearmSaveData.AttachmentTreeNode node in data.firearmNode.childs)
                        {
                            Setup(node, f.attachmentPoints);
                        }
                    }
                }
                catch (Exception)
                {
                    Debug.LogError("Failed to load " + selectedData.displayName);
                }
            }
        }

        private void Setup(FirearmSaveData.AttachmentTreeNode node, List<AttachmentPoint> parentPoints)
        {
            if (node.attachmentId.IsNullOrEmptyOrWhitespace()) return;

            string address = "";
            try
            {
                address = LoadAttachmentData(node.attachmentId).prefabAddress;
            }
            catch (Exception)
            {
                Debug.LogError("Failed to load " + node.attachmentId);
                return;
            }
            GameObject obj = Catalog.EditorLoad<GameObject>(address);
            GameObject attachmentObj = (GameObject)PrefabUtility.InstantiatePrefab(obj);
            Attachment attachment = attachmentObj.GetComponent<Attachment>();
            AttachmentPoint attachmentPoint = GetSlotFromId(node.slot, parentPoints);
            Debug.Log("Anchor: " + (attachmentPoint != null));
            attachment.transform.SetParent(attachmentPoint.transform);
            attachment.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

            foreach (FirearmSaveData.AttachmentTreeNode childNode in node.childs)
            {
                Setup(childNode, attachment.attachmentPoints);
            }
        }

        public AttachmentPoint GetSlotFromId(string id, List<AttachmentPoint> points)
        {
            foreach (AttachmentPoint point in points)
            {
                if (point.id.Equals(id)) return point;
            }
            return null;
        }

        private ItemData LoadItemData(string id)
        {
            return JsonConvert.DeserializeObject<ItemData>(File.ReadAllText(FindFile(path, id + ".json")), Catalog.jsonSerializerSettings);
        }

        private AttachmentData LoadAttachmentData(string id)
        {
            return JsonConvert.DeserializeObject<AttachmentData>(File.ReadAllText(FindFile(path, id + ".json")), Catalog.jsonSerializerSettings);
        }

        public static string FindFile(string partialPath, string fileName)
        {
            string[] subDirectories = Directory.GetDirectories(partialPath, "*", SearchOption.AllDirectories);

            foreach (string subDirectory in subDirectories)
            {
                string[] files = Directory.GetFiles(subDirectory, fileName);

                foreach (string file in files)
                {
                    if (Path.GetFileName(file).Equals(fileName))
                    {
                        return file;
                    }
                }
            }

            return null;
        }

        public List<GunLockerSaveData> Prebuilts()
        {
            List<GunLockerSaveData> data = new List<GunLockerSaveData>();
            string[] files = Directory.GetFiles(prebuiltsPath, $"*PREBUILT_*.json", SearchOption.AllDirectories);
            foreach (string filePath in files)
            {
                string fileText = File.ReadAllText(filePath);
                try
                {
                    data.Add(JsonConvert.DeserializeObject<GunLockerSaveData>(fileText, Catalog.jsonSerializerSettings));
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message + "\n" + filePath);
                }
            }
            Debug.Log($"Found {data.Count} prebuilts");
            return data;
        }
#endif
    }
}
