using System.ComponentModel;
using ThunderRoad;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    public class ItemMetaData
    {
        public enum ItemType
        {
            Firearm,
            PrebuiltFirearm,
            Cartridge,
            Magazine,
            Melee,
            Clothing,
            Tool,
            Grenade
        }
        public enum Actions
        {
            ClosedBolt,
            OpenBolt,
            PumpAction,
            BoltAction,
            BreakAction,
            ChamberLoader,
            Revolver,
            Minigun
        }
        public enum Eras
        {
            [Description("Victorian Era (1800 - 1900)")]
            Victorian, //1800 - 1900
            [Description("Wild West Era (1900 - 1914)")]
            WildWest, //1900 - 1914
            [Description("The Great War (1914 - 1918)")]
            TheGreatWar, //1914 - 1918
            [Description("Interwar Years (1918 - 1938)")]
            Interwar, //1918 - 1938
            [Description("World War II (1938 - 1945)")]
            WorldWarII, //1938 - 1945
            [Description("Early Cold War (1945 - 1970)")]
            EarlyColdWar, //1945 - 1970
            [Description("Late Cold War (1970 - 1991)")]
            LateColdWar, //1970 - 1991
            [Description("War On Terror (1991 - 2024)")]
            WarOnTerror //1991 - 2024
        }

        public ItemType itemType;
        public string category;
        public string description;
        public Actions action;
        public FirearmBase.FireModes[] fireModes;
        public int[] fireRates;
        public string[] calibers;
        public string[] magazineTypes;
        public int capacity;
        [TypePicker(TypePicker.Types.Countries)]
        public string countryOfOrigin;
        public Eras era;
        public int yearOfIntroduction;
    }
}