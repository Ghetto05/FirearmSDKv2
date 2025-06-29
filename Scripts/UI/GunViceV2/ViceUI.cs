using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EasyButtons;
using Newtonsoft.Json;
using ThunderRoad;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable once CheckNamespace
namespace GhettosFirearmSDKv2.UI.GunViceV2
{
    public class ViceUI : MonoBehaviour
    {
        private const string DATA_PATH = @"F:\SteamLibrary\steamapps\common\Blade & Sorcery\BladeAndSorcery_Data\StreamingAssets\Mods\GhettosFirearms\JSONS";

        public List<Collider> screenColliders;
        public RectTransform slotContent;
        public RectTransform slotTemplate;
        public RectTransform attachmentCategoryContent;
        public RectTransform attachmentCategoryTemplate;
        public RectTransform attachmentTemplate;
        public RectTransform railAttachmentContent;
        public RectTransform railAttachmentTemplate;
        public Holder holder;
        public Handle[] freezeHandles;

        private Firearm _currentFirearm;
        private UISlot _currentSlot;
        private UIRailAttachment _currentRailAttachment;

        private readonly List<UISlot> _slots = new();
        private readonly List<UIAttachmentCategory> _attachmentCategories = new();
        private readonly List<UIAttachment> _attachments = new();
        private readonly List<UIRailAttachment> _railAttachments = new();

        public Button removeAttachmentButton;
        public Button moveAttachmentForwardButton;
        public Button moveAttachmentRearwardButton;
        public Button saveAmmoItemButton;
        public TextMeshProUGUI saveAmmoItemButtonText;
        public TextMeshProUGUI slotDisplay;
        public RectTransform slotDisplayButton;

        public RectTransform tacticalDeviceSetupScreen;
        public RectTransform pressurePadSetupScreen;
        public GameObject pressurePadTriggerChannelLabel;
        public TMP_Dropdown pressurePadTriggerChannelDropdown;
        public GameObject pressurePadAltUseChannelLabel;
        public TMP_Dropdown pressurePadAltUseChannelDropdown;
        public GameObject pressurePadActionLabel;
        public TMP_Dropdown pressurePadActionDropdown;
        public RectTransform tacticalDeviceChannelPrefab;

        public AudioSource selectSound;
        public AudioSource interactSound;

        private bool _allowSwitchingSlots = true;

        private void Start()
        {
            removeAttachmentButton.onClick.AddListener(RemoveAttachment);
            moveAttachmentForwardButton.onClick.AddListener(delegate { MoveAttachment(true); });
            moveAttachmentRearwardButton.onClick.AddListener(delegate { MoveAttachment(false); });
        }

        private void RemoveAttachment()
        {
            if (_currentSlot == null)
                return;

            if (_currentSlot.AttachmentPoint.usesRail && _currentRailAttachment != null && !_currentRailAttachment.IsNewButton)
            {
                UpdateSlots(_currentRailAttachment.CurrentAttachment, true);
                _currentRailAttachment.CurrentAttachment.Detach();
                _railAttachments.Remove(_currentRailAttachment);
                Destroy(_currentRailAttachment.gameObject);
            }
            else if (_currentSlot.AttachmentPoint.currentAttachments.Any())
            {
                if (_currentSlot.AttachmentPoint.currentAttachments.FirstOrDefault() is not { } attachment) return;
                _currentSlot.SetAttachment(null);
                UpdateSlots(attachment, true);
                attachment.Detach();
            }
            _attachments.ForEach(x => x.selectionOutline.SetActive(false));
        }

        private void MoveAttachment(bool forward)
        {
            _currentRailAttachment?.CurrentAttachment?.MoveOnRail(forward);
            UpdateSlotCounter();
        }

        private void UpdateSlotCounter()
        {
            slotDisplay.text = "Slot: " + _currentRailAttachment?.CurrentAttachment?.RailPosition;
        }

        [Button]
        public void SetupForFirearm(Firearm firearm)
        {
            var data = GetAttachmentData();
            GetAllAttachments(firearm).ForEach(x => x.Data = data.FirstOrDefault(y => x.gameObject.name.Equals(y.prefabAddress) || x.gameObject.name.Equals(y.prefabAddress + "(Clone)")));

            _currentFirearm = firearm;
            SetupSlots();
        }

        #region EditorDataLoader

        private List<Attachment> GetAllAttachments(Firearm firearm)
        {
            List<Attachment> attachments = new();
            foreach (var point in firearm.attachmentPoints)
            {
                GetAllAttachmentsRecurve(point, ref attachments);
            }

            return attachments;
        }

        private void GetAllAttachmentsRecurve(AttachmentPoint slot, ref List<Attachment> attachments)
        {
            attachments.AddRange(slot.currentAttachments);
            foreach (var point in slot.currentAttachments.SelectMany(x => x.attachmentPoints))
            {
                GetAllAttachmentsRecurve(point, ref attachments);
            }
        }

        #endregion

        private void SetupSlots()
        {
            foreach (var x in _slots.ToArray()) { Destroy(x.gameObject); }
            _slots.Clear();

            foreach (var point in _currentFirearm.attachmentPoints)
            {
                AddSlot(point, _currentFirearm.item.itemId + ".Icon"/*_currentFirearm.item.data.iconAddress*/); //ToDo
                point.currentAttachments.ForEach(x => AddSlotsFromAttachment(x));
            }
        }

        private void UpdateSlots(Attachment attachment, bool remove)
        {
            if (!attachment)
                return;

            if (remove)
            {
                List<Attachment> attachments = new() { attachment };
                foreach (var point in attachment.attachmentPoints)
                {
                    GetAllAttachmentsRecurve(point, ref attachments);
                }
                var attachmentPoints = attachments.SelectMany(x => x.attachmentPoints).ToList();
                var toDelete = _slots.Where(x => attachmentPoints.Contains(x.AttachmentPoint)).ToList();
                foreach (var uiSlot in toDelete)
                {
                    _slots.Remove(uiSlot);
                    Destroy(uiSlot.gameObject);
                }
            }
            else
            {
                var tr = _slots.FirstOrDefault(x => x.AttachmentPoint == attachment.attachmentPoint)?.transform;
                var start = tr?.GetSiblingIndex();
                AddSlotsFromAttachment(attachment, start + 1);
            }
        }

        private void AddSlotsFromAttachment(Attachment attachment, int? startIndex = null)
        {
            if (attachment == null)
                return;

            var address = attachment.Data.iconAddress;

            foreach (var point in attachment.attachmentPoints)
            {
                AddSlot(point, address, startIndex);
                point.currentAttachments.ForEach(x => AddSlotsFromAttachment(x, startIndex + 1));
            }
        }

        private void AddSlot(AttachmentPoint attachmentPoint, string iconAddress, int? setIndex = null)
        {
            var slot = Instantiate(slotTemplate, slotContent).GetComponent<UISlot>();
            slot.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            slot.Setup(attachmentPoint, iconAddress, this);
            slot.gameObject.SetActive(true);
            if (setIndex != null)
                slot.transform.SetSiblingIndex(setIndex.Value);
            _slots.Add(slot);
        }

        public void SelectSlot(UISlot slot)
        {
            if (!_allowSwitchingSlots)
                return;

            _currentSlot = slot;
            SetupAttachmentList(slot);
            SetupRailAttachmentList(slot);
            SetButtonVisibility(slot.AttachmentPoint.usesRail);
        }

        private void SetButtonVisibility(bool visible)
        {
            moveAttachmentForwardButton.gameObject.SetActive(visible);
            moveAttachmentRearwardButton.gameObject.SetActive(visible);
            slotDisplayButton.gameObject.SetActive(visible);
        }

        public void SelectCategory(UIAttachmentCategory category)
        {
            var cs = _attachmentCategories.ToList();
            cs.Remove(category);
            cs.ForEach(x => x.Collapse());
            category.Expand();
        }

        public void SelectAttachment(UIAttachment attachment)
        {
            _allowSwitchingSlots = false;
            var a = _attachments.ToList();
            a.Remove(attachment);
            a.ForEach(x => x.selectionOutline.SetActive(false));
            attachment.selectionOutline.SetActive(true);

            if (_currentSlot.AttachmentPoint.usesRail)
            {
                if (_currentRailAttachment.IsNewButton)
                {
                    attachment.Data.SpawnAndAttach(_currentSlot.AttachmentPoint, newAttachment => { _currentRailAttachment.Convert(attachment.Data, newAttachment); PostAttachmentSpawnCallback(newAttachment); });
                    AddRailAttachment(null);
                }
                else
                {
                    UpdateSlots(_currentSlot.AttachmentPoint.currentAttachments.FirstOrDefault(), true);

                    var railPos = _currentRailAttachment.CurrentAttachment.RailPosition;
                    _currentRailAttachment.CurrentAttachment.Detach();
                    attachment.Data.SpawnAndAttach(_currentSlot.AttachmentPoint, newAttachment => { _currentRailAttachment.Convert(attachment.Data, newAttachment); PostAttachmentSpawnCallback(newAttachment); }, railPos);
                }
            }
            else
            {
                UpdateSlots(_currentSlot.AttachmentPoint.currentAttachments.FirstOrDefault(), true);

                _currentSlot.AttachmentPoint.currentAttachments.FirstOrDefault()?.Detach();
                attachment.Data.SpawnAndAttach(_currentSlot.AttachmentPoint, newAttachment => { PostAttachmentSpawnCallback(newAttachment); });
            }
        }

        private void PostAttachmentSpawnCallback(Attachment attachment)
        {
            _currentSlot.SetAttachment(attachment);
            UpdateSlots(attachment, false);
            UpdateSlotCounter();
            _allowSwitchingSlots = true;
        }

        public void SelectRailAttachment(UIRailAttachment attachment)
        {
            if (!_allowSwitchingSlots)
                return;

            _currentRailAttachment = attachment;
            UpdateSlotCounter();

            _attachments.ForEach(x => x.selectionOutline.SetActive(false));
            _attachments.FirstOrDefault(x => x.Data.id == _currentRailAttachment?.CurrentAttachment?.Data.id)?.selectionOutline.SetActive(true);

            var attachments = _railAttachments.ToList();
            attachments.Remove(attachment);
            attachments.ForEach(x => x.selectionOutline.gameObject.SetActive(false));
            attachment.selectionOutline.gameObject.SetActive(true);
        }

        private void SetupAttachmentList(UISlot slot)
        {
            foreach (var x in _attachmentCategories.ToArray()) { Destroy(x.gameObject); }
            _attachmentCategories.Clear();
            foreach (var x in _attachments.ToArray()) { Destroy(x.gameObject); }
            _attachments.Clear();

            var data = GetAttachmentData();
            data = slot.AttachmentPoint.usesRail ?
                data.Where(x => x.railLength <= slot.AttachmentPoint.railSlots.Count && x.type.Equals(slot.AttachmentPoint.railType)).ToList() :
                data.Where(x => x.type == slot.AttachmentPoint.type).ToList();
            data.OrderBy(x => x.categoryName).ThenBy(y => y.displayName).ToList().ForEach(AddAttachment);
            if (_attachmentCategories.FirstOrDefault(x => x.headerText.text.Equals("Default")) is { } category)
            {
                category.transform.SetAsFirstSibling();
                _attachmentCategories.Remove(category);
                _attachmentCategories.Insert(0, category);
            }
            _attachmentCategories.FirstOrDefault()?.Expand();
        }

        private void SetupRailAttachmentList(UISlot slot)
        {
            foreach (var x in _railAttachments.ToArray()) { Destroy(x.gameObject); }
            _railAttachments.Clear();

            if (!slot.AttachmentPoint.usesRail)
                return;

            foreach (var attachment in slot.AttachmentPoint.currentAttachments)
            {
                AddRailAttachment(attachment);
            }
            AddRailAttachment(null);

            SelectRailAttachment(_railAttachments.Last());
        }

        private List<AttachmentData> GetAttachmentData()
        {
            //ToDo
            var directoryFiles = Directory.GetFiles(DATA_PATH, "*.json", SearchOption.AllDirectories);
            directoryFiles = directoryFiles.Where(x => File.ReadAllText(x).Contains("GhettosFirearmSDKv2.AttachmentData, GhettosFirearmSDKv2")).ToArray();
            Catalog.GetJsonNetSerializerSettings();
            var data = directoryFiles.Select(x => JsonConvert.DeserializeObject<AttachmentData>(File.ReadAllText(x), Catalog.jsonSerializerSettings)).ToList();
            return data;
        }

        private void AddAttachment(AttachmentData data)
        {
            var category = GetOrAddCategory(data.categoryName);
            var attachment = Instantiate(attachmentTemplate, category.foldoutContent).GetComponent<UIAttachment>();
            category.Attachments.Add(attachment);
            attachment.gameObject.SetActive(true);
            attachment.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            attachment.Setup(data, this);
            _attachments.Add(attachment);
            if (_currentSlot.AttachmentPoint.currentAttachments.Any(x => x.Data.id == data.id && (!_currentSlot.AttachmentPoint.usesRail || _currentRailAttachment.CurrentAttachment.Data.id.Equals(data.id))))
            {
                attachment.selectionOutline.SetActive(true);
            }
        }

        private void AddRailAttachment(Attachment attachment)
        {
            var a = Instantiate(railAttachmentTemplate, railAttachmentContent).GetComponent<UIRailAttachment>();
            a.gameObject.SetActive(true);
            a.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            a.Setup(attachment, this);
            _railAttachments.Add(a);
        }

        private UIAttachmentCategory GetOrAddCategory(string category)
        {
            var c = _attachmentCategories.FirstOrDefault(x => x.headerText.text.Equals(category));
            if (c != null)
                return c;

            c = Instantiate(attachmentCategoryTemplate, attachmentCategoryContent).GetComponent<UIAttachmentCategory>();
            c.gameObject.SetActive(true);
            c.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            c.Setup(category, this);
            _attachmentCategories.Add(c);
            return c;
        }
    }
}