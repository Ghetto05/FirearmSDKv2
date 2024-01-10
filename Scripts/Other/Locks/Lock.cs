using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    public class Lock : MonoBehaviour
    {
        public bool inverted = false;
        public bool editorResult;
        
        public bool IsUnlocked()
        {
            return !inverted? GetState() : !GetState();
        }

        public virtual bool GetState()
        {
            return false;
        }

        private void Update()
        {
            editorResult = IsUnlocked();
        }
    }
}
