using EasyButtons;
using System.Collections;
using ThunderRoad;
using UnityEditor;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Ammunition/Default magazine load")]
    public class MagazineLoad : MonoBehaviour
    {
        [TypePicker(TypePicker.Types.Cartridges)]
        public string cartridgeReference;

        [Button]
        public void CopyReferenceToList()
        {
            for (int i = 0; i < ids.Length; i++)
            {
                ids[i] = cartridgeReference;
            }

#if UNITY_EDITOR
            EditorUtility.SetDirty(gameObject);
#endif
        }

        public int forCapacity;
        public string[] ids;
    }
}