using System;
using System.Collections;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    public class FlintLock : BoltBase
    {
        [Header("Firing")]
        public float fireDelay;
        public ParticleSystem panEffect;
        public PowderReceiver mainReceiver;
        public float baseRecoil = 20;

        [Header("Hammer")]
        public Transform hammer;
        public Transform hammerIdlePosition;
        public Transform hammerCockedPosition;
        private bool _hammerState;

        [Header("Pan")]
        public Transform pan;
        public Transform panOpenedPosition;
        public Transform panClosedPosition;
        public PowderReceiver panReceiver;
        private bool _panClosed;

        [Header("Round loading")]
        public string caliber;
        public Collider roundInsertCollider;
        public Transform roundMountPoint;
        public Cartridge loadedCartridge;
        private float lastRoundPosition;
        public Transform roundEjectDir;
        public Transform roundEjectPoint;
        public float roundEjectForce;

        [Header("Ram rod")]
        public Transform rodFrontEnd;
        public Transform rodRearEnd;
        public string ramRodItem;
        private Item currentRamRod;
        public Collider ramRodInsertCollider;
        private ConfigurableJoint joint;
        private bool rodAwayFromBreach;
        public bool flipRodOnInsert;

        [Header("Ram rod store")]
        public Transform rodStoreFrontEnd;
        public Transform rodStoreRearEnd;
        private Item currentStoredRamRod;
        public Collider ramRodStoreInsertCollider;
        private ConfigurableJoint storeJoint;
        private bool rodAwayFromStoreEnd;

        [Header("Audio")]
        public AudioSource[] sizzleSound;
        [Space]
        public AudioSource[] hammerCockSounds;
        public AudioSource[] hammerFireSounds;
        [Space]
        public AudioSource[] panOpenSounds;
        public AudioSource[] panCloseSounds;
        [Space]
        public AudioSource[] ramRodInsertSound;
        public AudioSource[] ramRodExtractSound;
        [Space]
        public AudioSource[] ramRodStoreInsertSound;
        public AudioSource[] ramRodStoreExtractSound;
        [Space]
        public AudioSource[] roundInsertSounds;

        private void Start()
        {
            OpenPan(true);
            Invoke(nameof(InvokedStart), 0.5f);
        }

        private void InvokedStart()
        {
            firearm.OnCollisionEvent += FirearmOnOnCollisionEvent;
        }

        private void FirearmOnOnCollisionEvent(Collision collision)
        {
            if (collision.rigidbody.TryGetComponent(out Item hitItem))
            {
                if (currentRamRod == null && hitItem.itemId.Equals(ramRodItem) && Util.CheckForCollisionWithThisCollider(collision, ramRodInsertCollider))
                {
                    //InitializeRamRodJoint(hitItem.physicBody.rigidBody);
                    InitializeRamRodJoint(collision.rigidbody);
                    currentRamRod = hitItem;
                    rodAwayFromBreach = false;
                    Util.PlayRandomAudioSource(ramRodInsertSound);
                }
                else
                {
                    Debug.Log($"{currentRamRod == null} {hitItem.itemId.Equals(ramRodItem)} {Util.CheckForCollisionWithThisCollider(collision, ramRodInsertCollider)}");
                }
            }
            else
            {
                Debug.Log("No item");
            }
        }

        [EasyButtons.Button]
        public override void TryFire()
        {
            if (!_hammerState)
            {
                //INVOKEFINISH
                return;
            }

            Util.PlayRandomAudioSource(hammerFireSounds);
            hammer.SetPositionAndRotation(hammerIdlePosition.position, hammerIdlePosition.rotation);
            _hammerState = false;

            if (!_panClosed)
            {
                //INVOKEFINISH
                return;
            }

            OpenPan();

            if (!panReceiver.Sufficient())
            {
                //INVOKEFINISH
                return;
            }

            panReceiver.currentAmount = 0;
            Util.PlayRandomAudioSource(sizzleSound);
            if (panEffect != null)
                panEffect.Play();

            Invoke(nameof(DelayedFire), fireDelay);

            base.TryFire();
        }

        public void DelayedFire()
        {
            if (!mainReceiver.Sufficient())
            {
                //INVOKEFINISH
                return;
            }

            mainReceiver.currentAmount = 0;
            Util.PlayRandomAudioSource(firearm.fireSounds);
            firearm.defaultMuzzleFlash?.Play();
        }

        [EasyButtons.Button]
        public void CockHammer()
        {
            if (_hammerState)
                return;
            Util.PlayRandomAudioSource(hammerCockSounds);
            hammer.SetPositionAndRotation(hammerCockedPosition.position, hammerCockedPosition.rotation);
            _hammerState = true;
        }

        [EasyButtons.Button]
        public void OpenPan(bool forced = false)
        {
            if (!_panClosed && !forced)
                return;
            if (!forced)
                Util.PlayRandomAudioSource(panOpenSounds);
            pan.SetPositionAndRotation(panOpenedPosition.position, panOpenedPosition.rotation);
            _panClosed = false;
            panReceiver.blocked = false;
        }

        [EasyButtons.Button]
        public void ClosePan()
        {
            if (_panClosed || !_hammerState)
                return;
            Util.PlayRandomAudioSource(panCloseSounds);
            pan.SetPositionAndRotation(panClosedPosition.position, panClosedPosition.rotation);
            _panClosed = true;
            panReceiver.blocked = true;
        }

        private void FixedUpdate()
        {
            mainReceiver.blocked = loadedCartridge != null || currentRamRod != null;

            if (currentRamRod != null && !rodAwayFromBreach &&
                Vector3.Distance(currentRamRod.transform.position, rodFrontEnd.position) > 0.05f)
                rodAwayFromBreach = true;

            if (currentRamRod != null && rodAwayFromBreach &&
                Vector3.Distance(currentRamRod.transform.position, rodFrontEnd.position) < 0.02f)
            {
                InitializeRamRodJoint(null);
                currentRamRod = null;
                Util.PlayRandomAudioSource(ramRodExtractSound);
            }

            if (currentRamRod != null && loadedCartridge != null)
            {
                float currentPos = Vector3.Distance(rodRearEnd.position, currentRamRod.transform.position);
                float targetPos = Vector3.Distance(rodFrontEnd.position, rodFrontEnd.position);
                float posTime = currentPos / targetPos;
                if (posTime > lastRoundPosition)
                    lastRoundPosition = posTime;
                loadedCartridge.transform.position = Vector3.Lerp(rodFrontEnd.position, rodRearEnd.position, lastRoundPosition);
            }
        }

        private void InitializeRamRodJoint(Rigidbody rodRb)
        {
            if (joint != null)
#if UNITY_EDITOR
                DestroyImmediate(joint);
#else
                Destroy(joint);
#endif
            if (rodRb == null)
            {
                Debug.Log("No RB");
                return;
            }

            rodRb.position = rodFrontEnd.position;
            rodRb.rotation = rodFrontEnd.rotation;
            joint = firearm.item.gameObject.AddComponent<ConfigurableJoint>();
            joint.linearLimit = new SoftJointLimit
            {
                limit = Vector3.Distance(rodFrontEnd.position, rodRearEnd.position) / 2
            };
            //joint.axis = Vector3.forward;
            //joint.secondaryAxis = Vector3.forward;
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = Vector3.zero;
            joint.anchor = new Vector3(GrandparentLocalPosition(rodRearEnd, firearm.item.transform).x, GrandparentLocalPosition(rodRearEnd, firearm.item.transform).y, GrandparentLocalPosition(rodRearEnd, firearm.item.transform).z + ((rodFrontEnd.localPosition.z - rodRearEnd.localPosition.z) / 2));
            joint.xMotion = ConfigurableJointMotion.Locked;
            joint.yMotion = ConfigurableJointMotion.Locked;
            joint.zMotion = ConfigurableJointMotion.Limited;
            joint.angularXMotion = ConfigurableJointMotion.Locked;
            joint.angularYMotion = ConfigurableJointMotion.Locked;
            joint.angularZMotion = ConfigurableJointMotion.Locked;
            joint.connectedBody = rodRb;
        }
    }
}