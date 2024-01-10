using UnityEngine;
using TMPro;

namespace GhettosFirearmSDKv2
{
    public class AmmoCounter : MonoBehaviour
    {
        public TextMeshProUGUI counter;
        public FirearmBase firearm;
        public Attachment attachment;
        public string counterTextFormat;
        public bool tryToDisplayCapacity;
        public bool countChamberAsCapacity;
        public string nullText;
        [Space]
        public int testCount = -1;
        public int testCapacity = -1;

        [EasyButtons.Button]
        public void TestFormat()
        {
            if (testCount != -1)
            {
                if (!tryToDisplayCapacity)
                    counter.text = string.Format(counterTextFormat.Replace("\\n", "\n"), testCount);
                else
                    counter.text = string.Format(counterTextFormat.Replace("\\n", "\n"), testCount, testCapacity);
            }
            else
                counter.text = nullText;
        }
    }
}
