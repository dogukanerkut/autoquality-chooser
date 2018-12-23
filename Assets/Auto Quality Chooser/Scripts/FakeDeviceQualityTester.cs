using UnityEngine;
using System.Collections.Generic;
namespace AutoQualityChooser
{

    [CreateAssetMenu(menuName = "FakeDeviceTester/FakeDeviceQualityTester")]
    public class FakeDeviceQualityTester : ScriptableObject
    {
        public TargetQualitySettings QualitySettingsToBeTested;
        public List<FakeDeviceInfo> FakeDeviceInfos = new List<FakeDeviceInfo>();

    }
}