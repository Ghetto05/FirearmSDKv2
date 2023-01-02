using UnityEngine;
using ThunderRoad;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Attachments/Systems/Illuminators/Tactical Light")]
    public class TacLight : MonoBehaviour
    {
        public GameObject lights;
        public Item item;
        public Attachment attachment;

        public void SetActive()
        { }

        public void SetNotActive()
        { }
    }
}
