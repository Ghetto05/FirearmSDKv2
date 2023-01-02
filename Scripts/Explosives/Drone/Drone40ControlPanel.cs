using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ThunderRoad;

namespace GhettosFirearmSDKv2.Drone
{
    public class Drone40ControlPanel : MonoBehaviour
    {
        public Drone40 currentDrone;
        public Item item;
        [Header("Selection")]
        public GameObject selectionCanvas;
        public List<GameObject> buttons;
        public RectTransform contentTransform;
        public GameObject buttonPrefab;
        [Header("Control")]
        public GameObject controlCanvas;
        public GameObject grenadeSubControlPanel;
        [Header("Camera control")]
        public GameObject cameraSubControlPanel;
        public RawImage camFeed;
        public RawImage lightBulb;
        public Color lightActiveColor;
        public Color lightDisabledColor;
        public bool lightActive = false;

        private void Awake()
        {
            OnUpdateList += UpdateList;
            GoToSelection();
        }

        public void UpdateList()
        {
            if (contentTransform.gameObject.activeInHierarchy)
            {
                GoToSelection();
            }
        }

        public void GoToSelection()
        {
            currentDrone = null;
            controlCanvas.SetActive(false);
            selectionCanvas.SetActive(true);
            if (Drone40.all == null) return;
            if (buttons != null)
            {
                foreach (GameObject obj in buttons)
                {
                    Destroy(obj);
                }
            }
            buttons = new List<GameObject>();

            foreach (Drone40 drone in Drone40.all)
            {
                GameObject button = Instantiate(buttonPrefab, new Vector3(20, -20, 0), Quaternion.Euler(0, 0, 0), contentTransform);
                button.SetActive(true);
                buttons.Add(button);
                button.transform.localPosition = new Vector3(20, -20 - (buttons.IndexOf(button) * 70), 0);
                button.transform.GetChild(0).gameObject.GetComponent<Text>().text = drone.droneId;
                button.GetComponent<Button>().onClick.AddListener(delegate { GoToDroneControl(button); });
            }

            contentTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 20 + (buttons.Count * 70));
        }

        public void GoToDroneControl(GameObject button)
        {
            controlCanvas.SetActive(true);
            selectionCanvas.SetActive(false);
            currentDrone = Drone40.GetByID(button.transform.GetChild(0).gameObject.GetComponent<Text>().text);
            lightActive = false;
            cameraSubControlPanel.SetActive(currentDrone.type == Drone40.DroneType.Camera);
            grenadeSubControlPanel.SetActive(currentDrone.type == Drone40.DroneType.Grenade);
            if (currentDrone.type == Drone40.DroneType.Camera)
            {
                camFeed.texture = new RenderTexture(currentDrone.cam.targetTexture);
                currentDrone.cam.targetTexture = (RenderTexture)camFeed.texture;
            }
        }

        public void MoveCameraUp()
        {
            if (currentDrone != null && currentDrone.active && currentDrone.type == Drone40.DroneType.Camera)
            {
                currentDrone.MoveCamera(Drone40.cameraDirections.Up);
            }
        }

        public void MoveCameraDown()
        {
            if (currentDrone != null && currentDrone.active && currentDrone.type == Drone40.DroneType.Camera)
            {
                currentDrone.MoveCamera(Drone40.cameraDirections.Down);
            }
        }

        public void MoveCameraLeft()
        {
            if (currentDrone != null && currentDrone.active && currentDrone.type == Drone40.DroneType.Camera)
            {
                currentDrone.MoveCamera(Drone40.cameraDirections.Left);
            }
        }

        public void MoveCameraRight()
        {
            if (currentDrone != null && currentDrone.active && currentDrone.type == Drone40.DroneType.Camera)
            {
                currentDrone.MoveCamera(Drone40.cameraDirections.Right);
            }
        }

        public static void CallUpdateList()
        {
            OnUpdateList?.Invoke();
        }

        public void ToggleLight()
        {
            lightActive = !lightActive;
            if (lightActive) lightBulb.color = lightActiveColor;
            else lightBulb.color = lightDisabledColor;
            currentDrone.ToggleLight(lightActive);
        }

        public void Detonate()
        {
            GoToSelection();
        }

        public void ArmAndDrop()
        {
            currentDrone.Drop();
            GoToSelection();
        }

        public delegate void UpdateListDelegate();
        public static event UpdateListDelegate OnUpdateList;
    }
}
