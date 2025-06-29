using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThunderRoad;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Firearm components/Additional stock pivot")]
    public class StockTogglerAdditional : MonoBehaviour
    {
        public StockToggler parent;
        public AudioSource toggleSound;
        public Transform pivot;
        public Transform[] positions;
        public bool useAsSeparateObjects;
    }
}