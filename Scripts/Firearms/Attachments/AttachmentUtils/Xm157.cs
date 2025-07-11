using System;
using System.Collections;
using System.Collections.Generic;
using EasyButtons;
using ThunderRoad;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace GhettosFirearmSDKv2
{
    [ExecuteInEditMode]
    public class Xm157 : MonoBehaviour
    {
        public enum Pages
        {
            MainMenu,
            UiColorSelection
        }

        public enum UiColors
        {
            Yellow,
            Red,
            Orange
        }

        public Scope scope;
        public float distanceToCamera;
        public Laser rangeFinder;
        public Laser visualLaser;
        public Transform scaleRoot;
        public Handle menuHandle;
        public AudioSource buttonSound;
        
        public int monospaceSize = 100;
        public TextMeshProUGUI ui;
        public Pages currentPage;
        public int currentOption;

        public bool compassEnabled;
        public bool rangeFinderEnabled;
        public UiColors uiColor;
        
        public TextMeshProUGUI rangeFinderDisplay;
        public TextMeshProUGUI compassDisplay;
        public Graphic redDot;

        public Graphic[] colorElements;

        private int _currentRowCount;
        private bool _grabbed;
        private bool _lastNonUiState = true;
        private bool _lastRedDotState = true;

        [Button]
        public void Grab()
        {
            ui.enabled = true;
            DrawPage();
            _grabbed = true;
        }
        
        [Button]
        public void UnGrab()
        {
            ui.enabled = false;
            _grabbed = false;
        }

        [Button]
        public void AlternateUse()
        {
            //cycle through options
            if (buttonSound != null)
                buttonSound.Play();
            currentOption++;
            if (currentOption >= _currentRowCount)
                currentOption = 0;
            DrawPage();
        }

        [Button]
        public void Trigger()
        {
            //confirm options
            if (buttonSound != null)
                buttonSound.Play();
            ApplyOption();
            DrawPage();
        }

        private void DrawPage()
        {
            GetPageInfo(out string text, out int rowCount);
            _currentRowCount = rowCount;
            object[] vars = PopulateArray(rowCount, " ");
            vars[currentOption] = ">";
            ui.text = string.Format($"<mspace=mspace={monospaceSize}>" + text + "</mspace>", vars);
            ui.enabled = false;
            ui.enabled = true;
        }

        private void GetPageInfo(out string text, out int rowCount)
        {
            switch (currentPage)
            {
                case Pages.MainMenu:
                    text = "{0} Compass (" + (compassEnabled ? "On" : "Off") + ")<br>" +
                           "{1} Range finder (" + (rangeFinderEnabled ? "On" : "Off") + ")<br>" +
                           "{2} Visual laser (" + (visualLaser == null ? "[ERROR]" : visualLaser.physicalSwitch ? "On" : "Off") + ")<br>" +
                           "{3} HUD color (" + uiColor + ")";
                    rowCount = 4;
                    break;
                case Pages.UiColorSelection:
                    text = "{0} Yellow<br>" +
                           "{1} Red<br>" +
                           "{2} Orange";
                    rowCount = 3;
                    break;
                default:
                    text = "[ERROR]";
                    rowCount = 0;
                    break;
            }
        }

        private void ApplyOption()
        {
            switch (currentPage)
            {
                case Pages.MainMenu:
                    switch (currentOption)
                    {
                        case 0:
                            ToggleCompass();
                            break;
                        case 1:
                            ToggleRangeFinder();
                            break;
                        case 2:
                            ToggleVisualLaser();
                            break;
                        case 3:
                            currentPage = Pages.UiColorSelection;
                            currentOption = 0;
                            break;
                    }
                    break;
                case Pages.UiColorSelection:
                    uiColor = currentOption switch
                    {
                        0 => UiColors.Yellow,
                        1 => UiColors.Red,
                        2 => UiColors.Orange,
                        _ => uiColor
                    };
                    ApplyColor();
                    currentPage = Pages.MainMenu;
                    break;
                default:
                    break;
            }
        }

        private object[] PopulateArray(int count, string defaultValue)
        {
            object[] output = new object[count];
            for (var i = 0; i < output.Length; i++)
                output[i] = defaultValue;
            return output;
        }

        private void Update()
        {
            UpdateScale();
            ToggleNonUiComponents();
            ToggleRedDot();
            UpdateCompass();
            if (rangeFinder != null)
                rangeFinderDisplay.SetText(rangeFinder.range.ToString("0.00") + "m"); 
        }

        #region Options

        private void ToggleCompass()
        {
            compassEnabled = !compassEnabled;
        }

        private void ToggleRangeFinder()
        {
            rangeFinderEnabled = !rangeFinderEnabled;
        }

        private void ToggleVisualLaser()
        {
            if (visualLaser != null)
                visualLaser.physicalSwitch = !visualLaser.physicalSwitch;
        }

        private void ApplyColor()
        {
            foreach (var element in colorElements)
            {
                element.color = 
                    uiColor switch
                    {
                        UiColors.Orange => new Color(1, 0.29f, 0),
                        UiColors.Red => Color.red,
                        UiColors.Yellow => Color.yellow,
                        _ => Color.black
                    };
            }
        }

        private void ToggleNonUiComponents()
        {
            bool a = !_grabbed && (scope == null || scope.currentIndex != 0);
            if (_lastNonUiState != a)
            {
                compassDisplay.enabled = a && compassEnabled;
                rangeFinderDisplay.enabled = a && rangeFinderEnabled;
            }
            _lastNonUiState = a;
        }

        private void ToggleRedDot()
        {
            bool a = !_grabbed && (scope == null || scope.currentIndex == 0);
            if (_lastRedDotState != a)
            {
                redDot.enabled = a;
            }
            _lastRedDotState = a;
        }

        private void UpdateCompass()
        {
            float angle = transform.eulerAngles.y;
            compassDisplay.text = Heading(angle) + angle.ToString("0.00") + "°";
        }

        private void UpdateScale()
        {
            scaleRoot.localScale = Vector3.one * 2.0f * distanceToCamera * Mathf.Tan(scope.cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        }

        private string Heading(float angle)
        {
            return angle switch
            {
                > 22.5f and <= 67.5f => "NE ",
                > 67.5f and <= 112.5f => "E ",
                > 112.5f and <= 157.5f => "SE ",
                > 157.5f and <= 202.5f => "S ",
                > 202.5f and <= 247.5f => "SW" ,
                > 247.5f and <= 292.5f => "W ",
                > 292.5f and <= 337.5f => "NW ",
                _ => "N "
            };
        }

        #endregion
    }
}
