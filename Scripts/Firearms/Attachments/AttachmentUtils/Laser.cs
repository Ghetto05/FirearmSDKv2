using ThunderRoad;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Attachments/Systems/Illuminators/Laser")]
    public class Laser : MonoBehaviour
    {
        public Item item;
        public Attachment attachment;
        [Space]
        public GameObject sourceObject;
        public Transform source;
        public GameObject endPointObject;
        public Transform cylinderRoot;
        public float range;
        public bool activeByDefault;

        public void SetActive()
        {
        }

        public void SetNotActive()
        {
        }
    }
}