using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using ThunderRoad;

namespace GhettosFirearmSDKv2
{
    public class AnchorEditors : MonoBehaviour
    {
        public enum Anchors
        {
            Vice,
            Rack
        }

        public Transform RackAnchor;
        public Transform ViceAnchor;
        public List<Item> firearms;

        public void GoTo(Anchors anchor)
        {
            string anchorName = anchor == Anchors.Vice ? "Ghetto's Firearm SDK V2 vice anchor" : "HolderRackTopAnchor";
            Transform target = anchor == Anchors.Vice ? ViceAnchor : RackAnchor;
            GetAllFirearms();
            foreach (Item firearm in firearms)
            {
                firearm.transform.SetParent(anchor == Anchors.Vice? ViceAnchor : RackAnchor);
                if (firearm.GetHolderPoint(anchorName) != null)
                {
                    firearm.transform.MoveAlign(firearm.GetHolderPoint(anchorName).anchor, target);
                }
                else
                {
                    firearm.transform.localPosition = Vector3.zero;
                    firearm.transform.localEulerAngles = Vector3.zero;
                }
            }
        }

        public void ApplyAnchor(Anchors anchor)
        {
            string anchorName = anchor == Anchors.Vice ? "Ghetto's Firearm SDK V2 vice anchor" : "HolderRackTopAnchor";
            Transform target = anchor == Anchors.Vice ? ViceAnchor : RackAnchor;
            foreach (Item firearm in firearms)
            {
                if (firearm.GetHolderPoint(anchorName) == null)
                {
                    Transform trans = new GameObject("Ghetto's Firearm SDK V2 vice anchor").transform;
                    trans.parent = firearm.transform;
                    trans.localPosition = Vector3.zero;
                    trans.localEulerAngles = Vector3.zero;
                    Item.HolderPoint holPoint = new Item.HolderPoint(trans, "Ghetto's Firearm SDK V2 vice anchor");
                    firearm.additionalHolderPoints.Add(holPoint);
                }
                firearm.GetHolderPoint(anchorName).anchor.SetPositionAndRotation(target.position, target.rotation);
            }
            firearms = null;
        }

        public void GetAllFirearms()
        {
            firearms = this.gameObject.GetComponentsInChildren<Item>().ToList();
        }
    }
}
