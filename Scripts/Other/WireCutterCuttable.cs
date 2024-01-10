using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ThunderRoad;
using UnityEngine;
using UnityEngine.Events;

namespace GhettosFirearmSDKv2
{
    public class WireCutterCuttable : MonoBehaviour
    {
        public static List<WireCutterCuttable> all = new List<WireCutterCuttable>();

        public UnityEvent onCut;
        public Collider[] cuttableColliders;
        private bool cut = false;
        
        private void Start()
        {
            all.Add(this);
        }

        public static void CutFound(Collider[] possibleColliders)
        {
            List<WireCutterCuttable> found = new List<WireCutterCuttable>();
            foreach (WireCutterCuttable wcc in all)
            {
                if (!found.Contains(wcc) && !wcc.cut && possibleColliders.Any(x => wcc.cuttableColliders.Contains(x)))
                {
                    found.Add(wcc);
                    wcc.cut = true;
                    wcc.onCut.Invoke();
                }
            }
        }
    }
}
