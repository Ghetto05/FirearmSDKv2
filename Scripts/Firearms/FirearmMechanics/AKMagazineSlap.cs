using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Firearm components/Physical magazine release")]
    public class AkMagazineSlap : MonoBehaviour
    {
        public Firearm firearm;
        public List<Collider> triggers;
    }
}
