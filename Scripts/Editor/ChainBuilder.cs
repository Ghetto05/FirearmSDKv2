using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ThunderRoad;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    public class ChainBuilder : MonoBehaviour
    {
        public Transform lastNextPosition;
        public GameObject linkPrefab;
        public string nextPositionName;
        public int currentCount;
        public int targetCount;
        public string childPositionName;

        public Transform finalParent;
        public List<GameObject> objects;

        [EasyButtons.Button]
        public void Build()
        {
            while (currentCount < targetCount)
            {
                currentCount++;
                GameObject newObj = Instantiate(linkPrefab, lastNextPosition);
                newObj.name = linkPrefab.name + currentCount;
                newObj.SetActive(true);
                objects.Add(newObj);
                lastNextPosition = newObj.transform.Find(nextPositionName);
            }
        }
        
        [EasyButtons.Button]
        public void GoToFinalParent()
        {
            foreach (GameObject o in objects)
            {
                if (!childPositionName.IsNullOrEmptyOrWhitespace())
                {
                    Transform newParent = o.transform.Find(childPositionName);
                    foreach (Transform tt in newParent)
                    {
                        DestroyImmediate(tt.gameObject);
                    }
                    newParent.gameObject.name = childPositionName + int.Parse(string.Concat(o.name.Where(char.IsDigit)));
                    newParent.SetParent(finalParent);
                    o.transform.SetParent(newParent);
                }
                else
                    o.transform.SetParent(finalParent);
            }
        }

        [EasyButtons.Button]
        public void Clean()
        {
            foreach (GameObject obj in objects)
            {
                DestroyImmediate(obj.transform.Find(nextPositionName).gameObject);
            }
        }
    }
}
