using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThunderRoad;
using System;
using UnityEngine.Assertions;
using System.Reflection;

namespace GhettosFirearmSDKv2
{
    public class GhettoHandle : Handle
    {
        public enum HandleType
        {
            Foregrip,
            MainGrip,
            Bolt,
            PumpAction
        }

        public HandleType type;

        public static void CopyValues(Handle originalHandle, GhettoHandle newHandle)
        {
            newHandle.allowedHandSide = originalHandle.allowedHandSide;
            newHandle.artificialDistance = originalHandle.artificialDistance;
            newHandle.axisLength = originalHandle.axisLength;
            newHandle.customRigidBody = originalHandle.customRigidBody;
            newHandle.data = originalHandle.data;
            newHandle.defaultGrabAxisRatio = originalHandle.defaultGrabAxisRatio;
            newHandle.forceAutoDropWhenGrounded = originalHandle.forceAutoDropWhenGrounded;
            newHandle.handOverlapColliders = originalHandle.handOverlapColliders;
            newHandle.ignoreClimbingForceOverride = originalHandle.ignoreClimbingForceOverride;
            newHandle.ikAnchorOffset = originalHandle.ikAnchorOffset;
            newHandle.interactableId = originalHandle.interactableId;
            newHandle.moveToHandle = originalHandle.moveToHandle;
            newHandle.moveToHandleAxisPos = originalHandle.moveToHandleAxisPos;
            newHandle.orientationDefaultLeft = originalHandle.orientationDefaultLeft;
            newHandle.orientationDefaultRight = originalHandle.orientationDefaultRight;
            newHandle.orientations = originalHandle.orientations;
            newHandle.reach = originalHandle.reach;
            newHandle.releaseHandle = originalHandle.releaseHandle;
            newHandle.silentGrab = originalHandle.silentGrab;
            newHandle.slideBehavior = originalHandle.slideBehavior;
            newHandle.slideToBottomHandle = originalHandle.slideToBottomHandle;
            newHandle.slideToHandleOffset = originalHandle.slideToHandleOffset;
            newHandle.slideToUpHandle = originalHandle.slideToUpHandle;
            newHandle.touchActive = originalHandle.touchActive;
            newHandle.touchCenter = originalHandle.touchCenter;
            newHandle.touchCollider = originalHandle.touchCollider;
            newHandle.touchRadius = originalHandle.touchRadius;
            newHandle.updatePosesAutomatically = originalHandle.updatePosesAutomatically;
        }

        public static GhettoHandle ReplaceHandle(Handle handle)
        {
            if (handle.GetType() == typeof(GhettoHandle)) return (GhettoHandle)handle;
            GhettoHandle newhand = handle.gameObject.AddComponent<GhettoHandle>();
            CopyValues(handle, newhand);
            DestroyImmediate(handle, true);
            return newhand;
        }
    }
}