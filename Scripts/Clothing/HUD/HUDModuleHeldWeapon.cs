using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Clothing/HUD/HUD module - held weapon ammo counter")]
    public class HUDModuleHeldWeapon : MonoBehaviour
    {
        public HUD hud;
        public Image icon;
        public Text roundCounter;
        public Text capacityDisplay;
        public Color defaultColor = Color.white;
        public Color lowColor = Color.red;
        public List<GameObject> additionalDisableObjects;
    }
}
