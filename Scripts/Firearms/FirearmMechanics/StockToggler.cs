using GhettosFirearmSDKv2.Attachments;
using UnityEngine;
using ThunderRoad;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Firearm components/Stock position switcher")]
    public class StockToggler : MonoBehaviour
    {
        public GameObject attachmentManager;
        public Attachment connectedAttachment;

        public AudioSource toggleSound;
        public Handle toggleHandle;
        public Transform pivot;
        public Transform[] positions;
        public bool useAsSeparateObjects;
    }
}