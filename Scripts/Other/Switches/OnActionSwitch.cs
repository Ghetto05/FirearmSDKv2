using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThunderRoad;
using UnityEngine.Events;
using System;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Switches/Handle action based")]
    public class OnActionSwitch : MonoBehaviour
    {
        public enum Actions
        {
            TriggerPull,
            TriggerRelease,
            AlternateButtonPress,
            AlternateButtonRelease
        }

        public Handle handle;
        public Actions switchAction;
        public AudioSource switchSound;
        private int current = 0;
        [Header("Event 0 is the default, on spawn executed")]
        public List<UnityEvent> events;
        public GameObject attachmentManager;
        public Attachment parentAttachment;
        public List<SwitchRelation> switches;

        [EasyButtons.Button]
        public void Switch()
        {
            if (switchSound != null) switchSound.Play();
            if (current + 1 < events.Count)
            {
                current++;
            }
            else
            {
                current = 0;
            }
            if (switchSound != null) switchSound.Play();
            events[current]?.Invoke();
            foreach (SwitchRelation swi in switches)
            {
                if (swi != null) AlignSwitch(swi, current);
            }
        }

        public void AlignSwitch(SwitchRelation swi, int index)
        {
            if (!swi.usePositionsAsDifferentObjects && swi.switchObject != null && swi.modePositions.Count > index && swi.modePositions[index] != null)
            {
                swi.switchObject.localPosition = swi.modePositions[index].localPosition;
                swi.switchObject.localEulerAngles = swi.modePositions[index].localEulerAngles;
            }
            else
            {
                foreach (Transform t in swi.modePositions)
                {
                    t.gameObject.SetActive(false);
                }
                swi.modePositions[current].gameObject.SetActive(true);
            }
        }
    }
}