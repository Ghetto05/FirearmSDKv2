using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThunderRoad;
using UnityEngine.Serialization;

namespace GhettosFirearmSDKv2
{
    public class ChemLight : MonoBehaviour
    {
        public float burnTime = 300f;
        public float lightUpTime = 10f;
        public float lightDownTime = 30f;

        public Item item;
        public MeshRenderer[] renderers;
        public Color color;
        public float strength;
        public AudioSource[] triggerSounds;
        public float lightIntensity;
        public Light[] lights;

        private bool triggered;
        private float triggerTime;
        private bool appliedConstant;
        private bool burntOut;

        [EasyButtons.Button]
        public void Trigger()
        {
            if (triggered) return;
            triggered = true;
            triggerTime = Time.time;
            //item.disallowDespawn = true;
        }

        public void Update()
        {
            if (burntOut || !triggered)
                return;
            
            float timePassed = Time.time - triggerTime;
            float timeRemaining = triggerTime + burnTime - Time.time;

            if (timePassed > burnTime)
            {
                ApplyLightLevel(0);
                burntOut = true;
                //item.disallowDespawn = false;
            }

            if (timePassed <= lightUpTime)
                ApplyLightLevel(timePassed / lightUpTime);
            else if (timeRemaining <= lightDownTime)
                ApplyLightLevel(timeRemaining / lightDownTime);
            else if (!appliedConstant)
            {
                appliedConstant = true;
                ApplyLightLevel(1f);
            }
        }

        [EasyButtons.Button]
        public void ResetChem()
        {
            triggered = false;
            burntOut = false;
        }

        private void ApplyLightLevel(float level)
        {
            foreach (MeshRenderer r in renderers)
            {
                r.material.SetColor("_EmissionColor", color * strength * level);
            }
        }
    }
}
