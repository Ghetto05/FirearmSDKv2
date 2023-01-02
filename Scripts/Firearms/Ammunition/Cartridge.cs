using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Ammunition/Cartridge")]
    public class Cartridge : MonoBehaviour
    {
        public PhysicMaterial smallCasing;
        public PhysicMaterial mediumCasing;
        public PhysicMaterial heavyCasing;
        public PhysicMaterial shell;

        public GameObject firedOnlyObject;
        public GameObject unfiredOnlyObject;
        public string caliber;
        public bool destroyOnFire = false;
        public ProjectileData data;
        [HideInInspector]
        public bool fired = false;
        public ParticleSystem detonationParticle;
        public AudioSource[] detonationSounds;
        public ParticleSystem additionalMuzzleFlash;
        public List<Collider> colliders;
        public Transform cartridgeFirePoint;
        public UnityEvent onFireEvent;

        [EasyButtons.Button]
        public void FindAllCollider()
        {
            colliders = this.gameObject.GetComponentsInChildren<Collider>().ToList();
        }

        public void Detonate()
        {
        }

        [EasyButtons.Button]
        public void SetSmallCasing()
        {
            foreach (Collider c in colliders)
            {
                c.sharedMaterial = smallCasing;
                c.material = smallCasing;
            }
#if UNITY_EDITOR
            EditorUtility.SetDirty(gameObject);
#endif
        }

        [EasyButtons.Button]
        public void SetMediumCasing()
        {
            foreach (Collider c in colliders)
            {
                c.sharedMaterial = mediumCasing;
                c.material = mediumCasing;
            }
#if UNITY_EDITOR
            EditorUtility.SetDirty(gameObject);
#endif
        }

        [EasyButtons.Button]
        public void SetHeavyCasing()
        {
            foreach (Collider c in colliders)
            {
                c.sharedMaterial = heavyCasing;
                c.material = heavyCasing;
            }
#if UNITY_EDITOR
            EditorUtility.SetDirty(gameObject);
#endif
        }

        [EasyButtons.Button]
        public void SetShell()
        {
            foreach (Collider c in colliders)
            {
                c.sharedMaterial = shell;
                c.material = shell;
            }
#if UNITY_EDITOR
            EditorUtility.SetDirty(gameObject);
#endif
        }
    }
}
