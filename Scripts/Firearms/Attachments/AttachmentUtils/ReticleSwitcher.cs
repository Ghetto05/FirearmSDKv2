using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThunderRoad;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Attachments/Systems/Sights/Reticle switcher")]
    public class ReticleSwitcher : MonoBehaviour
    {
        public Handle toggleHandle;
        public List<GameObject> reticles;
        public GameObject defaultReticle;
        public AudioSource switchSound;

        private void Awake()
        {
            if (defaultReticle == null && reticles != null && reticles.Count > 0) defaultReticle = reticles[0];
            foreach (GameObject reti in reticles)
            {
                reti.SetActive(false);
            }
            if (defaultReticle != null) defaultReticle.SetActive(true);
        }

        [EasyButtons.Button]
        public void Switch()
        {
            if (defaultReticle == null) return;
            if (reticles != null && reticles.Count > 1)
            {
                if (switchSound != null) switchSound.Play();
                if (reticles.IndexOf(defaultReticle) + 1 < reticles.Count)
                {
                    defaultReticle = reticles[reticles.IndexOf(defaultReticle) + 1];
                }
                else
                {
                    defaultReticle = reticles[0];
                }

                foreach (GameObject reti in reticles)
                {
                    reti.SetActive(false);
                }
                if (defaultReticle != null) defaultReticle.SetActive(true);
            }
        }
    }
}
