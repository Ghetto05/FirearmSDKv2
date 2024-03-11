using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    public class FirearmClicker : MonoBehaviour
    {
        public Transform[] axis;
        public Transform[] idlePos;
        public Transform[] pressedPos;

        public AudioSource[] clickSound;
        public AudioSource[] releaseSound;
    }
}
