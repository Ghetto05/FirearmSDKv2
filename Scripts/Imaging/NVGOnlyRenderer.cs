using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Attachments/Systems/Illuminators/NVG Only Renderer")]
    public class NvgOnlyRenderer : MonoBehaviour
    {
        public enum Types
        {
            InfraRed,
            FirstPerson,
            Thermal,
            DirectMode,
            Xm157Marker
        }

        public enum ThermalTypes
        {
            Standard,
            RedHot,
            WhiteHot,
            BlackHot
        }

        public NvgOnlyRendererMeshModule[] directModules;
        public Types renderType;
        public Camera renderCamera;
        public ThermalTypes thermalType;

        private void Start()
        {
            if (!directModules.Any())
            {
                RenderPipelineManager.beginCameraRendering += RegularMode_BeginRender;
                RenderPipelineManager.endCameraRendering += RegularMode_EndRender;
            }
            else
            {
                RenderPipelineManager.beginCameraRendering += DirectMode_BeginRender;
                RenderPipelineManager.endCameraRendering += DirectMode_EndRender;
            }
        }

        #region Direct Mode

        private void DirectMode_EndRender(ScriptableRenderContext arg1, Camera arg2)
        {
            foreach (var module in directModules)
            {
                foreach (GameObject obj in module.objects)
                {
                    obj.SetActive(false);
                }
            }
        }

        private void DirectMode_BeginRender(ScriptableRenderContext arg1, Camera arg2)
        {
            foreach (var module in directModules)
            {
                foreach (GameObject obj in module.objects)
                {
                    obj.SetActive(true);
                }
            }
        }

        #endregion
        
        #region Regular Mode

        private void RegularMode_EndRender(ScriptableRenderContext context, Camera cam)
        {
            if (cam == renderCamera)
            {
                if (NvgOnlyRendererMeshModule.All == null)
                    return;

                foreach (NvgOnlyRendererMeshModule module in NvgOnlyRendererMeshModule.All)
                {
                    if (module.renderType.Equals(renderType))
                    {
                        foreach (GameObject obj in module.objects)
                        {
                            //obj.SetActive(false);
                        }
                    }
                }
            }
        }

        private void UpdateThermal()
        {
            if (ThermalBody.all == null) return;

            foreach (ThermalBody t in ThermalBody.all)
            {
                t.SetColor(thermalType);
            }
        }

        private void RegularMode_BeginRender(ScriptableRenderContext context, Camera cam)
        {
            if (cam == renderCamera)
            {
                if (NvgOnlyRendererMeshModule.All == null) return;

                foreach (NvgOnlyRendererMeshModule module in NvgOnlyRendererMeshModule.All)
                {
                    if (module.renderType.Equals(renderType))
                    {
                        foreach (GameObject obj in module.objects)
                        {
                            obj.SetActive(true);
                            if (renderType == Types.Thermal) UpdateThermal();
                            foreach (Renderer r in obj.GetComponentsInChildren<Renderer>())
                            {
                                //r.enabled = true;
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}