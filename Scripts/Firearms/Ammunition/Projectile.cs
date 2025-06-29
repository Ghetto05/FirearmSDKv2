using System.Collections;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    public class Projectile : MonoBehaviour
    {
        public Item item;
        public ProjectileData data;
        // ReSharper disable once IdentifierTypo
        public List<Collider> affectors;
        public bool stick;
        public bool despawnOnContact;
        public float automaticDespawnDelay = -1;
    }
}
