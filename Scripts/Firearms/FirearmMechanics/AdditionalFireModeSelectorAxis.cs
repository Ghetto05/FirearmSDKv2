using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    public class AdditionalFireModeSelectorAxis : MonoBehaviour
    {
        public FiremodeSelector selector;

        public Transform axis;
        public List<Transform> positions;

        public Transform safePosition;
        public Transform semiPosition;
        public Transform burstPosition;
        public Transform autoPosition;
        public Transform attachmentFirearmPosition;
    }
}
