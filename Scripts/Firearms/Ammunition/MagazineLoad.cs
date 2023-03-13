using EasyButtons;
using System.Collections;
using ThunderRoad;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Ammunition/Default magazine load")]
    public class MagazineLoad : MonoBehaviour
    {
        [TypePicker(TypePicker.Types.Caliber)]
        public string cartridgeReference;

        [Button]
        public void CopyReferenceToList()
        {
            for (int i = 0; i < ids.Length; i++)
            {
                ids[i] = cartridgeReference;
            }
        }

        public int forCapacity;
        public string[] ids;
    }
}