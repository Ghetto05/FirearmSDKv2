using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThunderRoad;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    public class AttachmentValidator : MonoBehaviour
    {
        public Item item;
        
        public Light errorLight;
        public Light successLight;
        
        public AttachmentPoint slot;

        public AudioSource[] clickSounds;
    }
}