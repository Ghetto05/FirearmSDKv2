﻿using UnityEngine;
using UnityEngine.UI;

namespace GhettosFirearmSDKv2
{
    public class Laser : TacticalDevice
    {
        public GameObject sourceObject;
        public Transform source;
        public GameObject endPointObject;
        public Transform cylinderRoot;
        public float range;
        public bool activeByDefault;
        public Text distanceDisplay;

        private void Start()
        {
            if (activeByDefault) physicalSwitch = true;
        }

        private void Update()
        {
            if (!physicalSwitch)
            {
                if (cylinderRoot != null) cylinderRoot.localScale = Vector3.zero;
                if (endPointObject != null && endPointObject.activeInHierarchy) endPointObject.SetActive(false);
                if (distanceDisplay != null) distanceDisplay.text = "";
                return;
            }
            if (Physics.Raycast(source.position, source.forward, out RaycastHit hit, range, LayerMask.GetMask("NPC", "Ragdoll", "Default", "DroppedItem", "MovingItem", "PlayerLocomotionObject", "Avatar", "PlayerHandAndFoot")))
            {
                if (cylinderRoot != null) cylinderRoot.localScale = LengthScale(hit.distance);
                if (endPointObject != null && !endPointObject.activeInHierarchy) endPointObject.SetActive(true);
                if (endPointObject != null) endPointObject.transform.localPosition = LengthPosition(hit.distance);
                if (distanceDisplay != null) distanceDisplay.text = hit.distance.ToString();
            }
            else
            {
                if (cylinderRoot != null) cylinderRoot.localScale = LengthScale(8000f);
                if (endPointObject != null && endPointObject.activeInHierarchy) endPointObject.SetActive(false);
                if (distanceDisplay != null) distanceDisplay.text = "---";
            }
        }

        Vector3 LengthScale(float length)
        {
            return new Vector3(1, 1, length);
        }

        Vector3 LengthPosition(float length)
        {
            return new Vector3(0, 0, length);
        }

        public void SetActive()
        {
            if (sourceObject != null) sourceObject.SetActive(true);
            physicalSwitch = true;
        }

        public void SetNotActive()
        {
            if (sourceObject != null) sourceObject.SetActive(false);
            physicalSwitch = false;
        }
    }
}