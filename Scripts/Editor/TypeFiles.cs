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
            TextAsset ta = Catalog.EditorLoad<TextAsset>("FirearmSDK.AttachmentTypes");
            return ta;
        }

        public static TextAsset CalibersFile()
        {
            TextAsset ta = Catalog.EditorLoad<TextAsset>("FirearmSDK.Calibers");
            return ta;
        }

        public static TextAsset ExplosiveEffectsFile()
        {
            TextAsset ta = Catalog.EditorLoad<TextAsset>("FirearmSDK.ExplosiveEffects");
            return ta;
        }

        public static TextAsset CountriesFile()
        {
            TextAsset ta = Catalog.EditorLoad<TextAsset>("FirearmSDK.Countries");
            return ta;
        }
#endif
    }
}
