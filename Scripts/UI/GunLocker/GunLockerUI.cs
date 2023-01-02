using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThunderRoad;
using UnityEngine.UI;

namespace GhettosFirearmSDKv2
{
    public class GunLockerUI : MonoBehaviour
    {
        public Transform categoryContent;
        public GameObject categoryPrefab;
        public List<string> categories;
        private List<GameObject> categoryButtons;

        public Transform savesContent;
        public GameObject savesPrefab;
        public List<string> saves;
        private List<GameObject> saveButtons;

        public Holder holder;

        public void SaveWeapon()
        {
            
        }

        [EasyButtons.Button]
        public void SetupCategoryList()
        {
            categories.Remove("Prebuilts");
            categories.Sort();
            categories.Insert(0, "Prebuilts");

            if (categoryButtons != null)
            {
                foreach (GameObject obj in categoryButtons)
                {
                    Destroy(obj);
                }
                categoryButtons.Clear();
            }
            else categoryButtons = new List<GameObject>();

            foreach (string cat in categories)
            {
                GameObject buttonObj = Instantiate(categoryPrefab, categoryContent);
                buttonObj.SetActive(true);
                buttonObj.transform.localPosition = Vector3.zero;
                buttonObj.transform.localEulerAngles = Vector3.zero;
                GunLockerUICategory categoryComp = buttonObj.GetComponent<GunLockerUICategory>();
                categoryComp.button.onClick.AddListener(delegate { SetCategory(cat); });
                categoryComp.textName.text = cat;
                categoryButtons.Add(buttonObj);
            }
        }

        private void SetCategory(string category)
        {
            if (saveButtons != null)
            {
                foreach (GameObject obj in saveButtons)
                {
                    Destroy(obj);
                }
                saveButtons.Clear();
            }
            else saveButtons = new List<GameObject>();

            //categories = GetAllOfCategory();
            foreach (string saveId in saves)
            {
                GameObject buttonObj = Instantiate(savesPrefab, savesContent);
                buttonObj.SetActive(true);
                buttonObj.transform.localPosition = Vector3.zero;
                buttonObj.transform.localEulerAngles = Vector3.zero;
                GunLockerUISave saveComp = buttonObj.GetComponent<GunLockerUISave>();
                saveComp.button.onClick.AddListener(delegate { SpawnSave(saveId); });
                saveComp.textName.text = saveId; //data.displayName;
                saveButtons.Add(buttonObj);
            }
        }

        private void SpawnSave(string saveId)
        {
            
        }
    }
}
