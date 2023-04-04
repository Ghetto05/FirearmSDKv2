using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThunderRoad;
using UnityEditor;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Ammunition/Magazine")]
    public class Magazine : MonoBehaviour
    {
        public bool infinite = false;
        public string magazineType;
        public string caliber;
        public List<string> alternateCalibers;
        public Stack<Cartridge> cartridges;
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
        public bool canBeGrabbedInWell;
        public List<Handle> handles;
        [HideInInspector]
        public MagazineWell currentWell;
        public Transform nullCartridgePosition;
        public Transform[] cartridgePositions;
        public MagazineLoad defaultLoad;
        [HideInInspector]
        public bool hasOverrideLoad;
        public Item overrideItem;
        public List<Collider> colliders;

        [EasyButtons.Button]
        public void FixPreview()
        {
            Item item = this.gameObject.GetComponent<Item>();
            item.preview.transform.localEulerAngles = new Vector3(8.88f, -115.118f, 0f);
        }

        [EasyButtons.Button]
        public void SetAudioSourceMixers()
        {
            Util.FixAudioSources(gameObject);
        }

        [EasyButtons.Button]
        public void GetAllColliders()
        {
            colliders = new List<Collider>();
            foreach (Collider c in this.gameObject.GetComponentsInChildren<Collider>())
            {
                colliders.Add(c);
            }
        }

        [EasyButtons.Button]
        public void GetAllHandles()
        {
            handles = new List<Handle>();
            foreach (Handle h in this.gameObject.GetComponentsInChildren<Handle>())
            {
                handles.Add(h);
            }
        }
    }
}
