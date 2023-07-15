using UnityEngine;
using ThunderRoad;
using System.Collections.Generic;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Attachments/Systems/Sights/Simple scope")]
    public class Scope : MonoBehaviour
    {
        [Header("Lens")]
        public MeshRenderer lens;
        public int materialIndex = 0;
        public Camera cam;
        [Header("Zoom")]
        public float noZoomMagnification;
        public bool hasZoom = true;
        public Handle controllingHandle;
        public Firearm connectedFirearm;
        public Attachment connectedAtatchment;
        public List<float> MagnificationLevels;
        public Transform Selector; 
        public List<Transform> SelectorPositions;
        public List<GameObject> Reticles;
        public AudioSource CycleUpSound;
        public AudioSource CycleDownSound;

        private void Awake()
        {
            RenderTexture rt = new RenderTexture(512, 512, 1, UnityEngine.Experimental.Rendering.DefaultFormat.HDR);
            rt.graphicsFormat = UnityEngine.Experimental.Rendering.GraphicsFormat.R16G16B16A16_UNorm;
            cam.targetTexture = rt;
            lens.materials[materialIndex].SetTexture("_BaseMap", rt);
        }
    }
}
