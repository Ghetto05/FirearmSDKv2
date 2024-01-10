using System;
using System.Collections.Generic;
using UnityEngine;
using ThunderRoad;

namespace GhettosFirearmSDKv2.Clothing
{
    public class WearableItem : MonoBehaviour
    {
        public Item item;
        public HumanBodyBones baseBone;
        public List<Handle> unequipHandles;
    }
}