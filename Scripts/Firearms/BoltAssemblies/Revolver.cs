using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThunderRoad;

namespace GhettosFirearmSDKv2
{
    public class Revolver : BoltBase
    {
        [Header("FOLDING")]
        public List<Handle> foregripHandles;
        public List<AttachmentPoint> foldAttachmentPoints;
        private HingeJoint foldJoint;
        public Rigidbody foldBody;
        public Transform foldAxis;
        public Vector3 foldingAxis;
        public float minFoldAngle;
        public float maxFoldAngle;
        public float foldingDamper = 0.07f;
        [Space]
        public Transform foldClosedPosition;
        public Transform foldOpenedPosition;
        [Space]
        public Transform latchAxis;
        public Transform latchClosedPosition;
        public Transform latchOpenedPosition;

        [Space]
        [Header("ROTATE")]
        public Rigidbody rotateBody;
        public Transform rotateAxis;
        public Vector3 rotatingAxis;
        public Transform rotateRoot;
        public float rotatingDamper = 0.3f;
        [Space]
        public Transform chamberPicker;
        public List<Transform> chamberLocators;
        public List<float> chamberRotations;
        private HingeJoint rotateJoint;

        [Space]
        [Header("TRIGGER")]
        public float EDITORTriggerPullPercentage;
        public float triggerPullForTrigger = 0.3f;
        public float triggerPullMax = 1f;
        public Transform triggerAxis;
        public Transform triggerIdlePosition;
        public Transform triggerPulledPosition;
        [HideInInspector]
        public float triggerPull;
        public List<AudioSource> triggerPullSound;
        public List<AudioSource> triggerResetSound;
        public float onTriggerWeight = 0.8f;

        [HideInInspector]
        public bool cocked;
        [HideInInspector]
        public bool returnedTriggerSinceHammer = true;
        [Space]
        [Header("HAMMER")]
        public bool singleActionOnly = false;
        public bool pullHammerWhenOpened = false;
        public Transform hammerAxis;
        public Transform hammerIdlePosition;
        public Transform hammerCockedPosition;
        public List<AudioSource> hammerHitSounds;
        public List<AudioSource> hammerCockSounds;
        public Collider cockCollider;

        [Space]
        [Header("LOADING")]
        public bool autoEject = false;
        public Transform ejectDir;
        public float ejectForce;
        public List<string> calibers;
        public List<Transform> mountPoints;
        public List<Collider> loadColliders;
        private Cartridge[] loadedCartridges;
        public Transform ejectorRoot;
        public Transform ejectorStart;
        public Transform ejectorEnd;

        [Space]
        [Header("AUDIO")]
        public List<AudioSource> lockSounds;
        public List<AudioSource> unlockSounds;
        public List<AudioSource> ejectSounds;
        public List<AudioSource> loadSounds;

        bool closed = false;

        private void Awake()
        {
            Lock();
        }

        private void FixedUpdate()
        {
            triggerPull = Mathf.Clamp01(EDITORTriggerPullPercentage / triggerPullMax);
            if (Mathf.Approximately(triggerPull, 0f)) returnedTriggerSinceHammer = true;
            if (hammerAxis != null)
            {
                if (!cocked && triggerPull >= 1f && returnedTriggerSinceHammer)
                {
                    hammerAxis.localEulerAngles = hammerCockedPosition.localEulerAngles;
                    cocked = true;
                    Fire();
                }
                if (!cocked && returnedTriggerSinceHammer)
                {
                    hammerAxis.localEulerAngles = new Vector3(Mathf.Lerp(hammerIdlePosition.localEulerAngles.x, hammerCockedPosition.localEulerAngles.x, triggerPull), 0, 0);
                }
            }
            triggerAxis.localEulerAngles = new Vector3(Mathf.Lerp(triggerIdlePosition.localEulerAngles.x, triggerPulledPosition.localEulerAngles.x, triggerPull), 0, 0);
            if (((cocked && returnedTriggerSinceHammer) || hammerAxis == null) && triggerPull >= triggerPullForTrigger)
            {
                Fire();
            }

            if (closed)
            {
                foldAxis.SetParent(foldClosedPosition.parent);
                foldAxis.localPosition = foldClosedPosition.localPosition;
                foldAxis.localEulerAngles = foldClosedPosition.localEulerAngles;

                rotateAxis.SetParent(rotateRoot.parent);
                rotateAxis.localPosition = rotateRoot.localPosition;
                rotateAxis.localEulerAngles = new Vector3(0, 0, rotateAxis.localEulerAngles.z);
            }
            else
            {
                foldAxis.SetParent(foldBody.transform);
                foldAxis.localPosition = Vector3.zero;
                foldAxis.localEulerAngles = Vector3.zero;

                rotateAxis.SetParent(rotateBody.transform);
                rotateAxis.localPosition = Vector3.zero;
                rotateAxis.localEulerAngles = new Vector3(0, 0, rotateAxis.localEulerAngles.z);
            }
        }

        [EasyButtons.Button]
        public void Cock()
        {
            hammerAxis.localEulerAngles = hammerCockedPosition.localEulerAngles;
            cocked = true;
        }

        public void Fire()
        {
            Debug.Log("Fired");
            if (hammerAxis != null)
            {
                returnedTriggerSinceHammer = false;
                hammerAxis.localEulerAngles = hammerIdlePosition.localEulerAngles;
                cocked = false;
            }
        }

        [EasyButtons.Button]
        public void Lock()
        {
            if (closed) return;
            closed = true;

            InitializeFoldJoint(true);
            InitializeRotateJoint(true);
        }

        public Quaternion ToQuaternion(Vector3 vec)
        {
            return Quaternion.Euler(vec.x, vec.y, vec.z);
        }

        [EasyButtons.Button]
        public void Unlock()
        {
            if (!closed) return;
            closed = false;

            InitializeFoldJoint(false);
            InitializeRotateJoint(false);
        }

        public void InitializeFoldJoint(bool closed)
        {
            if (closed)
            {
                foldBody.transform.localPosition = foldClosedPosition.localPosition;
                foldBody.transform.eulerAngles = foldClosedPosition.eulerAngles;

                foldAxis.SetParent(foldClosedPosition.transform);
                foldAxis.localPosition = Vector3.zero;
                foldAxis.localEulerAngles = Vector3.zero;
            }
            else
            {
                foldBody.transform.localPosition = foldClosedPosition.localPosition;
                foldBody.transform.eulerAngles = foldClosedPosition.eulerAngles;

                foldAxis.SetParent(foldBody.transform);
                foldAxis.localPosition = Vector3.zero;
                foldAxis.localEulerAngles = Vector3.zero;
            }

            if (foldJoint == null)
            {
                foldJoint = firearm.item.gameObject.AddComponent<HingeJoint>();
                foldJoint.connectedBody = foldBody;
                foldJoint.massScale = 0.00001f;
                foldJoint.enableCollision = false;
            }
            foldJoint.autoConfigureConnectedAnchor = false;
            foldJoint.anchor = GrandparentLocalPosition(foldClosedPosition.transform, firearm.item.transform);
            foldJoint.connectedAnchor = Vector3.zero;
            foldJoint.axis = foldingAxis;
            foldJoint.useLimits = true;
            foldJoint.limits = closed ? new JointLimits() { min = 0f, max = 0f } : new JointLimits() { min = minFoldAngle, max = maxFoldAngle };
        }

        public void InitializeRotateJoint(bool closed)
        {
            if (closed)
            {
                rotateBody.transform.localPosition = rotateRoot.localPosition;
                rotateBody.transform.localEulerAngles = rotateRoot.localEulerAngles;

                rotateAxis.SetParent(rotateRoot);
                rotateAxis.localPosition = Vector3.zero;
                rotateAxis.localEulerAngles = new Vector3(0, 0, chamberRotations[0]);
            }
            else
            {
                rotateBody.transform.localPosition = rotateRoot.localPosition;
                rotateBody.transform.localEulerAngles = new Vector3(0, 0, chamberRotations[0]);

                rotateAxis.SetParent(rotateBody.transform);
                rotateAxis.localPosition = Vector3.zero;
                rotateAxis.localEulerAngles = Vector3.zero;
            }

            if (rotateJoint == null)
            {
                rotateJoint = foldBody.gameObject.AddComponent<HingeJoint>();
                rotateJoint.connectedBody = rotateBody;
                rotateJoint.massScale = 0.00001f;
            }
            rotateJoint.enableCollision = false;
            rotateJoint.autoConfigureConnectedAnchor = false;
            rotateJoint.anchor = GrandparentLocalPosition(rotateRoot.transform, foldBody.transform);
            rotateJoint.connectedAnchor = Vector3.zero;
            rotateJoint.axis = rotatingAxis;
            rotateJoint.useLimits = closed;
            rotateJoint.limits = new JointLimits() { min = 0f, max = 0f };
        }
    }
}
