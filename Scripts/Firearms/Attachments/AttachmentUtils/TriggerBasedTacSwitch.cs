using ThunderRoad;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    public class TriggerBasedTacSwitch : ThunderBehaviour
    {
        public enum Mode
        {
            FullPull,
            PartialPull
        }
        
        public Firearm firearm;
        public Mode mode;
    }
}
