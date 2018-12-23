using System.Collections.Generic;

namespace AutoQualityChooser
{
	[System.Serializable]
	public class QualityData
	{
#if UNITY_EDITOR
		public bool Foldout;
#endif
		public Qualities Quality;
		public List<QualityProperty> QualityProperties = new List<QualityProperty>();
	}
	[System.Serializable]
	public class QualityProperty
	{
		public DeviceParameter DeviceParameter;
		public List<TwoFoldValue> Argument = new List<TwoFoldValue>();
	}
	public enum DeviceParameter
	{
		MODEL,
		RAM,
		CPU,
		CPUARCHITECTURE,
		CPUCOUNT,
		GFXNAME,
		GFXVENDOR,
		GFXSHADER,
		GFXVERSION,
		SCREENRESOLUTION,
		DPI,
		OSVERSION
	}
	[System.Serializable]
	public class TwoFoldValue
	{
		public string FirstValue;
		public string SecondValue;
	}

}