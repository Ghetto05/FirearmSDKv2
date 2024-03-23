using TMPro;
using UnityEngine;

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
        public TextMeshProUGUI timeSlow;

        private void Update()
        {
            health.localScale = Scale(he, 50f);
            mana.localScale = Scale(ma, 100f);
            focus.localScale = Scale(fo, 100f);
            timeSlow.text = ts * 100 + "%";
        }

        private static Vector3 Scale(float current, float max)
        {
            return new Vector3(1, current / max, 1);
        }
    }
}
