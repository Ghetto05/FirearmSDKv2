using UnityEngine;
using ThunderRoad;
using System.Collections.Generic;
using System.Collections;
using EasyButtons;
using GhettosFirearmSDKv2.Attachments;
using UnityEngine.Serialization;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Attachments/Systems/Sights/Scope")]
    public class Scope : MonoBehaviour
    {
        public GameObject attachmentManager;
        [FormerlySerializedAs("connectedAtatchment")]
        public Attachment connectedAttachment;

        public enum LensSizes
        {
            _10mm = 10,
            _15mm = 15,
            _20mm = 20,
            _25mm = 25,
            _30mm = 30,
            _35mm = 35,
            _40mm = 40,
            _45mm = 45,
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
        [FormerlySerializedAs("MagnificationLevels")] public List<float> magnificationLevels;
        [FormerlySerializedAs("Selector")] public Transform selector;
        [FormerlySerializedAs("SelectorPositions")] public List<Transform> selectorPositions;
        [FormerlySerializedAs("Reticles")] public List<GameObject> reticles;
        [FormerlySerializedAs("CycleUpSound")] public AudioSource cycleUpSound;
        [FormerlySerializedAs("CycleDownSound")] public AudioSource cycleDownSound;
        public int currentIndex;

        [EasyButtons.Button]
        public void UpdateZoom()
        {
            SetFOVFromMagnification(hasZoom ? magnificationLevels[currentIndex] : noZoomMagnification);
            UpdatePosition();
        }

        private void Start()
        {
            RenderTexture rt = new RenderTexture(1024, 1024, 1, UnityEngine.Experimental.Rendering.DefaultFormat.HDR);
            rt.graphicsFormat = UnityEngine.Experimental.Rendering.GraphicsFormat.R16G16B16A16_UNorm;
            cam.targetTexture = rt;
            if (lens != null) lenses.Add(lens);
            StartCoroutine(DelayedLoad());
        }

        IEnumerator DelayedLoad()
        {
            yield return new WaitForSeconds(1.05f);
            SetFOVFromMagnification(noZoomMagnification);

            if (hasZoom)
            {
                currentIndex = 0;
                SetZoom();
                UpdatePosition();
            }
        }

        [Button]
        public void CycleUp() => Cycle(true);
        [Button]
        public void CycleDown() => Cycle(false);

        public void Cycle(bool up)
        {
            if (up)
            {
                if (cycleUpSound != null)
                    cycleUpSound.Play();
                if (currentIndex == magnificationLevels.Count - 1) currentIndex = -1;
                currentIndex++;
            }
            else
            {
                if (cycleDownSound != null)
                    cycleDownSound.Play();
                if (currentIndex == 0) currentIndex = magnificationLevels.Count;
                currentIndex--;
            }
            SetZoom();
            UpdatePosition();
        }

        public void SetZoom()
        {
            SetFOVFromMagnification(magnificationLevels[currentIndex]);
        }

        public float GetScale()
        {
            return (float)size / 100f;
        }

        public void UpdatePosition()
        {
            if (reticles.Count > currentIndex && reticles[currentIndex] != null)
            {
                foreach (GameObject reticle in reticles)
                {
                    reticle.SetActive(false);
                }
                reticles[currentIndex].SetActive(true);
            }
            if (selector != null && selectorPositions[currentIndex] is Transform t)
            {
                selector.localPosition = t.localPosition;
                selector.localEulerAngles = t.localEulerAngles;
            }
        }

        [EasyButtons.Button]
        public void SetFOVFromMagnification(float magnification)
        {
            float factor = 2.0f * Mathf.Tan(0.5f * /*var*/20f * Mathf.Deg2Rad);
            float fov = 2.0f * Mathf.Atan(factor / (2.0f * magnification)) * Mathf.Rad2Deg;

            cam.fieldOfView = fov;
            foreach (Camera c in additionalCameras)
            {
                c.fieldOfView = fov;
            }
            UpdateRenderers();
        }

        public void UpdateRenderers()
        {
            foreach (MeshRenderer l in lenses)
            {
                l.materials[materialIndex].SetTexture("_BaseMap", cam.targetTexture);
                l.materials[materialIndex].SetTextureScale("_BaseMap", Vector2.one * GetScale());
                l.materials[materialIndex].SetTextureOffset("_BaseMap", Vector3.one * ((1 - GetScale()) / 2));
            }
        }
    }
}