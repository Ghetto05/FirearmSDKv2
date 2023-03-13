using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    public class HUDToggle : MonoBehaviour
    {
        public List<Collider> toggleColliders;
        public List<GameObject> componentsToToggle;
        public bool hudActive = true;
    }
}
