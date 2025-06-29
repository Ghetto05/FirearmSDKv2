using System.Linq;
using UnityEngine;
using ThunderRoad;
using UnityEditor;

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

        public enum LegacyHandlePriority
        {
            NoAI = 0,
            Foregrip = 1,
            AttachForegrip = 2
        }

        public enum HandlePriority
        {
            NoAI = 0,
            Foregrip = 10,
            AttachForegrip = 20
        }

        public enum Tags
        {
            None = 0,
            SlidingStockHandle = 1 << 0
        }

        public HandleType type;
        public HandlePriority aiPriority;
        public Tags tags;

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

        #if UNITY_EDITOR
        [MenuItem("Tools/Update handle priority in Prefabs")]
        public static void UpdateEnumsInPrefabs()
        {
            int[] oldValues = { 1, 2 };
            var prefabPaths = AssetDatabase.FindAssets("t:Prefab");
            foreach (var prefabPath in prefabPaths)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(prefabPath);
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

                var changesMade = false;
                foreach (var handle in prefab.GetComponentsInChildren<GhettoHandle>())
                {
                    if (!oldValues.Contains((int)handle.aiPriority))
                        continue;

                    handle.aiPriority = (int)handle.aiPriority switch
                    {
                        1 => HandlePriority.Foregrip,
                        2 => HandlePriority.AttachForegrip,
                        _ => HandlePriority.NoAI
                    };
                    changesMade = true;
                }

                if (changesMade)
                {
                    EditorUtility.SetDirty(prefab);
                    Debug.Log($"Updated enums in prefab: {prefab.name}");
                }
            }

            AssetDatabase.SaveAssets();
        }
        #endif
    }
}