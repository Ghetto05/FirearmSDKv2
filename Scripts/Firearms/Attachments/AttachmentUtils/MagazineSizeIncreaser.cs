using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Attachments/Systems/Magazines/Magazine increaser")]
    public class MagazineSizeIncreaser : MonoBehaviour
    {
        public Attachment attachment;
        public bool useDeltaInsteadOfFixed;
        public int targetSize;
        public int deltaSize;
        private int previousSize;
        private Magazine magazine;
    }
}
