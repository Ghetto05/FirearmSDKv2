using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    public class Tracer : MonoBehaviour
    {
        public enum Colors
        {
            Red,
            Green,
            Blue,
            Orange,
            White
        }

        public Colors color;
        [HideInInspector]
        public Material redMaterial;
        [HideInInspector]
        public Material greenMaterial;
        [HideInInspector]
        public Material blueMaterial;
        [HideInInspector]
        public Material orangeMaterial;
        [HideInInspector]
        public Material whiteMaterial;
        public List<MeshRenderer> tracerRenderers;
        public Light emissionLight;

        private void OnValidate()
        {
            emissionLight.color = color switch
            {
                Colors.Red => Color.red,
                Colors.Green => Color.green,
                Colors.Blue => Color.blue,
                Colors.Orange => Color.yellow,
                Colors.White => Color.white,
                _ => Color.black
            };
            foreach (var tracerRenderer in tracerRenderers)
            {
                tracerRenderer.material = GetMaterial(color);
            }
        }

        private Material GetMaterial(Colors targetColor)
        {
            return targetColor switch
            {
                Colors.Red => redMaterial,
                Colors.Green => greenMaterial,
                Colors.Blue => blueMaterial,
                Colors.Orange => orangeMaterial,
                Colors.White => whiteMaterial,
                _ => null
            };
        }
    }
}
