using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GhettosFirearmSDKv2
{
    public class TypePicker : PropertyAttribute
    {
        public enum Types
        {
            Attachment,
            Caliber,
            ExplosiveEffect
        }

        public bool showPopup;
        public Types category;

        public TypePicker(Types category)
        {
            this.category = category;
        }

        public static string[] getJsonArray<T>(string json)
        {
            string newJson = "{ \"array\": " + json + "}";
            Wrapper wrapper = JsonUtility.FromJson<Wrapper>(newJson);
            return wrapper.array;
        }
    }

    [Serializable]
    public class Wrapper
    {
        public string[] array;
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(TypePicker))]
    public class CatalogPickerDrawer : PropertyDrawer
    {
        private string[] list;
        public bool firstShow = true;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) { return 0; }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                TypePicker typePicker = attribute as TypePicker;
                if (firstShow)
                {
                    firstShow = false;

                    string content = "";
                    switch (typePicker.category)
                    {
                        case TypePicker.Types.Attachment:
                            {
                                content = TypeFiles.AttachmentTypesFile().text;
                            }
                            break;
                        case TypePicker.Types.Caliber:
                            {
                                content = TypeFiles.CalibersFile().text;
                            }
                            break;
                        case TypePicker.Types.ExplosiveEffect:
                            {
                                content = TypeFiles.ExplosiveEffectsFile().text;
                            }
                            break;
                        default:
                            return;
                    }

                    Wrapper wrap = JsonUtility.FromJson<Wrapper>(content);

                    typePicker.showPopup = wrap.array.Contains(property.stringValue);
                }

                if (typePicker.showPopup)
                {
                    string content = "";
                    switch (typePicker.category)
                    {
                        case TypePicker.Types.Attachment:
                            {
                                content = TypeFiles.AttachmentTypesFile().text;
                            }
                            break;
                        case TypePicker.Types.Caliber:
                            {
                                content = TypeFiles.CalibersFile().text;
                            }
                            break;
                        case TypePicker.Types.ExplosiveEffect:
                            {
                                content = TypeFiles.ExplosiveEffectsFile().text;
                            }
                            break;
                        default:
                            return;
                    }

                    Wrapper wrap = JsonUtility.FromJson<Wrapper>(content);
                    var allId = wrap.array;
                    list = new string[allId.Length];
                    allId.CopyTo(list, 0);
                }

                EditorGUILayout.BeginHorizontal();

                if (typePicker.showPopup && list.Length > 0)
                {
                    int index = Mathf.Max(0, Array.IndexOf(list, property.stringValue));
                    index = EditorGUILayout.Popup(property.displayName, index, list);
                    property.stringValue = list[index] == "None" ? "" : list[index];
                }
                else
                {
                    property.stringValue = EditorGUILayout.TextField(property.displayName, property.stringValue);
                }

                typePicker.showPopup = GUILayout.Toggle(typePicker.showPopup, "Common", "Button", GUILayout.Width(110));

                EditorGUILayout.EndHorizontal();

                #region unused
                /* Alternative that work in default Unity lists
                CatalogPicker catalogPicker = attribute as CatalogPicker;

                if (catalogPicker.showPopup)
                {
                    if (Catalog.gameData == null)
                    {
                        Catalog.LoadAllJson();
                    }

                    var allId = Catalog.GetAllID(catalogPicker.category);
                    list = new string[allId.Count + 1];
                    list[0] = "None";
                    allId.CopyTo(list, 1);
                }

                var rect = EditorGUI.PrefixLabel(position, label);

                var leftRect = new Rect(rect.x, rect.y, rect.width - buttonWidth - 5, rect.height);
                var rightRect = new Rect(rect.x + rect.width - buttonWidth, rect.y, buttonWidth - 2, rect.height);

                if (catalogPicker.showPopup && list.Length > 0)
                {
                    int index = Mathf.Max(0, Array.IndexOf(list, property.stringValue));
                    index = EditorGUI.Popup(leftRect, index, list);
                    property.stringValue = list[index] == "None" ? "" : list[index];
                }
                else
                {
                    property.stringValue = EditorGUI.TextField(leftRect, property.stringValue);
                }

                catalogPicker.showPopup = EditorGUI.Toggle(rightRect, catalogPicker.showPopup, "Button");
                EditorGUI.LabelField(rightRect, "Toggle picker");
                */
                #endregion unused
            }
            else
            {
                base.OnGUI(position, property, label);
            }
        }
    }
#endif
}
