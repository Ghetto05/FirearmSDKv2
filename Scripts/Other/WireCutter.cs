using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThunderRoad;

namespace GhettosFirearmSDKv2
{
    public class WireCutter : MonoBehaviour
    {
        public Item item;
        public Animator animator;
        public string clipAnimation;
        public float clipAnimationLength;
        public Transform clipRoot;
        public float clipRange = 0.02f;
        public AudioSource[] clipSounds;
        private float _lastClipTime = 0f;

        private void Start()
        {
        
        }

        private void Clip()
        {
            if (Time.time - _lastClipTime < clipAnimationLength)
                return;
            _lastClipTime = Time.time;
            
            Util.PlayRandomAudioSource(clipSounds);
            animator.Play(clipAnimation);

            Collider[] hits = Physics.OverlapSphere(clipRoot.position, clipRange);
            if (hits.Length > 0)
                WireCutterCuttable.CutFound(hits);
        }
    }
}
