using System.Collections;
using System.Collections.Generic;
using EasyButtons;
using UnityEditor;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    public class PositionNamer : MonoBehaviour
    {
#if UNITY_EDITOR
        public string prefix;
        public int postfixSpaceCount;

        [Button]
        public void Fix()
        {
            foreach (var o in transform)
            {
                var t = (Transform)o;
                var index = t.GetSiblingIndex();
                t.name = $"{prefix}_{new string('0', postfixSpaceCount - index.ToString().Length)}{index}";
            }
            
            EditorUtility.SetDirty(gameObject);
        }
#endif
    }
}
