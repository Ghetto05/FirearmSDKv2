using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Bolt assemblies/Automatic bolt")]
    public class BoltSemiautomatic : BoltBase
    {
        public bool isOpenBolt = false;
        [Space]
        public List<AttachmentPoint> onBoltPoints;
        [Space]
        public bool locksWhenSafetyIsOn = false;
        [Space]
        public bool hasBoltcatch = true;
        public bool hasBoltCatchReleaseControl = true;
        public bool onlyCatchIfManuallyPulled = false;
        public bool chargingHandleLocksBack = false;
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
        public AudioSource[] catchOnSearSounds;
        [Space]
        public float roundEjectForce = 0.6f;
        public Transform roundEjectDir;
        public Transform roundEjectPoint;
        [Space]
        public Hammer hammer;
        public bool cockHammerOnTriggerPull;

        private void Awake()
        {
            //InitializeJoint(false);
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

        [EasyButtons.Button]
        public void SetupDefault()
        {
            if (rigidBody == null)
            {
                GameObject objRB = new GameObject();
                objRB.transform.SetParent(transform);
                objRB.transform.localPosition = Vector3.zero;
                objRB.transform.localEulerAngles = Vector3.zero;
                objRB.name = "RB";
                Rigidbody rrb = objRB.AddComponent<Rigidbody>();
                ConstantForce cf = objRB.AddComponent<ConstantForce>();
                cf.relativeForce = Vector3.forward * 100;
                force = cf;
                rrb.useGravity = false;
                rrb.angularDrag = 0f;
                rigidBody = rrb;

                GameObject objHandle = new GameObject();
                objHandle.transform.SetParent(objRB.transform);
                objHandle.name = "BoltHandle";
                objHandle.transform.localPosition = Vector3.zero;
                objHandle.transform.localEulerAngles = Vector3.zero;
                GhettoHandle han = objHandle.AddComponent<GhettoHandle>();
                han.customRigidBody = rigidBody;
                han.type = GhettoHandle.HandleType.Bolt;
                han.CheckOrientations();
            }

            if (bolt == null)
            {
                GameObject objObj = new GameObject();
                objObj.transform.SetParent(transform);
                objObj.transform.localPosition = Vector3.zero;
                objObj.transform.localEulerAngles = Vector3.zero;
                bolt = objObj.transform;
                objObj.name = "Obj";
            }

            if (startPoint == null)
            {
                GameObject objStart = new GameObject();
                objStart.transform.SetParent(transform);
                objStart.transform.localPosition = Vector3.zero;
                objStart.transform.localEulerAngles = Vector3.zero;
                startPoint = objStart.transform;
                objStart.name = "Start";
            }

            if (endPoint == null)
            {
                GameObject objEnd = new GameObject();
                objEnd.transform.SetParent(transform);
                objEnd.transform.localPosition = Vector3.zero;
                objEnd.transform.localEulerAngles = Vector3.zero;
                endPoint = objEnd.transform;
                objEnd.name = "End";
            }

            if (catchPoint == null)
            {
                GameObject objCatch = new GameObject();
                objCatch.transform.SetParent(transform);
                objCatch.transform.localPosition = Vector3.zero;
                objCatch.transform.localEulerAngles = Vector3.zero;
                catchPoint = objCatch.transform;
                objCatch.name = "Catch";
            }

            if (roundLoadPoint == null)
            {
                GameObject objLoad = new GameObject();
                objLoad.transform.SetParent(transform);
                objLoad.transform.localPosition = Vector3.zero;
                objLoad.transform.localEulerAngles = Vector3.zero;
                roundLoadPoint = objLoad.transform;
                objLoad.name = "Load";
            }

            if (hammerCockPoint == null)
            {
                GameObject objCock = new GameObject();
                objCock.transform.SetParent(transform);
                objCock.transform.localPosition = Vector3.zero;
                objCock.transform.localEulerAngles = Vector3.zero;
                hammerCockPoint = objCock.transform;
                objCock.name = "Cock";
            }

            if (roundMount == null)
            {
                GameObject objRoundMount = new GameObject();
                objRoundMount.transform.SetParent(bolt);
                objRoundMount.transform.localPosition = Vector3.zero;
                objRoundMount.transform.localEulerAngles = Vector3.zero;
                roundMount = objRoundMount.transform;
                objRoundMount.name = "RoundMount";
            }

            if (roundEjectDir == null)
            {
                GameObject objEjectDir = new GameObject();
                objEjectDir.transform.SetParent(bolt);
                objEjectDir.transform.localPosition = Vector3.zero;
                objEjectDir.transform.localEulerAngles = Vector3.zero;
                roundEjectDir = objEjectDir.transform;
                objEjectDir.name = "EjectDir";
            }
        }
    }
}
