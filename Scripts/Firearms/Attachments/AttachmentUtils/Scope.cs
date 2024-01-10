using UnityEngine;
using ThunderRoad;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Attachments/Systems/Sights/Simple scope")]
    public class Scope : MonoBehaviour
    {
        public enum LensSizes
        {
            _10mm = 10,
            _20mm = 20,
            _25mm = 25,
            _30mm = 30,
            _35mm = 35,
            _40mm = 40,
            _50mm = 50,
            _100mm = 100,
            _200mm = 200
        }
        public float x1Zoom;
        [Header("Lens")]
        public MeshRenderer lens;
        public List<MeshRenderer> lenses;
        public int materialIndex = 0;
        public Camera cam;
        public List<Camera> additionalCameras;
        public LensSizes size;
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
        int currentIndex;
        float baseFov;

        [EasyButtons.Button]
        public void UpdateZoom()
        {
            SetZoomNoZoomer(noZoomMagnification);
            UpdatePosition();
        }

        private void Start()
        {
            baseFov = x1Zoom;
            RenderTexture rt = new RenderTexture(1024, 1024, 1, UnityEngine.Experimental.Rendering.DefaultFormat.HDR);
            rt.graphicsFormat = UnityEngine.Experimental.Rendering.GraphicsFormat.R16G16B16A16_UNorm;
            cam.targetTexture = rt;
            lens.materials[materialIndex].SetTexture("_BaseMap", rt);
            StartCoroutine(delayedLoad());
        }

        private void Settings_LevelModule_OnValueChangedEvent()
        {
            baseFov = x1Zoom;
            if (hasZoom) SetZoom();
            else SetZoomNoZoomer(noZoomMagnification);
        }

        IEnumerator delayedLoad()
        {
            yield return new WaitForSeconds(1.05f);
            if (hasZoom && connectedFirearm != null)
            {
            }
            else if (hasZoom && connectedAtatchment != null)
            {
            }
            else SetZoomNoZoomer(noZoomMagnification);

            if (hasZoom)
            {
                currentIndex = 0;
                SetZoom();
                UpdatePosition();
            }
        }

        public void Cycle(bool up)
        {
            if (up)
            {
                CycleUpSound.Play();
                if (currentIndex == MagnificationLevels.Count - 1) currentIndex = -1;
                currentIndex++;
            }
            else
            {
                CycleDownSound.Play();
                if (currentIndex == 0) currentIndex = MagnificationLevels.Count;
                currentIndex--;
            }
            SetZoom();
            UpdatePosition();
        }

        public void SetZoom()
        {
            SetZoomNoZoomer(MagnificationLevels[currentIndex]);
        }

        public float GetScale()
        {
            return (float)size / 100f;
        }

        public void UpdatePosition()
        {
            if (Reticles.Count > currentIndex && Reticles[currentIndex] != null)
            {
                foreach (GameObject reticle in Reticles)
                {
                    reticle.SetActive(false);
                }
                Reticles[currentIndex].SetActive(true);
            }
            if (Selector != null && SelectorPositions[currentIndex] is Transform t)
            {
                Selector.localPosition = t.localPosition;
                Selector.localEulerAngles = t.localEulerAngles;
            }
        }

        public void SetZoomNoZoomer(float zoom)
        {
            var factor = 2.0f * Mathf.Tan(0.5f * baseFov * Mathf.Deg2Rad);
            var zoomedFOV = 2.0f * Mathf.Atan(factor / (2.0f * zoom)) * Mathf.Rad2Deg;
            cam.fieldOfView = zoomedFOV;
            foreach (Camera c in additionalCameras)
            {
                c.fieldOfView = zoomedFOV;
            }
            lens.material.SetTextureScale("_BaseMap", Vector2.one * GetScale());
            lens.material.SetTextureOffset("_BaseMap", Vector3.one * ((1 - GetScale()) / 2));
        }
    }
}
