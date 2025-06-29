using System.Collections.Generic;
using UnityEngine;
using ThunderRoad;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Attachments/Systems/Bipod")]
    public class Bipod : MonoBehaviour
    {
        public GameObject attachmentManager;
        public Attachment attachment;
        public Transform axis;
        public List<Transform> positions;
        public Handle toggleHandle;
        public AudioSource toggleSound;
        public Bipod[] requiredBipods;
        public Bipod[] requiredInactiveBipods;
        [HideInInspector]
        public int index;

        public void ToggleUp()
        {
            if (index + 1 == positions.Count) index = 0;
            else index++;
            ApplyPosition();
        }

        public void ToggleDown()
        {
            if (index - 1 == -1) index = positions.Count - 1;
            else index--;
            ApplyPosition();
        }

        public void ApplyPosition()
        {
            toggleSound.Play();
            axis.localPosition = positions[index].localPosition;
            axis.localEulerAngles = positions[index].localEulerAngles;
        }
    }
}