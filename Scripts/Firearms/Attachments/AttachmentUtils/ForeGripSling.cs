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
        public Item item;

        private void Start()
        {
            Invoke(nameof(InvokedStart), 1f);
        }

        private void InvokedStart()
        {
            if (attachment != null)
                item = attachment.attachmentPoint.parentFirearm.item;
        }

        private void Grab()
        {
            rb.gameObject.SetActive(false);
            axis.SetParent(heldPosition);
            axis.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }

        private void UnGrab()
        {
            rb.gameObject.SetActive(true);
            axis.SetParent(rb.transform);
            axis.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
    }
}
