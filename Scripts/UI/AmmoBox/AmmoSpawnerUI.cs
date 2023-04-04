using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThunderRoad;
using UnityEngine.UI;

namespace GhettosFirearmSDKv2
{
    public class AmmoSpawnerUI : MonoBehaviour
    {
        public bool alwaysFrozen;
        [Space]
        public Transform spawnPosition;
        public Item item;
        public Canvas canvas;
        public Collider canvasCollider;
        public Transform categoriesContent;
        public Transform calibersContent;
        public Transform variantContent;
        [Space]
        public string currentCategory;
        public string currentCaliber;
        public string currentVariant;
        [Space]
        public GameObject categoryPref;
        public GameObject caliberPref;
        public GameObject variantPref;
        [Space]
        public Text description;

        public void SetupCalibersList()
        {
            
        }

        public void GetCaliberFromGunOrMag()
        {
            
        }

        public void ClearMagazine()
        {
        
        }

        public void FillMagazine()
        {
            
        }

        public void TopOffMagazine()
        {
            
        }

        public void SpawnRound()
        {
            
        }

        public void Lock()
        {
        }
    }
}
