using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ThunderRoad;

namespace GhettosFirearmSDKv2
{
    public class StripperClip : MonoBehaviour
    {
        public string caliber;
        public string clipType;
        public int capacity;
        
        public Item item;
        public Transform[] cartridgePositions;
        public GameObject insertColliderRoot;
        public Collider insertCollider;
        public Collider mountCollider;
        public bool insertFromBottom;
        public Collider roundLoadCollider;
        public Transform roundEjectPoint;

        public AudioSource[] pushSounds;
        public AudioSource[] loadSounds;
        public AudioSource[] unloadSounds;
        public AudioSource[] mountSounds;
        public AudioSource[] removeSounds;

        public MagazineLoad defaultLoad;
        
        [HideInInspector]
        public List<Cartridge> loadedCartridges;

        private StripperClipWell _currentWell;

        private void Start()
        {
            //Invoke(nameof(InvokedStart), FirearmsSettings.invokeTime);
        }

        private void InvokedStart()
        {
            
        }

        private void FixedUpdate()
        {
            UpdatePositions();
        }

        private void UpdatePositions()
        {
            if (loadedCartridges.Count == 0)
            {
                insertColliderRoot.SetActive(false);
                return;
            }

            insertColliderRoot.SetActive(true);
            insertColliderRoot.transform.SetParent(cartridgePositions[loadedCartridges.Count - 1]);
            insertColliderRoot.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

            foreach (Cartridge c in loadedCartridges)
            {
                Quaternion rot = c.transform.localRotation;
                c.transform.SetParent(cartridgePositions[loadedCartridges.IndexOf(c)]);
                c.transform.SetLocalPositionAndRotation(Vector3.zero, rot);
            }
        }

        private void InsertRound(Cartridge c)
        {
            if (loadedCartridges.Count < capacity && !c.loaded && (c.caliber.Equals(caliber)/* || FirearmSettings.ignoreCaliberChecks*/))
            {
                Util.PlayRandomAudioSource(loadSounds);
                foreach (Handle handle in c.item.handles)
                {
                    handle.Release();
                }
                /*c.item.disallowDespawn = true;*/
                c.item.physicBody.isKinematic = true;
                c.item.transform.SetParent(cartridgePositions[0]);
                c.item.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(Util.RandomCartridgeRotation()));
                if (insertFromBottom)
                    loadedCartridges.Insert(0, c);
                else
                    loadedCartridges.Add(c);
                SaveContent();
            }
        }

        private void EjectRound()
        {
            if (loadedCartridges.Count > 0)
            {
                Util.PlayRandomAudioSource(unloadSounds);
                Cartridge c = insertFromBottom ? loadedCartridges[0] : loadedCartridges.Last();
                loadedCartridges.Remove(c);
                SaveContent();
                c.item.physicBody.isKinematic = false;
                c.item.transform.SetParent(null);
                /*c.item.disallowDespawn = false;*/
                c.item.transform.SetPositionAndRotation(roundEjectPoint.position, roundEjectPoint.rotation);
            }
        }

        private void PushRoundToMagazine()
        {
            if (loadedCartridges.Count > 0 && _currentWell.magazineWell.firearm.magazineWell != null && _currentWell.magazineWell.firearm.magazineWell.currentMagazine != null)
            {
                Magazine mag = _currentWell.magazineWell.firearm.magazineWell.currentMagazine;
                if (mag.cartridges.Count < mag.maximumCapacity)
                {
                    Cartridge c = loadedCartridges[0];
                    loadedCartridges.RemoveAt(0);
                    SaveContent();
                    mag.InsertRound(c, true, true);
                    Util.PlayRandomAudioSource(pushSounds);
                }
            }
        }

        public void MountToGun(StripperClipWell well)
        {
            if (_currentWell != null || (!well.clipType.Equals(clipType) /*&& !FirearmSettings.ignoreMagazineTypeChecks*/))
                return;

            _currentWell = well;
            foreach (Handle handle in item.handles)
            {
                handle.Release();
            }
            /*item.disallowDespawn = true;*/
            item.physicBody.isKinematic = true;
            item.transform.SetParent(_currentWell.mountPoint);
            Util.PlayRandomAudioSource(mountSounds);
        }

        public void RemoveFromGun()
        {
            if (_currentWell == null)
                return;
            
            /*item.disallowDespawn = false;*/
            item.physicBody.isKinematic = false;
            item.transform.SetParent(null);
            _currentWell = null;
            Util.PlayRandomAudioSource(removeSounds);
        }

        private void SaveContent()
        {
            
        }
    }
}
