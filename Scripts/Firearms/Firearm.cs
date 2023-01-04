﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Firearm")]
    [RequireComponent(typeof(Item))]
    public class Firearm : FirearmBase
    {
        public List<AttachmentPoint> attachmentPoints;

        [EasyButtons.Button]
        public void SetAudioSourceMixers()
        {
            Util.FixLinkers(gameObject);
        }

        [EasyButtons.Button]
        public void FindAllAttachmentPoints()
        {
            attachmentPoints = new List<AttachmentPoint>();
            foreach (AttachmentPoint point in this.gameObject.GetComponentsInChildren<AttachmentPoint>())
            {
                if (!attachmentPoints.Contains(point)) attachmentPoints.Add(point);
            }
        }

        [EasyButtons.Button]
        public void AddAnchorsAndFixPreview()
        {
            item = this.gameObject.GetComponent<Item>();
            item.holderPoint.localEulerAngles = new Vector3(0, 0, 90);

            if (HasAnchor("Ghetto's Firearm SDK V2 vice anchor"))
            {

            }
            else
            {
                Transform trans = new GameObject("Ghetto's Firearm SDK V2 vice anchor").transform;
                trans.parent = this.transform;
                trans.localPosition = Vector3.zero;
                trans.localEulerAngles = Vector3.zero;
                Item.HolderPoint holPoint = new Item.HolderPoint(trans, "Ghetto's Firearm SDK V2 vice anchor");
                item.additionalHolderPoints.Add(holPoint);
            }
            if (HasAnchor("HolderRackTopAnchor"))
            {

            }
            else
            {
                Transform trans = new GameObject("HolderRackTopAnchor").transform;
                trans.parent = this.transform;
                trans.localPosition = Vector3.zero;
                trans.localEulerAngles = Vector3.zero;
                Item.HolderPoint holPoint = new Item.HolderPoint(trans, "HolderRackTopAnchor");
                item.additionalHolderPoints.Add(holPoint);
            }
            item.preview.transform.localEulerAngles = new Vector3(8.88f, -115.118f, 0f);
        }

        void Reset()
        {
            AddAnchorsAndFixPreview();
            if (hitscanMuzzle == null)
            {
                Transform mademuzzle = new GameObject("HitscanMuzzle").transform;
                mademuzzle.SetParent(this.transform);
                mademuzzle.localPosition = Vector3.zero;
                mademuzzle.localEulerAngles = Vector3.zero;
                hitscanMuzzle = mademuzzle;
            }
        }

        private bool HasAnchor(string s)
        {
            foreach (Item.HolderPoint point in item.additionalHolderPoints)
            {
                if (point.anchorName.Equals(s)) return true;
            }
            return false;
        }
    }
}