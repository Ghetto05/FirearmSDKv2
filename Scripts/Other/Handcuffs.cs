using System;
using System.Collections;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;
using UnityEngine.UIElements;

namespace GhettosFirearmSDKv2
{
    public class Handcuffs : MonoBehaviour
    {
        public Item item;
        public bool canBeReopened;
        public bool destroyOnReopen;
        public Transform leftAnchor;
        public Transform rightAnchor;
        public Transform leftFootAnchor;
        public Transform rightFootAnchor;

        public Transform leftAxis;
        public Transform leftOpenedPosition;
        public Transform leftClosedPosition;
        public Transform rightAxis;
        public Transform rightOpenedPosition;
        public Transform rightClosedPosition;

        public GameObject closedLeftObject;
        public GameObject openedLeftObject;
        public GameObject closedRightObject;
        public GameObject openedRightObject;

        public AudioSource[] closeSounds;
        public AudioSource[] openSounds;

        public Collider leftTrigger;
        public Collider rightTrigger;

        public Collider[] leftColliders;
        public Collider[] rightColliders;

        private Joint _leftJoint;
        private Joint _rightJoint;
        private RagdollPart _leftConnectedPart;
        private RagdollPart _rightConnectedPart;
        [Space] [Space] [Space] public string _;

        // private void Start()
        // {
        //     item.OnHeldActionEvent += OnHeldAction;
        //     item.mainCollisionHandler.OnCollisionStartEvent += OnCollisionStartEvent;
        // }
        //
        // private void OnHeldAction(RagdollHand ragdollHand, Handle handle, Interactable.Action action)
        // {
        //     if (action == Interactable.Action.AlternateUseStart)
        //     {
        //         Unlock(false);
        //     }
        // }
        //
        // private void OnCollisionStartEvent(CollisionInstance collisionInstance)
        // {
        //     
        // }

        public void LockTo(RagdollPart part, Side side)
        {
            if (side == Side.Left)
            {
                _leftConnectedPart = part;
            }
            else
            {
                _rightConnectedPart = part;
            }
            ToggleCreaturePhysics(true);
            LockAnimation(side);
        }

        [EasyButtons.Button]
        public void Unlock(bool withTool)
        {
            if (canBeReopened || withTool)
            {
                Destroy(_leftJoint);
                Destroy(_rightJoint);
                ToggleCreaturePhysics(false);
                UnlockAnimation();
            }
        }

        [EasyButtons.Button]
        public void LockAnimation(Side side)
        {
            Util.PlayRandomAudioSource(closeSounds);

            Transform axis = side == Side.Left ? leftAxis : rightAxis;
            Transform target = side == Side.Left ? leftClosedPosition : rightClosedPosition;
            if (axis != null)
                axis.SetPositionAndRotation(target.position, target.rotation);
            
            if (closedLeftObject != null && side == Side.Left)
                closedLeftObject.SetActive(true);
            if (openedLeftObject != null && side == Side.Left)
                openedLeftObject.SetActive(false);
            if (closedRightObject != null && side == Side.Right)
                closedRightObject.SetActive(true);
            if (openedRightObject != null && side == Side.Right)
                openedRightObject.SetActive(false);
        }

        [EasyButtons.Button]
        public void UnlockAnimation()
        {
            Util.PlayRandomAudioSource(openSounds);

            if (leftAxis != null)
            {
                leftAxis.SetPositionAndRotation(leftOpenedPosition.position, leftOpenedPosition.rotation);
                rightAxis.SetPositionAndRotation(rightOpenedPosition.position, rightOpenedPosition.rotation);
            }
            
            if (closedLeftObject != null)
                closedLeftObject.SetActive(false);
            if (openedLeftObject != null)
                openedLeftObject.SetActive(true);
            if (closedRightObject != null)
                closedRightObject.SetActive(false);
            if (openedRightObject != null)
                openedRightObject.SetActive(true);
        }

        private void ToggleCreaturePhysics(bool forcedOn)
        {
            Creature c = _leftConnectedPart != null ? _leftConnectedPart.ragdoll.creature :
                _rightConnectedPart != null ? _rightConnectedPart.ragdoll.creature : null;

            if (c != null)
                c.ragdoll.physicToggle = !forcedOn;
        }
    }
}
