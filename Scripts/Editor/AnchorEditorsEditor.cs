using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ThunderRoad;
using System;

#if UNITY_EDITOR
namespace GhettosFirearmSDKv2
{
    [CustomEditor(typeof(AnchorEditors))]
    public class AnchorEditorsEditor : Editor
    {
        bool debug = false;
        public override void OnInspectorGUI()
        {
            AnchorEditors edit = (AnchorEditors)target;
            EditorGUILayout.HelpBox("Parent all firearms to this gameobject, then use the buttons below.", MessageType.Info);

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            if (GUILayout.Button("Find firearms"))
            {
                edit.GetAllFirearms();
            }

            #region Vice
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            if (GUILayout.Button("Move firearms to Vice"))
            {
                edit.GoTo(AnchorEditors.Anchors.Vice);
            }
            EditorGUILayout.Space();
            EditorGUILayout.Space();//
            EditorGUILayout.HelpBox("Move to Vice anchor first!", MessageType.Warning);
            if (GUILayout.Button("Apply Vice anchor"))
            {
                edit.ApplyAnchor(AnchorEditors.Anchors.Vice);
            }
            #endregion

            #region Rack
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            if (GUILayout.Button("Move firearms to Rack"))
            {
                edit.GoTo(AnchorEditors.Anchors.Rack);
            }
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("Move to Rack anchor first!", MessageType.Warning);
            if (GUILayout.Button("Apply Rack anchors"))
            {
                edit.ApplyAnchor(AnchorEditors.Anchors.Rack);
            }
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            #endregion

            #region Case
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            if (GUILayout.Button("Move firearms to Case"))
            {
                edit.GoTo(AnchorEditors.Anchors.Case);
            }
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("Move to Case anchor first!", MessageType.Warning);
            if (GUILayout.Button("Apply Case anchors"))
            {
                edit.ApplyAnchor(AnchorEditors.Anchors.Case);
            }
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            #endregion


            debug = EditorGUILayout.Toggle("Debug mode", debug);
            if (debug)
            {
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                edit.ViceAnchor = (Transform)EditorGUILayout.ObjectField("Vice anchor", edit.ViceAnchor, typeof(Transform), true);
                edit.RackAnchor = (Transform)EditorGUILayout.ObjectField("Rack anchor", edit.RackAnchor, typeof(Transform), true);
                edit.CaseAnchor = (Transform)EditorGUILayout.ObjectField("Case anchor", edit.CaseAnchor, typeof(Transform), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("firearms"));
            }
        }
    }
}
#endif
