using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    public class MagazineSlotsFixer : MonoBehaviour
    {
        public Transform parent;

        [EasyButtons.Button]
        public void Fix()
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                Transform t = parent.GetChild(i);
                t.localEulerAngles = new Vector3(t.localEulerAngles.x + 90f, t.localEulerAngles.y, t.localEulerAngles.z);
                t.localScale = Vector3.one;
            }
        }
    }
}
