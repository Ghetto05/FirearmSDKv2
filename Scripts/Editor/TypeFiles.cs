using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    public class TypeFiles : ScriptableObject
    {
        public TextAsset attachmentTypesFileAsset;
        public TextAsset caliberTypesFileAsset;

        public static TextAsset attachmentTypesFile()
        {
            TypeFiles tf = (TypeFiles) CreateInstance(typeof(TypeFiles));
            return tf.attachmentTypesFileAsset;
        }

        public static TextAsset caliberTypesFile()
        {
            TypeFiles tf = (TypeFiles) CreateInstance(typeof(TypeFiles));
            return tf.caliberTypesFileAsset;
        }
    }
}
