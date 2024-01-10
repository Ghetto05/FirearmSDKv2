using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    public class ThermalBody : MonoBehaviour
    {
        public static List<ThermalBody> all = new List<ThermalBody>();

        public Transform rig;
        public List<Transform> bones;
        public List<SkinnedMeshRenderer> renderers;

        public Material standardMaterial;
        public Material redHotMaterial;
        public Material whiteHotMaterial;
        public Material blackHotMaterial;

        public void Start()
        {
            if (all == null) all = new List<ThermalBody>();
            all.Add(this);
        }

        public void SetColor(NVGOnlyRenderer.ThermalTypes t)
        {
            Material m = null;
            if (t == NVGOnlyRenderer.ThermalTypes.Standard)
            {
                m = standardMaterial;
            }
            else if (t == NVGOnlyRenderer.ThermalTypes.BlackHot)
            {
                m = blackHotMaterial;
            }
            else if (t == NVGOnlyRenderer.ThermalTypes.RedHot)
            {
                m = redHotMaterial;
            }
            else if (t == NVGOnlyRenderer.ThermalTypes.WhiteHot)
            {
                m = whiteHotMaterial;
            }

            foreach (SkinnedMeshRenderer r in renderers)
            {
                r.material = m;
            }
        }
    }
}
