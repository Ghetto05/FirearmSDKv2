using System.Collections;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Firearm components/Reciprocating barrel")]
    public class ReciprocatingBarrel : MonoBehaviour
    {
        public BoltBase bolt;
        public bool lockBoltBack;
        public Transform pivot;
        public Transform front;
        public Transform rear;
        public float pauseTime;
    }
}
