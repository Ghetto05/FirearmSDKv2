using ThunderRoad;
using System.Collections.Generic;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    public class PressureSwitch : TacticalSwitch
    {
        public bool toggleMode;

        public Attachment attachment;
        public List<Handle> handles;
        public GameObject attachmentManager;

        public List<AudioSource> pressSounds;
        public List<AudioSource> releaseSounds;
    }
}