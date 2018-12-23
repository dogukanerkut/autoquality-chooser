using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AutoQualityChooser
{

    [CustomEditor(typeof(FakeDeviceQualityTester))]
    public class FakeDeviceQualityTesterEditor : Editor
    {
        private FakeDeviceQualityTester _fakeDeviceQualityTester;
        private void OnEnable()
        {
            _fakeDeviceQualityTester = (FakeDeviceQualityTester)target;
        }
        private System.Text.StringBuilder _results;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUI.enabled = _fakeDeviceQualityTester.QualitySettingsToBeTested != null && _fakeDeviceQualityTester.FakeDeviceInfos.Count > 0;
            if (!GUI.enabled)
            {
                EditorGUILayout.HelpBox("Attach a quality setting and some fake device infos to run the test", MessageType.Warning);
            }

            if (GUILayout.Button("Test Fake Devices"))
            {
                _results = new System.Text.StringBuilder();
                _results.Append("Testing for " + _fakeDeviceQualityTester.FakeDeviceInfos.Count + " devices...\n");
                for (int i = 0; i < _fakeDeviceQualityTester.FakeDeviceInfos.Count; i++)
                {
                    Qualities selectedQuality = _fakeDeviceQualityTester.QualitySettingsToBeTested.CalculateQualitySettings(_fakeDeviceQualityTester.FakeDeviceInfos[i].DeviceInfo);

                    _results.Append(string.Format("• {0}({1}) <b>{2}{3}</color></b>\n", _fakeDeviceQualityTester.FakeDeviceInfos[i].name, _fakeDeviceQualityTester.FakeDeviceInfos[i].DeviceInfo.model, GetColorCodeForQuality(selectedQuality), selectedQuality.ToString()));
                }
                _results.Append("\nYou can also see the logs for details.");
            }

            if (_results != null && _results.Length > 0)
            {
                GUIStyle style = new GUIStyle(GUI.skin.label);
                style.richText = true;
                style.wordWrap = true;
                EditorGUILayout.BeginVertical(GUI.skin.box);
                {
                    EditorGUILayout.LabelField(_results.ToString(), style);

                }
                EditorGUILayout.EndVertical();
            }
        }

        private string GetColorCodeForQuality(Qualities quality)
        {
            switch (quality)
            {
                case Qualities.VeryLow:
                    return "<color=#FF2D00>";
                case Qualities.Low:
                    return "<color=#FF7800>";
                case Qualities.Medium:
                    return "<color=#FFD800>";
                case Qualities.High:
                    return "<color=#D1FF00>";
                case Qualities.VeryHigh:
                    return "<color=#8FFF00>";
                case Qualities.Ultra:
                    return "<color=#00FF08>";
                default:
                    return "<color=#FF2D00>";
            }
        }
    }
}
