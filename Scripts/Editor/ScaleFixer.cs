using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EasyButtons;
using ThunderRoad;
using UnityEditor;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    public class ScaleFixer : MonoBehaviour
    {
        #if UNITY_EDITOR
        public Transform transformToFix;
        private Dictionary<Transform, Transform> _transformParentPairs;

        [Button]
        public void Fix()
        {
            if (!transformToFix)
                return;
            _transformParentPairs = new Dictionary<Transform, Transform>();
            GatherTransforms(transformToFix);
            FixScales();
        }

        private void GatherTransforms(Transform t)
        {
            foreach (Transform ct in t)
            {
                _transformParentPairs.Add(ct, t);
                GatherTransforms(ct);
            }
        }

        private void FixScales()
        {
            foreach (var key in _transformParentPairs.Keys.Where(key => !PrefabUtility.IsPartOfAnyPrefab(key.gameObject)))
            {
                key.SetParent(null);
            }
            
            transformToFix.localScale = Vector3.one;
            foreach (var key in _transformParentPairs.Keys.Where(key => !key.gameObject.GetComponent<Renderer>() &&
                                                                        !key.gameObject.GetComponent<Collider>() &&
                                                                        !key.gameObject.GetComponent<ParticleSystem>() &&
                                                                        !key.gameObject.GetComponent<Preview>() &&
                                                                        (!PrefabUtility.IsPartOfAnyPrefab(key.gameObject) ||
                                                                         PrefabUtility.IsAnyPrefabInstanceRoot(key.gameObject))))
            {
                key.localScale = Vector3.one;
            }
            
            foreach (var pair in _transformParentPairs.Where(pair => !PrefabUtility.IsPartOfAnyPrefab(pair.Key.gameObject)))
            {
                pair.Key.SetParent(pair.Value);
            }
        }
        #endif
    }
}
