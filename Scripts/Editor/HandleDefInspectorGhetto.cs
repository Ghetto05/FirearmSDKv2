using ThunderRoad;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace GhettosFirearmSDKv2
{
    [CustomEditor(typeof(Handle))]
    public class HandleDefInspectorGhetto : HandleDefInspector
    {
        Handle handle;

        public override void OnInspectorGUI()
        {
            handle = (Handle)target;
            GameObject go = handle.gameObject;

            bool edited = false;
            if (GUILayout.Button("Change to Ghetto Handle (Main)"))
            {
                GhettoHandle newhand = GhettoHandle.ReplaceHandle(handle);
                newhand.type = GhettoHandle.HandleType.MainGrip;
                edited = true;
            }

            if (GUILayout.Button("Change to Ghetto Handle (Foregrip)"))
            {
                GhettoHandle.ReplaceHandle(handle);
                edited = true;
            }

            if (GUILayout.Button("Change to Ghetto Handle (Bolt)"))
            {
                GhettoHandle newhand = GhettoHandle.ReplaceHandle(handle);
                newhand.type = GhettoHandle.HandleType.Bolt;
                edited = true;
            }

            if (edited) EditorUtility.SetDirty(go);

            base.OnInspectorGUI();
        }
    }
}
#endif
