using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ThunderRoad;

namespace GhettosFirearmSDKv2
{
    public class ViceUI : MonoBehaviour
    {
        public bool AlwaysFrozen;

        public Firearm currentFirearm;

        public Item item;

        public Collider screenCollider;

        public Canvas canvas;
        public AttachmentPoint currentSlot;
        public AttachmentPoint lastSlot = null;
        public Holder holder;

        public Transform slotContentReference;
        public GameObject slotButtonPrefab;
        public List<ViceUIAttachmentSlot> slotButtons;

        public Transform categoriesContentReference;
        public GameObject categoryButtonPrefab;
        public List<Transform> categories;

        public List<ViceUIAttachmentSlot> attachmentButtons;
        public GameObject attachmentButtonPrefab;

        public float categoryElementHeight;
        public float categroyHeaderHeight;
        public float categoryGapHeight;

        private void Awake()
        {
        }

        [EasyButtons.Button]
        public void SetAnchorPoint()
        {
            if (currentFirearm != null && currentFirearm.item.GetHolderPoint("Ghetto's Firearm SDK V2 vice anchor") != null)
            {
                currentFirearm.item.GetHolderPoint("Ghetto's Firearm SDK V2 vice anchor").anchor.SetPositionAndRotation(holder.transform.position, holder.transform.rotation);
            }
            else if (currentFirearm != null)
            {
                Debug.LogError($"Firearm is {currentFirearm}, anchor is {currentFirearm.item.GetHolderPoint("Ghetto's Firearm SDK V2 vice anchor")}!");
            }
            else
            {
                Debug.LogError($"Firearm is null!");
            }
        }

        [EasyButtons.Button]
        public void SetupFirearm()
        {
            if (holder.startObjects.Count > 0)
            {
                if (holder.startObjects[0].TryGetComponent(out Firearm firearm))
                {
                    if (slotButtons != null)
                    {
                        foreach (ViceUIAttachmentSlot button in slotButtons)
                        {
                            Destroy(button.gameObject);
                        }
                        slotButtons.Clear();
                    }

                    AddAttachmentSlots(firearm);
                }
                else Debug.Log("No firearm!");
            }
            else Debug.Log("No items!");
        }

        private void AddAttachmentSlots(Firearm parentFirearm)
        {
            foreach (AttachmentPoint point in parentFirearm.attachmentPoints)
            {
                AddPoint(point, parentFirearm.item.preview.generatedIcon, point.id);
                if (point.currentAttachment != null) FromAttachment(point.currentAttachment);
            }
        }

        private Transform GetOrAddCategory(string id)
        {
            foreach (Transform t in categories)
            {
                if (t.name.Equals(id)) return t;
            }
            Transform cat = Instantiate(categoryButtonPrefab, categoriesContentReference).transform;
            categories.Add(cat);
            return cat;
        }

        [EasyButtons.Button]
        public void PositionCategories()
        {
            categories = new List<Transform>();
            for (int i = 0; i < categoriesContentReference.childCount; i++)
            {
                categories.Add(categoriesContentReference.GetChild(i));
            }

            foreach (Transform cat in categories)
            {
                if (categories.IndexOf(cat) == 0) cat.localPosition = new Vector3(0, 0, 0);
                else
                {
                    int indexOfThis = categories.IndexOf(cat);

                    int previousHeaders = 0;
                    foreach (Transform prevCat in categories)
                    {
                        if (categories.IndexOf(prevCat) < indexOfThis)
                        {
                            previousHeaders++;
                        }
                    }
                    int previousGaps = previousHeaders;
                    int previousRows = previousHeaders;
                    foreach (Transform prevCat in categories)
                    {
                        if (categories.IndexOf(prevCat) < indexOfThis)
                        {
                            previousRows += (prevCat.GetChild(0).childCount - 1) / 6;
                            previousGaps += (prevCat.GetChild(0).childCount - 1) / 6;
                            previousGaps++;
                        }
                    }

                    Debug.Log($"Gaps: {previousGaps}, Rows: {previousRows}, Headers: {previousHeaders}");
                    float newY = -previousRows * categoryElementHeight - previousHeaders * categroyHeaderHeight - previousGaps * categoryGapHeight;

                    cat.localPosition = new Vector3(0, newY, 0);
                }
            }

            RectTransform trans = (RectTransform)categoriesContentReference;
            int headers = 0;
            foreach (Transform prevCat in categories)
            {
                headers++;
            }
            int gaps = headers;
            int rows = headers;
            foreach (Transform prevCat in categories)
            {
                rows += (prevCat.GetChild(0).childCount - 1) / 6;
                gaps += (prevCat.GetChild(0).childCount - 1) / 6;
                gaps++;
                if ((prevCat.GetChild(0).childCount - 1) % 6 == 0)
                {
                    rows--;
                    gaps--;
                }
            }
            float Y = rows * categoryElementHeight + headers * categroyHeaderHeight + gaps * categoryGapHeight;
            trans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Y);
        }

        private void FromAttachment(Attachment attachment)
        {
            foreach (AttachmentPoint point in attachment.attachmentPoints)
            {
                AddPoint(point, attachment.icon, point.id);
                if (point.currentAttachment != null) FromAttachment(point.currentAttachment);
            }
        }

        public void AddPoint(AttachmentPoint slot, Texture2D icon, string name)
        {
            GameObject obj = Instantiate(slotButtonPrefab);
            obj.transform.SetParent(slotContentReference);
            obj.transform.localScale = Vector3.one;
            obj.transform.localEulerAngles = Vector3.zero;
            obj.transform.localPosition = Vector3.zero;
            obj.SetActive(true);

            ViceUIAttachmentSlot slotComp = obj.GetComponent<ViceUIAttachmentSlot>();
            slotComp.slotName.text = name;
            slotComp.image.texture = icon;
            slotComp.button.onClick.AddListener(delegate { SetCurrentSlot(slot); });
            slotButtons.Add(slotComp);
        }

        public void SetCurrentSlot(AttachmentPoint point)
        {
            lastSlot = point;
            currentSlot = point;
        }

        private void Update()
        {
            //canvas.enabled = holder.items.Count > 0;
            //if (currentSlot != lastSlot)
            //{
            //    SetupAttachmentList(currentSlot.type);
            //}
        }

        private void SetupAttachmentList(string attachmentType)
        {
        }

        public void SpawnAttachment()
        {
        }
    }
}
