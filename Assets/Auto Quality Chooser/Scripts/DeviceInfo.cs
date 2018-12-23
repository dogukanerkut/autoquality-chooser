using UnityEngine;

namespace AutoQualityChooser
{
    [System.Serializable]
    public class DeviceInfo
    {
        public string model;
        public string manufacturer;//not used but retrieved
        public string device;//not used but retrieved
        public int ram;
        public string cpu;
        public string cpuArchitecture;
        public string gfx_name;
        public string gfx_vendor;
        public int cpu_count;
        public float dpi;
        public string screen;
        public int platform_id;
        public string os_ver;
        public int gfx_shader;
        public string gfx_ver;
        public int max_texture_size;

        public void Collect()
        {
            string manufacturerDeviceModel = GetDeviceModel();
            string[] s = manufacturerDeviceModel.Split('|');
            if (s != null && s.Length == 3)
            {
                this.manufacturer = s[0];
                this.model = s[1];
                this.device = s[2];
            }

            this.ram = SystemInfo.systemMemorySize;
            this.cpu = SystemInfo.processorType;
            this.cpuArchitecture = GetCPUArchitecture();
            this.cpu_count = SystemInfo.processorCount;
            this.gfx_name = SystemInfo.graphicsDeviceName;
            this.gfx_vendor = SystemInfo.graphicsDeviceVendor;
            string screenWithHertz = Screen.currentResolution.ToString();
            s = screenWithHertz.Split('@');
            if (s != null && s.Length == 2)
            {
                this.screen = s[0].Trim();
            }
            else
            {
                this.screen = Screen.currentResolution.ToString();
            }
            this.dpi = Screen.dpi;
            if (Application.isEditor)
            {
                this.platform_id = (int)RuntimePlatform.Android;
            }
            else
            {
                this.platform_id = (int)Application.platform;
            }
            this.os_ver = SystemInfo.operatingSystem;
            this.gfx_shader = SystemInfo.graphicsShaderLevel;
            this.gfx_ver = SystemInfo.graphicsDeviceVersion;
            this.max_texture_size = SystemInfo.maxTextureSize;
        }
        public override string ToString()
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            stringBuilder.Append("---Device Information---\n");
            stringBuilder.Append("Manufacturer : {" + manufacturer + "}\n");
            stringBuilder.Append("Model : {" + model + "}\n");
            stringBuilder.Append("Device : {" + device + "}\n");
            stringBuilder.Append("RAM : {" + ram + "}\n");
            stringBuilder.Append("CPU : {" + cpu + "}\n");
            stringBuilder.Append("CPU Architecture : {" + cpuArchitecture + "}\n");
            stringBuilder.Append("CPU Count  : {" + cpu_count + "}\n");
            stringBuilder.Append("-----\n");
            stringBuilder.Append("GFX Name  : {" + gfx_name + "}\n");
            stringBuilder.Append("GFX Vendor : {" + gfx_vendor + "}\n");
            stringBuilder.Append("GFX Shader : {" + gfx_shader + "}\n");
            stringBuilder.Append("GFX Version : {" + gfx_ver + "}\n");
            stringBuilder.Append("-----\n");
            stringBuilder.Append("Screen : {" + screen + "}\n");
            stringBuilder.Append("DPI : {" + dpi + "}\n");
            stringBuilder.Append("-----\n");
            stringBuilder.Append("Platform ID : {" + platform_id + "}\n");
            stringBuilder.Append("OS Version : {" + os_ver + "}\n");
            stringBuilder.Append("Max Texture Size : {" + max_texture_size + "}");

            return stringBuilder.ToString();
        }

        private string GetDeviceModel()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
			// get manufacturer/model/device
			AndroidJavaClass jc = new AndroidJavaClass("android.os.Build");
			string manufacturer = jc.GetStatic<string>("MANUFACTURER");
			string model = jc.GetStatic<string>("MODEL");
			string device = jc.GetStatic<string>("DEVICE");
			return string.Format("{0}|{1}|{2}", manufacturer, model, device);
#else
            return SystemInfo.deviceModel;
#endif
        }

        private string GetCPUArchitecture()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
		AndroidJavaClass jc = new AndroidJavaClass("java.lang.System");
		string cpuArchitecture = jc.CallStatic<string>("getProperty", "os.arch");
		return cpuArchitecture;
#else
            return "notidentified";
#endif
        }
    }
}
