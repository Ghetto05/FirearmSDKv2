using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThunderRoad;
using EasyButtons;

namespace GhettosFirearmSDKv2
{
    public class BreakAction : BoltBase
    {
        public bool ejector = true;

        public Axes foldAxis;
        public float minFoldAngle;
        public float maxFoldAngle;

        public List<AudioSource> lockSounds;
        public List<AudioSource> unlockSounds;

        public List<AudioSource> ejectSounds;
        public List<AudioSource> insertSounds;

        public List<Handle> foregripHandles;
        public Rigidbody rb;
        public Transform barrel;
        public Transform closedPosition;
        public Transform openedPosition;

        public List<string> calibers;
        public List<Transform> muzzles;
        public List<ParticleSystem> muzzleFlashes;
        public List<Hammer> hammers;
        public List<Transform> mountPoints;
        public List<Collider> loadColliders;
        public List<Transform> ejectDirections;
        public List<Transform> ejectPoints;
        private Cartridge[] loadedCartridges;
        public List<float> ejectForces;

        public Transform lockAxis;
        public Transform lockLockedPosition;
        public Transform lockUnlockedPosition;

        public enum Axes
        {
            X,
            Y,
            Z,
        }

        [Button]
        public void GetMuzzlesAndMounts()
        {
            muzzles = new List<Transform>();
            mountPoints = new List<Transform>();

            foreach (Collider c in loadColliders)
            {
                muzzles.Add(c.gameObject.transform.Find("Muzzle"));
                mountPoints.Add(c.gameObject.transform.Find("Mount"));
            }
        }
    }
}
