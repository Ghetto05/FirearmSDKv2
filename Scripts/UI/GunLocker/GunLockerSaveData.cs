using System;
using UnityEngine;
using ThunderRoad;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

namespace GhettosFirearmSDKv2
{
    public class GunLockerSaveData : CustomData
    {
        public string displayName;
        public string itemId;
        public string category;
        public List<ContentCustomData> dataList;
    }
}
