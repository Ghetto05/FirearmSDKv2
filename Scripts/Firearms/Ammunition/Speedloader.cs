using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThunderRoad;

namespace GhettosFirearmSDKv2
{
    public class Speedloader : MonoBehaviour
    {
        public Item item;

        public List<Transform> mountPoints;
        public List<Collider> loadColliders;
        public List<string> calibers;
        public List<AudioSource> insertSounds;
    }
}
