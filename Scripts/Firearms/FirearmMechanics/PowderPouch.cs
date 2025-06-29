using System;
using System.Collections;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    public class PowderPouch : MonoBehaviour
    {
        private readonly float _delay = 0.15f;

        public Item item;
        public Cartridge cartridge;
        public bool opened;
        public Transform source;
        public GameObject grain;
        public float maxAngle;

        public Transform lid;
        public Transform lidClosedPosition;
        public Transform lidOpenedPosition;

        public AudioSource[] tapSounds;
        public AudioSource[] grainSpawnSounds;
        public AudioSource grainFlowSound;

        public int content = -1;

        private float _lastEject;

        [EasyButtons.Button]
        public void Open()
        {
            if (opened)
                return;
            opened = true;
            if (lid != null)
                lid.SetPositionAndRotation(lidOpenedPosition.position, lidOpenedPosition.rotation);
        }

        [EasyButtons.Button]
        public void Close()
        {
            if (!opened)
                return;
            opened = false;
            if (lid != null)
                lid.SetPositionAndRotation(lidClosedPosition.position, lidClosedPosition.rotation);
        }

        [EasyButtons.Button]
        public void Tap()
        {
            if (opened)
                Spawn();
        }

        private void FixedUpdate()
        {
            if (opened && Vector3.Angle(source.forward, Vector3.down) <= maxAngle)
            {
                Spawn();
            }
        }

        private void Spawn()
        {
            if (!(Time.time - _lastEject > _delay))
            {
                return;
            }
            var grainIn = Instantiate(grain, source.position, Quaternion.Euler(Util.RandomRotation()));
            _lastEject = Time.time;
            grainIn.SetActive(true);
            Destroy(grainIn, 5f);
            if (content != -1)
            {
                content--;
            }
        }
    }
}