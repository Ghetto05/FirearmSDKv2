using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using ThunderRoad;
using UnityEditor;

namespace GhettosFirearmSDKv2
{
    public class AnchorEditors : MonoBehaviour
    {
#if UNITY_EDITOR
        public enum Anchors
        {
            Vice,
            Rack,
            Case
        }

        public Transform RackAnchor;
        public Transform ViceAnchor;
        public Transform CaseAnchor;
        public List<Item> firearms;

        public void GoTo(Anchors anchor)
        {
            GetAllFirearms();
            foreach (Item firearm in firearms)
            {
                firearm.transform.SetParent(Anchor(anchor));
                if (firearm.GetHolderPoint(AnchorName(anchor)) != null)
                {
                    firearm.transform.MoveAlign(firearm.GetHolderPoint(AnchorName(anchor)).anchor, Anchor(anchor));
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
            foreach (Item firearm in firearms)
            {
                if (!firearm.additionalHolderPoints.Any(t => t.anchorName.Equals(AnchorName(anchor))))
                {
                    Transform anch;
                    if (firearm.transform.Find(AnchorName(anchor)) is Transform t)
                    {
                        anch = t;
                    }
                    else
                    {
                        anch = new GameObject(AnchorName(anchor)).transform;
                        anch.parent = firearm.transform;
                        anch.localPosition = Vector3.zero;
                        anch.localEulerAngles = Vector3.zero;
                    }
                    Item.HolderPoint holPoint = new(anch, AnchorName(anchor));
                    firearm.additionalHolderPoints.Add(holPoint);
                }
                firearm.GetHolderPoint(AnchorName(anchor)).anchor.SetPositionAndRotation(Anchor(anchor).position, Anchor(anchor).rotation);
                EditorUtility.SetDirty(firearm.gameObject);
            }
            firearms = null;
        }

        public void GetAllFirearms()
        {
            firearms = gameObject.GetComponentsInChildren<Item>().ToList();
        }

        private string AnchorName(Anchors anchor)
        {
            string s = "";

            switch (anchor)
            {
                case Anchors.Vice:
                    s = "Ghetto's Firearm SDK V2 vice anchor";
                    break;

                case Anchors.Rack:
                    s = "HolderRackTopAnchor";
                    break;

                case Anchors.Case:
                    s = "Ghetto's Firearm SDK V2 gun case anchor";
                    break;
            }

            return s;
        }

        private Transform Anchor(Anchors anchor)
        {
            Transform t = null;

            switch (anchor)
            {
                case Anchors.Vice:
                    t = ViceAnchor;
                    break;

                case Anchors.Rack:
                    t = RackAnchor;
                    break;

                case Anchors.Case:
                    t = CaseAnchor;
                    break;
            }

            return t;
        }
#endif
    }
}
