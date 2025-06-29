using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Attachments/Systems/Illuminators/NVG Only Renderer - Mesh module")]
    public class NvgOnlyRendererMeshModule : MonoBehaviour
    {
        public static List<NvgOnlyRendererMeshModule> All;

        public NvgOnlyRenderer.Types renderType;
        public List<GameObject> objects;

        public void Start()
        {
            if (All == null) All = new List<NvgOnlyRendererMeshModule>();
            All.Add(this);
            foreach (GameObject obj in objects)
            {
                obj.SetActive(false);
            }
        }

        public void OnDestroy()
        {
            All.Remove(this);
        }
    }
}
