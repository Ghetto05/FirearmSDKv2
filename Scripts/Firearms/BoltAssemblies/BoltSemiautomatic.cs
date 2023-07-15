using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Bolt assemblies/Automatic bolt")]
    public class BoltSemiautomatic : BoltBase
    {
        public List<AttachmentPoint> onBoltPoints;
        [Space]
        public bool locksWhenSafetyIsOn = false;
        [Space]
        public bool hasBoltcatch = true;
        public bool hasBoltCatchReleaseControl = true;
        public bool onlyCatchIfManuallyPulled = false;
        public bool lockIfNoMagazineFound = false;
        public BoltReleaseButton[] releaseButtons;
        [Space]
        public Rigidbody rigidBody;
        public Transform bolt;
        public Transform chargingHandle;
        [Space]
        public Transform startPoint;
        public Transform endPoint;
        public Transform akBoltLockPoint;
        public Transform catchPoint;
        public Transform roundLoadPoint;
        public Transform hammerCockPoint;
        [Space]
        public Transform roundMount;
        public Cartridge loadedCartridge;
        [Space]
        private ConfigurableJoint joint;
        public ConstantForce force;
        public float pointTreshold = 0.004f;
        [Space]
        public AudioSource[] rackSounds;
        public AudioSource[] pullSounds;
        public AudioSource[] chargingHandleRackSounds;
        public AudioSource[] chargingHandlePullSounds;
        public AudioSource[] rackSoundsHeld;
        public AudioSource[] pullSoundsHeld;
        public AudioSource[] rackSoundsNotHeld;
        public AudioSource[] pullSoundsNotHeld;
        [Space]
        public float roundEjectForce = 0.6f;
        public Transform roundEjectDir;
        public Transform roundEjectPoint;
        [Space]
        public Hammer hammer;

        private void Awake()
        {
            InitializeJoint(false);
        }

        private void InitializeJoint(bool lockedBack, bool safetyLocked = false)
        {
            ConfigurableJoint pJoint = firearm.gameObject.AddComponent<ConfigurableJoint>();
            pJoint.connectedBody = rigidBody;
            pJoint.massScale = 0.00001f;
            SoftJointLimit limit = new SoftJointLimit();
            //default, start to back movement
            if (!lockedBack && !safetyLocked)
            {
                pJoint.anchor = new Vector3(endPoint.localPosition.x, endPoint.localPosition.y, endPoint.localPosition.z + ((startPoint.localPosition.z - endPoint.localPosition.z) / 2));
                limit.limit = Vector3.Distance(endPoint.position, startPoint.position) / 2;
            }
            //locked back, between end point and lock point movement
            else if (lockedBack && !safetyLocked)
            {
                pJoint.anchor = new Vector3(endPoint.localPosition.x, endPoint.localPosition.y, endPoint.localPosition.z + ((catchPoint.localPosition.z - endPoint.localPosition.z) / 2));
                limit.limit = Vector3.Distance(endPoint.position, catchPoint.position) / 2;
            }
            else if (safetyLocked && !lockedBack)
            //locked front by safety, between start and ak lock point movement
            {
                pJoint.anchor = new Vector3(startPoint.localPosition.x, startPoint.localPosition.y, startPoint.localPosition.z + ((akBoltLockPoint.localPosition.z - startPoint.localPosition.z) / 2));
                limit.limit = Vector3.Distance(startPoint.position, akBoltLockPoint.position) / 2;
            }
            pJoint.linearLimit = limit;
            pJoint.autoConfigureConnectedAnchor = false;
            pJoint.connectedAnchor = Vector3.zero;
            pJoint.xMotion = ConfigurableJointMotion.Locked;
            pJoint.yMotion = ConfigurableJointMotion.Locked;
            if (!lockedBack) pJoint.zMotion = ConfigurableJointMotion.Limited;
            else pJoint.zMotion = ConfigurableJointMotion.Free;
            pJoint.angularXMotion = ConfigurableJointMotion.Locked;
            pJoint.angularYMotion = ConfigurableJointMotion.Locked;
            pJoint.angularZMotion = ConfigurableJointMotion.Locked;
            ConfigurableJoint prevJoint = joint;
            joint = pJoint;
            Destroy(prevJoint);
            if (lockedBack)
            {
                rigidBody.transform.localPosition = catchPoint.localPosition;
                rigidBody.transform.localRotation = catchPoint.localRotation;
            }
            else
            {
                rigidBody.transform.localPosition = startPoint.localPosition;
                rigidBody.transform.localRotation = startPoint.localRotation;
            }
        }
    }
}
