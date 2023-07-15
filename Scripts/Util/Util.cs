using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;
using UnityEditor;

namespace GhettosFirearmSDKv2
{
    public class Util
    {
        public static void FixAudioSources(GameObject gameObject)
        {
            foreach (AudioSource a in gameObject.GetComponentsInChildren<AudioSource>())
            {
                #region mixer
                if (a.gameObject.GetComponent<AudioMixerLinker>() is AudioMixerLinker linker)
                {
                    linker.audioMixer = AudioMixerName.Effect;
                }
                else
                {
                    linker = a.gameObject.AddComponent<AudioMixerLinker>();
                    linker.audioMixer = AudioMixerName.Effect;
                }
                #endregion mixer
                a.spatialBlend = 1f;
                a.playOnAwake = false;
            }
#if UNITY_EDITOR
            EditorUtility.SetDirty(gameObject);
#endif
        }

        public static bool CheckForCollisionWithThisCollider(Collision collision, Collider thisCollider)
        {
            foreach (ContactPoint con in collision.contacts)
            {
                if (con.thisCollider == thisCollider) return true;
            }
            return false;
        }

        public static bool CheckForCollisionWithOtherCollider(Collision collision, Collider otherCollider)
        {
            foreach (ContactPoint con in collision.contacts)
            {
                if (con.otherCollider == otherCollider) return true;
            }
            return false;
        }

        public static bool CheckForCollisionWithBothColliders(Collision collision, Collider thisCollider, Collider otherCollider)
        {
            foreach (ContactPoint con in collision.contacts)
            {
                if (con.thisCollider == thisCollider && con.otherCollider == otherCollider) return true;
            }
            return false;
        }

        public static void IgnoreCollision(GameObject obj1, GameObject obj2, bool ignore)
        {
            foreach (Collider c1 in obj1.GetComponentsInChildren<Collider>())
            {
                foreach (Collider c2 in obj2.GetComponentsInChildren<Collider>())
                {
                    Physics.IgnoreCollision(c1, c2, ignore);
                }
            }
        }

        private static IEnumerator DelayIgnoreCollisionCoroutine(GameObject obj1, GameObject obj2, bool ignore, float delay)
        {
            yield return new WaitForSeconds(delay);
            IgnoreCollision(obj1, obj2, ignore);
            yield break;
        }

        public static void PlayRandomAudioSource(List<AudioSource> sources)
        {
            AudioSource source = GetRandomFromList(sources);
            if (source != null) source.Play();
        }

        public static void PlayRandomAudioSource(AudioSource[] sources) => PlayRandomAudioSource(sources.ToList());

        public static float AbsDist(Vector3 v1, Vector3 v2)
        {
            return Mathf.Abs(Vector3.Distance(v1, v2));
        }

        public static T GetRandomFromList<T>(List<T> list)
        {
            if (list == null || list.Count == 0) return default;
            int i = Random.Range(0, list.Count);
            return list[i];
        }

        public static T GetRandomFromList<T>(IList<T> array)
        {
            if (array == null) return default;
            return GetRandomFromList(array.ToList());
        }

        public static void DelayedExecute(float delay, System.Action action, MonoBehaviour handler)
        {
            handler.StartCoroutine(DelayedExecuteIE(delay, action));
        }

        private static IEnumerator DelayedExecuteIE(float delay, System.Action action)
        {
            yield return new WaitForSeconds(delay);
            action.Invoke();
        }
    }
}
