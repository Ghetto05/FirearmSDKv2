using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Chemicals/Darts")]
    public class DartBehavoirs : MonoBehaviour
    {
        public enum Behaviours
        {
            Heal,
            MissingTextures,
            Gun,
            Poison
        }

        public Cartridge cartridge;
        public Projectile projectile;
        public Behaviours behaviour;
    }
}
