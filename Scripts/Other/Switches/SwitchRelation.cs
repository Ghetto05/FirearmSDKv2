using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Switches/Handle action based - Relation")]
    public class SwitchRelation : MonoBehaviour
    {
        public Transform switchObject;
        public bool usePositionsAsDifferentObjects = false;
        public List<Transform> modePositions;
    }
}
