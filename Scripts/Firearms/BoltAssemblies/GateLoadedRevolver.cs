using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThunderRoad;
using UnityEngine.Serialization;

namespace GhettosFirearmSDKv2
{
    public class GateLoadedRevolver : BoltBase
    {
        private bool loadMode = false;
        private bool cocked = false;
        private int currentChamber;

        [Header("Cylinder")]
        public Transform cylinderAxis;
        public Vector3[] cylinderRotations;
        public Vector3[] cylinderLoadRotations;
        public int loadChamberOffset;
        [Space]
        public string[] calibers;
        public Cartridge[] loadedCartridges;
        public Transform[] mountPoints;

        [Header("Load Gate")]
        public Transform loadGateAxis;
        public Transform loadGateClosedPosition;
        public Transform loadGateOpenedPosition;
        public Collider loadCollider;

        [Header("Hammer")]
        public Transform hammerAxis;
        public Transform hammerIdlePosition;
        public Transform hammerCockedPosition;
        public Transform hammerLoadPosition;
        public Collider cockCollider;

        [Header("Ejector Rod")]
        public Transform roundReparent;
        public Rigidbody ejectorRb;
        public Transform ejectorAxis;
        public Transform ejectorRoot;
        public Transform ejectorEjectPoint;
        private ConfigurableJoint _ejectorJoint;
        public float ejectForce;
        public Transform ejectDir;
        public Transform springRoot;
        public Vector3 springTargetScale = Vector3.one;

        [Header("Audio")]
        public List<AudioSource> insertSounds;
        public List<AudioSource> ejectSounds;
        public List<AudioSource> openSounds;
        public List<AudioSource> closeSounds;
        public List<AudioSource> hammerCockSounds;
        public List<AudioSource> hammerHitSounds;

        private void Awake()
        {
            UpdateEjector();
        }

        [EasyButtons.Button]
        public void AltPress()
        {
            loadMode = !loadMode;
            ApplyChamber();
            if (loadMode)
            {
                loadGateAxis.localPosition = loadGateOpenedPosition.localPosition;
                loadGateAxis.localEulerAngles = loadGateOpenedPosition.localEulerAngles;

                hammerAxis.localPosition = hammerCockedPosition.localPosition;
                hammerAxis.localEulerAngles = hammerCockedPosition.localEulerAngles;
            }
            else
            {
                loadGateAxis.localPosition = loadGateClosedPosition.localPosition;
                loadGateAxis.localEulerAngles = loadGateClosedPosition.localEulerAngles;

                if (!cocked)
                {
                    hammerAxis.localPosition = hammerIdlePosition.localPosition;
                    hammerAxis.localEulerAngles = hammerIdlePosition.localEulerAngles;
                }
            }
            UpdateEjector();
        }

        public void UpdateEjector()
        {
            InitializeEjectorJoint();
            if (loadMode)
            {
                Vector3 vec = BoltBase.GrandparentLocalPosition(ejectorEjectPoint, firearm.item.transform);
                _ejectorJoint.anchor = new Vector3(vec.x, vec.y, vec.z + ((ejectorRoot.localPosition.z - ejectorEjectPoint.localPosition.z) / 2));
                SoftJointLimit limit = new SoftJointLimit();
                limit.limit = Vector3.Distance(ejectorEjectPoint.position, ejectorRoot.position) / 2;
                _ejectorJoint.linearLimit = limit;

                ejectorAxis.SetParent(ejectorRb.transform);
                ejectorAxis.localPosition = Vector3.zero;
                ejectorAxis.localEulerAngles = Vector3.zero;
            }
            else
            {
                Vector3 vec = BoltBase.GrandparentLocalPosition(ejectorRoot, firearm.item.transform);
                _ejectorJoint.anchor = new Vector3(vec.x, vec.y, vec.z);
                SoftJointLimit limit = new SoftJointLimit();
                limit.limit = 0f;
                _ejectorJoint.linearLimit = limit;

                ejectorAxis.SetParent(ejectorRoot);
                ejectorAxis.localPosition = Vector3.zero;
                ejectorAxis.localEulerAngles = Vector3.zero;
            }
        }

        public void InitializeEjectorJoint()
        {
            if (_ejectorJoint == null)
            {
                _ejectorJoint = firearm.item.gameObject.AddComponent<ConfigurableJoint>();
                _ejectorJoint.connectedBody = ejectorRb;
                _ejectorJoint.massScale = 0.00001f;

                _ejectorJoint.autoConfigureConnectedAnchor = false;
                _ejectorJoint.connectedAnchor = Vector3.zero;
                _ejectorJoint.xMotion = ConfigurableJointMotion.Locked;
                _ejectorJoint.yMotion = ConfigurableJointMotion.Locked;
                _ejectorJoint.zMotion = ConfigurableJointMotion.Limited;
                _ejectorJoint.angularXMotion = ConfigurableJointMotion.Locked;
                _ejectorJoint.angularYMotion = ConfigurableJointMotion.Locked;
                _ejectorJoint.angularZMotion = ConfigurableJointMotion.Locked;
                ejectorRb.transform.localPosition = ejectorRoot.localPosition;
                ejectorRb.transform.localRotation = ejectorRoot.localRotation;
            }
        }

        [EasyButtons.Button]
        public void TriggerPress()
        {
            if (loadMode)
            {
                currentChamber++;
                if (currentChamber >= cylinderRotations.Length) currentChamber = 0;
                ApplyChamber();
            }
            else
            {
                if (cocked)
                {
                    cocked = false;
                    hammerAxis.localPosition = hammerIdlePosition.localPosition;
                    hammerAxis.localEulerAngles = hammerIdlePosition.localEulerAngles;
                    //fire if chamber is loaded
                }
            }
        }

        [EasyButtons.Button]
        public void Cock()
        {
            if (loadMode) return;
            if (!cocked)
            {
                cocked = true;
                hammerAxis.localPosition = hammerCockedPosition.localPosition;
                hammerAxis.localEulerAngles = hammerCockedPosition.localEulerAngles;
                currentChamber++;
                if (currentChamber >= cylinderRotations.Length) currentChamber = 0;
                ApplyChamber();
            }
            else
            {
                cocked = false;
                hammerAxis.localPosition = hammerIdlePosition.localPosition;
                hammerAxis.localEulerAngles = hammerIdlePosition.localEulerAngles;
            }
        }

        public void ApplyChamber()
        {
            cylinderAxis.localEulerAngles = loadMode ? cylinderLoadRotations[currentChamber] : cylinderRotations[currentChamber];
        }
    }
}
