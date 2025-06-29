using ThunderRoad;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    public class ForeGripSling : MonoBehaviour
    {
        public Rigidbody rb;
        public Transform axis;
        public Transform heldPosition;
        public Handle[] handles;
        public Attachment attachment;
        public GameObject attachmentManager;
    }
}