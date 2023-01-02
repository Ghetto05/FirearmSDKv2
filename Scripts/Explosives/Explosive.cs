using System.Collections;
using UnityEngine;
using ThunderRoad;

namespace GhettosFirearmSDKv2.Explosives
{
    public class Explosive : MonoBehaviour
    {
        public Explosive followUpExplosive;
        public float followUpDelay = 0f;
        public bool detonated = false;
        public Item item;

        public virtual void Detonate(float delay = 0f)
        {
        }

        public virtual void ActualDetonate()
        {
        }
    }
}