using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThunderRoad;
using UnityEngine.Events;

namespace GhettosFirearmSDKv2
{
    public class GunCase : MonoBehaviour
    {
        public bool isStatic;
        public List<GameObject> nonStaticObjects;
        [Space]
        public Item item;
        public Holder holder;
        [Space]
        public Animator animator;
        public string openingAnimationName;
        public string closingAnimationName;
        [Space]
        public UnityEvent openingEvent;
        public UnityEvent closingEvent;
        public UnityEvent openingStartedEvent;
        public UnityEvent closingFinishedEvent;
        [Space]
        public List<Handle> freezeHandles;
        public List<Handle> toggleHandles;

        private bool closed = true;
        private bool open = false;
        private bool moving = false;

        void Start()
        {

        }

        [EasyButtons.Button]
        public void Open()
        {
            if (open || moving) return;
            StartCoroutine(OpenIE());
        }

        [EasyButtons.Button]
        public void Close()
        {
            if (closed || moving) return;
            StartCoroutine(CloseIE());
        }

        private IEnumerator OpenIE()
        {
            moving = true;
            animator.Play(openingAnimationName);
            openingStartedEvent.Invoke();
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            openingEvent.Invoke();
            open = true;
            closed = false;
            moving = false;
        }

        private IEnumerator CloseIE()
        {
            moving = true;
            closingEvent.Invoke();
            animator.Play(closingAnimationName);
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            closingFinishedEvent.Invoke();
            open = false;
            closed = true;
            moving = false;
        }
    }
}
