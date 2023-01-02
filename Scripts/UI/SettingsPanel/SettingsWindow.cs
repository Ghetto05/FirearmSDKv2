using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GhettosFirearmSDKv2.UI
{
    public class SettingsWindow : MonoBehaviour
    {
        [Header("Damage multiplier")]
        public Text damageMultiplierValue;

        [Header("Store magazines in inventory")]
        public Toggle storeMagsInInventoryButton;

        [Header("Disable magazine collisions")]
        public Toggle disableMagazineCollisionsButton;

        void Awake()
        {
        }

        private void Settings_LevelModule_OnValueChangedEvent()
        {
        }

        public void ChangeDamageMultiplier(float value)
        {
        }

        public void ToggleDisableMagazineCollisions()
        {
        }

        public void ToggleStoreMagazinesInInventory()
        {
        }
    }
}