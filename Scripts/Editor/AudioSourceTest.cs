using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThunderRoad;
using EasyButtons;

namespace GhettosFirearmSDKv2
{
    public class AudioSourceTest : MonoBehaviour
    {
        public AudioSource[] sources;

        [Button]
        public void Play()
        {
            foreach (AudioSource source in sources)
            {
                source.Play();
            }
        }
    }
}
