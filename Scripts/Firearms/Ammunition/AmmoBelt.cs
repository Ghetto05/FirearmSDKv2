using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    public class AmmoBelt : MonoBehaviour
    {
        public Magazine magazine;
        public int hideCount;
        public GameObject[] beltLinks;
        public string linkItem;
        public float beltLinkEjectForce;

        private bool _inserted;
        private Transform[] _allPositions;
        private Transform[] _cappedPositions;

        private void Start()
        {
            List<Transform> original = magazine.cartridgePositions.ToList();
            _allPositions = original.ToArray();
            original.RemoveRange(0, hideCount);
            _cappedPositions = original.ToArray();
            magazine.cartridgePositions = _cappedPositions;
        }

        [EasyButtons.Button]
        public void Insert()
        {
            if (_inserted)
                return;
            _inserted = true;
            
            magazine.cartridgePositions = _allPositions;
            magazine.UpdateCartridgePositions();
            for (int i = 0; i < hideCount; i++)
            {
                beltLinks[i].SetActive(true);
            }
        }
        
        [EasyButtons.Button]
        public void Remove()
        {
            if (!_inserted)
                return;
            _inserted = false;
            
            magazine.cartridgePositions = _cappedPositions;
            magazine.UpdateCartridgePositions();
            for (int i = 0; i < hideCount; i++)
            {
                beltLinks[i].SetActive(false);
            }
        }
    }
}
