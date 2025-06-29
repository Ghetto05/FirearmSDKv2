using System.Collections.Generic;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    public class FireModeReplacer : MonoBehaviour
    {
        public Attachment attachment;
        public FirearmBase.FireModes oldFireMode;
        public FirearmBase.FireModes newFireMode;
        private Dictionary<FiremodeSelector, List<int>> _replacedIndexes = new();
    }
}
