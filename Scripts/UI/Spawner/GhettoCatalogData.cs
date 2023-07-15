using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThunderRoad;

namespace GhettosFirearmSDKv2
{
    public class GhettoCatalogData : CustomData
    {
        //Firearm
        public string countryOfOrigin;
        public string era;
        public string introductionYear;
        public string action;
        public List<string> magazineType;
        public List<string> caliber;
        public string weaponClass;

        //Magazine
        public string type;
        public List<string> loadableCaliber;
        public string capacity;

        //Universal
        public List<string> tags;
    }
}
