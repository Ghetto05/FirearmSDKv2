using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    public class WTFInator : MonoBehaviour
    {
        public List<MeshRenderer> meshRenderers;
        public List<GameObject> objects;
        
        void Update()
        {
            foreach (GameObject ob in objects)
            {
                Debug.Log(ob.name + " " + (ob != null) + " " + ob.activeInHierarchy);
            }

            foreach (MeshRenderer ob in meshRenderers)
            {
                Debug.Log(ob.name + " " + (ob != null) + " " + ob.enabled);
            }
        }
    }
}
