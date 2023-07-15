using EasyButtons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    public class RevolverEjectorRod : MonoBehaviour
    {
        public Revolver revolver;
        public Rigidbody rigidBody;
        public Transform axis;
        public Transform root;
        public Transform ejectPoint;
        private ConfigurableJoint joint;

        private void Awake()
        {
            InitializeJoint();
            Util.IgnoreCollision(axis.gameObject, revolver.firearm.gameObject, true);
            Revolver_onClose();
        }

        [Button]
        public void Revolver_onOpen()
        {
            axis.SetParent(rigidBody.transform);
            axis.localPosition = Vector3.zero;
            axis.localEulerAngles = Vector3.zero;
        }

        [Button]
        private void Revolver_onClose()
        {
            axis.SetParent(root);
            axis.localPosition = Vector3.zero;
            axis.localEulerAngles = Vector3.zero;
        }

        public void InitializeJoint()
        {
            if (joint == null)
            {
                joint = revolver.rotateBody.gameObject.AddComponent<ConfigurableJoint>();
                joint.connectedBody = rigidBody;
                joint.massScale = 0.00001f;
            }
            SoftJointLimit limit = new SoftJointLimit();
            Vector3 vec = BoltBase.GrandparentLocalPosition(ejectPoint, revolver.rotateBody.transform);
            joint.anchor = new Vector3(vec.x, vec.y, vec.z + ((root.localPosition.z - ejectPoint.localPosition.z) / 2));
            limit.limit = Vector3.Distance(ejectPoint.position, root.position) / 2;
            joint.linearLimit = limit;
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = Vector3.zero;
            joint.xMotion = ConfigurableJointMotion.Locked;
            joint.yMotion = ConfigurableJointMotion.Locked;
            joint.zMotion = ConfigurableJointMotion.Limited;
            joint.angularXMotion = ConfigurableJointMotion.Locked;
            joint.angularYMotion = ConfigurableJointMotion.Locked;
            joint.angularZMotion = ConfigurableJointMotion.Locked;
            rigidBody.transform.localPosition = root.localPosition;
            rigidBody.transform.localRotation = root.localRotation;
        }
    }
}
