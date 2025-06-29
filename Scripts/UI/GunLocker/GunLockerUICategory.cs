using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

namespace GhettosFirearmSDKv2
{
    public class GunLockerUICategory : MonoBehaviour
    {
        [FormerlySerializedAs("SelectionOutline")]
        public GameObject selectionOutline;
        public Button button;
        public TextMeshProUGUI textName;
    }
}
