using UnityEngine;

namespace GhettosFirearmSDKv2
{
    public class TacticalDevice : MonoBehaviour
    {
        public string channelName;
        public int channel = 1;
        public Attachment attachment;
        public GameObject attachmentManager;
        public bool physicalSwitch;
    }
}