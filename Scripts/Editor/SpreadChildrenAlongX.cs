using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    public class SpreadChildrenAlongX : MonoBehaviour
    {
#if UNITY_EDITOR
        private List<Transform> transformsList;

        [EasyButtons.Button]
        void GetTransforms()
        {
            transformsList = new List<Transform>();
            List<Transform> trs = new();
            foreach (Transform t in transform)
            {
                trs.Add(t);
                t.localPosition = Vector3.zero;
            }
            
            List<string> calibers = JsonUtility.FromJson<Wrapper>(TypeFiles.CartridgesFile().text).array.ToList();
            transformsList = trs.OrderBy(tr => calibers.IndexOf(tr.name)).ToList();
        }

        [EasyButtons.Button]
        void SpreadOutTransforms()
        {
            if (transformsList == null || transformsList.Count < 2)
            {
                return;
            }

            float lastPos = 0f;
            for (int i = 1; i < transformsList.Count; i++)
            {
                Transform currentTransform = transformsList[i];
                Bounds lastBounds = GetBoundsXAxis(transformsList[i-1]);
                Bounds theseBounds = GetBoundsXAxis(currentTransform);

                currentTransform.localPosition = new Vector3(lastPos + 0.01f + lastBounds.extents.x + theseBounds.extents.x, 0, 0);
                lastPos = currentTransform.localPosition.x;
            }
        }

        // Bounds GetBoundsXAxis(Transform t)
        // {
        //     Renderer renderer = t.GetComponent<Renderer>();
        //     Bounds bounds = new Bounds(); // Initialize bounds
        //
        //     if (renderer != null)
        //     {
        //         bounds = renderer.bounds;
        //     }
        //
        //     // Check children for renderers
        //     foreach (Transform child in t)
        //     {
        //         Bounds childBounds = GetBoundsXAxis(child);
        //         if (childBounds.size.x > 0) // Ignore empty transforms
        //         {
        //             if (bounds.size == Vector3.zero)
        //             {
        //                 bounds = childBounds;
        //             }
        //             else
        //             {
        //                 bounds.Encapsulate(childBounds);
        //             }
        //         }
        //     }
        //
        //     return bounds;
        // }
        
        Bounds GetBoundsXAxis(Transform t)
        {
            return t.GetComponentInChildren<Renderer>().bounds;
        }
#endif
    }
}