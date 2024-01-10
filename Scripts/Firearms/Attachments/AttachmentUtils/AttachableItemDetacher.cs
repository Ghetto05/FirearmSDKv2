using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThunderRoad;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    public class AttachableItemDetacher : MonoBehaviour
    {
        public Attachment attachment;
        public List<Handle> detachHandles;
        public List<AudioSource> detachSounds;
        public string itemId;
    }
}
