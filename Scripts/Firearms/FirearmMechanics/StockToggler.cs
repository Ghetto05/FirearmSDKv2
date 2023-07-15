using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThunderRoad;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Firearm components/Stock position switcher")]
    public class StockToggler : MonoBehaviour
    {
        public AudioSource toggleSound;
        public Handle toggleHandle;
        public Transform pivot;
        public Transform[] positions;
        public Firearm connectedFirearm;
        public Attachment connectedAttachment;
    }
}
