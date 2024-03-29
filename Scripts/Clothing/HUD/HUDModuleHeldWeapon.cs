using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Clothing/HUD/HUD module - held weapon ammo counter")]
    public class HUDModuleHeldWeapon : MonoBehaviour
    {
        public HUD hud;
        public Image icon;
        public TextMeshProUGUI roundCounter;
        public TextMeshProUGUI capacityDisplay;
        public Color defaultColor = Color.white;
        public Color lowColor = Color.red;
        public List<GameObject> additionalDisableObjects;
    }
}
