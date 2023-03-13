using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GhettosFirearmSDKv2
{
    public class HUDModuleStats : MonoBehaviour
    {
        public float he;
        public Transform health;
        public float ma;
        public Transform mana;
        public float fo;
        public Transform focus;
        public float ts;
        public Text timeSlow;

        void Update()
        {
            health.localScale = Scale(he, 50f);
            mana.localScale = Scale(ma, 100f);
            focus.localScale = Scale(fo, 100f);
            timeSlow.text = (ts * 100).ToString() + "%";
        }

        private Vector3 Scale(float current, float max)
        {
            return new Vector3(1, current / max, 1);
        }
    }
}
