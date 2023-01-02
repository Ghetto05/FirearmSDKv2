using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThunderRoad;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Ammunition/Sticky bomb")]
    [RequireComponent(typeof(Item))]
    public class StickyBomb : MonoBehaviour
    {
        public List<Collider> colliders;
    }
}
