using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    public class CartridgeHolder : MonoBehaviour
    {
        public Firearm firearm;
        public Attachment attachment;

        public int slot;
        public string caliber;
        public Collider mountCollider;

        public List<AudioSource> roundInsertSounds;
        public List<AudioSource> roundEjectSounds;

        public Cartridge EjectRound()
        {
            return null;
        }

        public void InsertRound(Cartridge c, bool silent, bool forced)
        {
        }
    }
}
