using System.Collections;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    public class PumpAutomaticAdapter : MonoBehaviour
    {
        public enum SwitchType
        {
            BoltRelease,
            FireModeSwitch,
            AutoBoltTrigger,
            AutoBoltAlternate,
            PumpTrigger,
            PumpAlternate
        }

        public Item item;
        public SwitchType switchType;
        public PumpAction pumpAction;
        public BoltSemiautomatic automaticBolt;
        public Transform switchRoot;
        public Transform automaticPosition;
        public Transform pumpPosition;
        public AudioSource[] switchSounds;
        public bool pumpActionEngaged;
    }
}
