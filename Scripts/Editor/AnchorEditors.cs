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
        public List<Firearm> firearms;

        public void GoTo(Anchors anchor)
        {
            string anchorName = anchor == Anchors.Vice ? "Ghetto's Firearm SDK V2 vice anchor" : "HolderRackTopAnchor";
            Transform target = anchor == Anchors.Vice ? ViceAnchor : RackAnchor;
            GetAllFirearms();
            foreach (Firearm firearm in firearms)
            {
                firearm.transform.SetParent(anchor == Anchors.Vice? ViceAnchor : RackAnchor);
                if (firearm.item.GetHolderPoint(anchorName) != null)
                {
                    firearm.transform.MoveAlign(firearm.item.GetHolderPoint(anchorName).anchor, target);
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
            foreach (Firearm firearm in firearms)
            {
                if (firearm.item.GetHolderPoint(anchorName) == null) firearm.AddAnchorsAndFixPreview();
                firearm.item.GetHolderPoint(anchorName).anchor.position = target.position;
                firearm.item.GetHolderPoint(anchorName).anchor.rotation = target.rotation;
            }
            firearms = null;
        }

        public void GetAllFirearms()
        {
            firearms = this.gameObject.GetComponentsInChildren<Firearm>().ToList();
        }
    }
}
