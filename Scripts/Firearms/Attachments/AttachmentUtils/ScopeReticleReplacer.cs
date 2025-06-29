using System.Linq;
using EasyButtons;
using UnityEditor;
using UnityEngine;

namespace GhettosFirearmSDKv2
{
    public class ScopeReticleReplacer : MonoBehaviour
    {
        public Attachment attachment;
        public MeshRenderer newReticle;

        [Button]
        public void GetSetup()
        {
            attachment = GetComponent<Attachment>();
            newReticle = GetComponentsInChildren<MeshRenderer>().FirstOrDefault(x => x.name == "Image");
            if (attachment.minimumMuzzlePosition != null)
                DestroyImmediate(attachment.minimumMuzzlePosition.gameObject);
#if UNITY_EDITOR
            EditorUtility.SetDirty(gameObject);
#endif
        }
    }
}