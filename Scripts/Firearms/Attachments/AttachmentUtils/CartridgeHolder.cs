using System.Collections.Generic;
using GhettosFirearmSDKv2.Attachments;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    public class CartridgeHolder : MonoBehaviour
    {
        public Firearm firearm;
        public Attachment attachment;
        public AttachmentManager attachmentManager;

        public int slot;
        public string caliber;
        public Collider mountCollider;
        public ChamberLoader chamberLoader;

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
