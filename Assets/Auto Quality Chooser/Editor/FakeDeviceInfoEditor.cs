using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AutoQualityChooser
{

    [CustomEditor(typeof(FakeDeviceInfo))]
    public class FakeDeviceInfoEditor : Editor
    {
        private string _deviceInfoString;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.LabelField("Paste Device Information Here");
            _deviceInfoString = EditorGUILayout.TextArea(_deviceInfoString);

            if (GUILayout.Button("Apply to fields"))
            {
                string[] firstSplit = _deviceInfoString.Split('{');
                List<string> actualInformations = new List<string>();
                for (int i = 1; i < firstSplit.Length; i++)
                {
                    string[] secondSplit = firstSplit[i].Split('}');
                    actualInformations.Add(secondSplit[0]);
                }

                FakeDeviceInfo fakeDeviceInfo = (FakeDeviceInfo)target;
                fakeDeviceInfo.DeviceInfo.manufacturer = actualInformations[0];
                fakeDeviceInfo.DeviceInfo.model = actualInformations[1];
                fakeDeviceInfo.DeviceInfo.device = actualInformations[2];
                fakeDeviceInfo.DeviceInfo.ram = int.Parse(actualInformations[3]);
                fakeDeviceInfo.DeviceInfo.cpu = actualInformations[4];
                fakeDeviceInfo.DeviceInfo.cpuArchitecture = actualInformations[5];
                fakeDeviceInfo.DeviceInfo.cpu_count = int.Parse(actualInformations[6]);
                fakeDeviceInfo.DeviceInfo.gfx_name = actualInformations[7];
                fakeDeviceInfo.DeviceInfo.gfx_vendor = actualInformations[8];
                fakeDeviceInfo.DeviceInfo.gfx_shader = int.Parse(actualInformations[9]);
                fakeDeviceInfo.DeviceInfo.gfx_ver = actualInformations[10];
                fakeDeviceInfo.DeviceInfo.screen = actualInformations[11];
                fakeDeviceInfo.DeviceInfo.dpi = int.Parse(actualInformations[12]);
                fakeDeviceInfo.DeviceInfo.platform_id = int.Parse(actualInformations[13]);
                fakeDeviceInfo.DeviceInfo.os_ver = actualInformations[14];
                fakeDeviceInfo.DeviceInfo.max_texture_size = int.Parse(actualInformations[15]);
                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
    }
}
