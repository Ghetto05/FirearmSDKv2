using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ThunderRoad;

namespace GhettosFirearmSDKv2
{
    public class TypeFiles : ScriptableObject
    {
#if UNITY_EDITOR

        public static TextAsset AttachmentTypesFile()
        {
            return Catalog.EditorLoad<TextAsset>("FirearmSDK.AttachmentTypes");
        }

        public static TextAsset CartridgesFile()
        {
            return Catalog.EditorLoad<TextAsset>("FirearmSDK.Cartridges");
        }

        public static TextAsset ExplosiveEffectsFile()
        {
            return Catalog.EditorLoad<TextAsset>("FirearmSDK.ExplosiveEffects");
        }

        public static TextAsset CountriesFile()
        {
            return Catalog.EditorLoad<TextAsset>("FirearmSDK.Countries");
        }

        public static TextAsset RailTypeFile()
        {
            return Catalog.EditorLoad<TextAsset>("FirearmSDK.RailTypes");
        }
#endif
    }
}
