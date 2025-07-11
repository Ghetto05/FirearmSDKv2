using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThunderRoad;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Bolt assemblies/Handle folder")]
    public class HandleFolder : MonoBehaviour
    {
        public Transform axis;
        public List<Handle> handles;
        public List<string> handleNames;
        public Transform defaultPosition;
        public List<Transform> positions;
        public bool parentToPosition;
        public bool foldIfBoltCaught;
        public BoltSemiautomatic bolt;

        private void Update()
        {
            bool held = false;
            foreach (Handle h in handles)
            {
            }
            if (!held)
            {
                axis.localPosition = defaultPosition.localPosition;
                axis.localEulerAngles = defaultPosition.localEulerAngles;
            }
        }
    }
}
