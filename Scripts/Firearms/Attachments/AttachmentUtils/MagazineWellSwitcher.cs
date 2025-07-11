using UnityEngine;

namespace GhettosFirearmSDKv2
{
    [AddComponentMenu("Firearm SDK v2/Attachments/Systems/Magazines/Magazine well type switcher")]
    public class MagazineWellSwitcher : MonoBehaviour
    {
        public Attachment attachment;
        private string originalType;
        public string newType;

        private string originalCaliber;
        public string newCaliber;

        private string originalDefaultAmmo;
        public string newDefaultAmmo;
        public MagazineLoad overrideMagazineLoad;
    }
}
