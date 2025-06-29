using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ThunderRoad;
using UnityEditor;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Ammunition/Magazine")]
    public class Magazine : MonoBehaviour
    {
        public bool overrideLastAmmoItemWithInsertedCartridge = false;
        public bool ejectOnLastRoundFired = false;
        public bool infinite = false;
        public string magazineType;
        public string caliber;
        public bool useCaliberOfMagazineWell;
        public List<string> alternateCalibers;
        public bool forceCorrectCaliber = false;
        public List<Cartridge> cartridges;
        public int maximumCapacity;
        public bool canEjectRounds = true;
        public bool destroyOnEject = false;
        public Collider roundInsertCollider;
        public AudioSource[] roundInsertSounds;
        public Transform roundEjectPoint;
        public AudioSource[] roundEjectSounds;
        public AudioSource[] magazineEjectSounds;
        public AudioSource[] magazineInsertSounds;
        public Collider mountCollider;
        public Transform overrideMountPoint;
        public bool canBeGrabbedInWell;
        public List<Handle> handles;
        [HideInInspector]
        public MagazineWell currentWell;
        public Transform nullCartridgePosition;
        public Transform[] cartridgePositions;
        public Transform[] oddCountCartridgePositions;
        public List<Transform[]> alternateCartridgePositions;
        public MagazineLoad defaultLoad;
        [HideInInspector]
        public bool hasOverrideLoad;
        public Item overrideItem;
        public Attachment overrideAttachment;
        public List<Collider> colliders;
        public List<GameObject> feederObjects;
        public BoltBase bolt;
        public bool onlyAllowLoadWhenBoltIsBack;
        public List<MagazinePositionSet> positionSets;
        public bool addHandlesToParentMagazine;
        public string overrideMagazineAttachmentType;

#if UNITY_EDITOR
        [EasyButtons.Button]
        public void FixPreview()
        {
            Item item = gameObject.GetComponent<Item>();
            item.preview.transform.localEulerAngles = new Vector3(8.88f, -115.118f, 0f);
            EditorUtility.SetDirty(gameObject);
        }

        [EasyButtons.Button]
        public void SetAudioSourceMixers()
        {
            Util.FixAudioSources(gameObject);
            EditorUtility.SetDirty(gameObject);
        }

        [EasyButtons.Button]
        public void GetAllColliders()
        {
            colliders = new List<Collider>();
            foreach (Collider c in this.gameObject.GetComponentsInChildren<Collider>())
            {
                colliders.Add(c);
            }
            EditorUtility.SetDirty(gameObject);
        }

        [EasyButtons.Button]
        public void GetAllHandles()
        {
            handles = new List<Handle>();
            foreach (Handle h in this.gameObject.GetComponentsInChildren<Handle>())
            {
                handles.Add(h);
            }
            EditorUtility.SetDirty(gameObject);
        }

        [EasyButtons.Button]
        public void SetupDefault()
        {
            GameObject go;
            if (nullCartridgePosition == null)
            {
                go = Create(transform, "Null");
                nullCartridgePosition = go.transform;
            }
            if (roundEjectPoint == null)
            {
                go = Create(transform, "Eject");
                roundEjectPoint = go.transform;
            }
            if (GetComponentInChildren<Damager>() == null)
            {
                go = Create(transform, "Body");
                ColliderGroup cg = go.AddComponent<ColliderGroup>();
                go = Create(transform, "Blunt");
                Damager d = go.AddComponent<Damager>();
                d.colliderGroup = cg;
            }
            if (defaultLoad == null)
            {
                defaultLoad = gameObject.AddComponent<MagazineLoad>();
            }
            EditorUtility.SetDirty(gameObject);
        }

        private GameObject Create(Transform t, string name)
        {
            GameObject obj = new GameObject(name);
            obj.transform.SetParent(t);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localEulerAngles = Vector3.zero;

            return obj;
        }
#endif

        public void UpdateCartridgePositions()
        {
            foreach (Cartridge c in cartridges)
            {
                if (c != null && c.transform != null)
                {
                    if (cartridgePositions.Length - 1 < cartridges.IndexOf(c) || cartridgePositions[cartridges.IndexOf(c)] == null)
                    {
                        c.transform.parent = nullCartridgePosition;
                        c.transform.localPosition = Vector3.zero;
                        c.transform.localEulerAngles = Vector3.zero;
                    }
                    else
                    {
                        c.transform.parent = cartridgePositions[cartridges.IndexOf(c)];
                        c.transform.localPosition = Vector3.zero;
                        c.transform.localEulerAngles = Vector3.zero;
                    }
                }
            }
        }

        public Cartridge[] cs;
        [EasyButtons.Button]
        public void InsertRound()
        {
            for (int i = 0; i < cs.Length; i++)
            {
                InsertRound(cs[cs.Length - 1 - i], false, false, true, false);
            }
        }

        [EasyButtons.Button]
        public void InsertRound(Cartridge c, bool silent, bool forced, bool save = true, bool atBottom = false)
        {
            if (cartridges.Count < maximumCapacity && !cartridges.Contains(c))
            {
                int nullPositions = cartridges.Count(cn => cn == null);

                if (!atBottom)
                    cartridges.Insert(nullPositions, c);
                else
                    cartridges.Add(c);
                Util.PlayRandomAudioSource(roundInsertSounds);
                c.GetComponent<Rigidbody>().isKinematic = true;
                c.transform.parent = nullCartridgePosition;
                c.transform.localPosition = Vector3.zero;
                c.transform.localEulerAngles = Vector3.zero;
            }
            UpdateCartridgePositions();
        }
    }
}