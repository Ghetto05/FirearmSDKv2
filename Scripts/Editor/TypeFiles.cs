using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GhettosFirearmSDKv2
{
    public class TypeFiles : ScriptableObject
    {
#if UNITY_EDITOR

        public static TextAsset AttachmentTypesFile()
        {
            return (TextAsset)AssetDatabase.LoadAssetAtPath("Assets/FirearmSDKv2/Types/CommonAttachmentTypes.json", typeof(TextAsset));
        }

        public static TextAsset CalibersFile()
        {
            return (TextAsset)AssetDatabase.LoadAssetAtPath("Assets/FirearmSDKv2/Types/Calibers.json", typeof(TextAsset));
        }

        public static TextAsset ExplosiveEffectsFile()
        {
            return (TextAsset)AssetDatabase.LoadAssetAtPath("Assets/FirearmSDKv2/Types/ExplosiveEffects.json", typeof(TextAsset));
        }
#endif
    }
}
