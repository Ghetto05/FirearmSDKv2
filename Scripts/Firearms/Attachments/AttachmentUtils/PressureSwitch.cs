using ThunderRoad;
using System.Collections.Generic;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    public class PressureSwitch : MonoBehaviour
    {
        public bool toggleMode;

        public Attachment attachment;
        public List<Handle> handles;
        public Item item;

        public List<AudioSource> pressSounds;
        public List<AudioSource> releaseSounds;
    }
}
