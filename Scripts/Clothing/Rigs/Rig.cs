using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThunderRoad;

namespace GhettosFirearmSDKv2
{
    public class Rig : MonoBehaviour
    {
        public List<RigSlot> slots;

        [EasyButtons.Button]
        public void GetAllSlots()
        {
            slots = new List<RigSlot>();
            foreach (RigSlot s in this.gameObject.GetComponentsInChildren<RigSlot>())
            {
                slots.Add(s);
            }
        }

        private void Reset()
        {
            GameObject prev = new GameObject("Preview");
            prev.transform.SetParent(this.transform);
            prev.transform.localPosition = Vector3.zero;
            prev.transform.localEulerAngles = new Vector3(8.88f, 154.882f, -0f);
            prev.AddComponent<Preview>();
        }
    }
}
