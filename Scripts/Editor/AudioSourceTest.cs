using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThunderRoad;

namespace GhettosFirearmSDKv2
{
    public class AudioSourceTest : MonoBehaviour
    {
        public AudioSource[] sources;

        [EasyButtons.Button]
        public void Play()
        {
            foreach (AudioSource source in sources)
            {
                source.Play();
            }
        }
    }
}
