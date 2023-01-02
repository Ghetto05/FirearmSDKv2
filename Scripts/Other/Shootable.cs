using UnityEngine;
using UnityEngine.Events;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Misc/Shootable object")]
    public class Shootable : MonoBehaviour
    {
        public ProjectileData.PenetrationLevels requiredPenetrationLevel;
        public bool onlyOnce;
        public UnityEvent onShotEvent;
        private bool shot = false;

        public void Shoot(ProjectileData.PenetrationLevels penetrationLevel)
        {
            if (penetrationLevel >= requiredPenetrationLevel && (!onlyOnce || !shot))
            {
                onShotEvent.Invoke();
            }
        }
    }
}
