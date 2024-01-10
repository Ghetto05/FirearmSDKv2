using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GhettosFirearmSDKv2.UI
{
    public class CanvasCamAssigner : MonoBehaviour
    {
        public Canvas canvas;

        public void Awake()
        {
            StartCoroutine(Assign());
        }

        private IEnumerator Assign()
        {
            yield return new WaitForSeconds(1f);
            canvas.worldCamera = Camera.main;
        }
    }
}
