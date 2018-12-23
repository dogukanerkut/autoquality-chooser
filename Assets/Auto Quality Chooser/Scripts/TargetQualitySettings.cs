using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;

namespace AutoQualityChooser
{
	[CreateAssetMenu(menuName = "AutoQualityChooser/QualitySettings")]
	public class TargetQualitySettings : ScriptableObject
	{
		public Qualities DefaultQuality = Qualities.High;
		public List<QualityData> QualityData = new List<QualityData>();

		public Qualities CalculateQualitySettings(DeviceInfo deviceInfo = null)
		{
			if (QualityData == null)
			{
				Debug.LogWarning("No quality data is specified. Returning default settings!");
				return DefaultQuality;
			}
			if (QualityData.Count == 0)
			{
				Debug.LogWarning("No quality data found. Returning default settings!");
				return DefaultQuality;
			}

			CustomLogger.Log("Starting the process of identifying a suitable quality setting for this device.", this);

			if (deviceInfo == null)
			{
				deviceInfo = new DeviceInfo();
				deviceInfo.Collect();
			}
			CustomLogger.Log(deviceInfo.ToString(), this);

			QualityData[] _temp = QualityData.OrderByDescending((x) => x.Quality).ToArray();
			CustomLogger.Log("QualityData ordered by descending. First Quality : [" + _temp[0].Quality + "] Last Quality : " + _temp[_temp.Length - 1].Quality + "]", this);

			for (int i = 0; i < _temp.Length; i++)
			{
				CustomLogger.Log("Checking if device [" + deviceInfo.model + "] can handle [" + _temp[i].Quality + "] settings...", this);
				if (CheckModel(deviceInfo.model, _temp[i]) ||
					CheckRAM(deviceInfo.ram, _temp[i]) ||
					CheckCPU(deviceInfo.cpu, _temp[i]) ||
					CheckCPUArchitecture(deviceInfo.cpuArchitecture, _temp[i]) ||
					CheckCPUCount(deviceInfo.cpu_count, _temp[i]) ||
					CheckGFXName(deviceInfo.gfx_name, _temp[i]) ||
					CheckGFXVendor(deviceInfo.gfx_vendor, _temp[i]) ||
					CheckGFXVersion(deviceInfo.gfx_ver, _temp[i]) ||
					CheckScreenResolution(deviceInfo.screen, _temp[i]) ||
					CheckDPI((int)deviceInfo.dpi, _temp[i]) ||
					CheckOSVersion(deviceInfo.os_ver, _temp[i]))
				{
					CustomLogger.Log("Quality settings decided as [" + _temp[i].Quality + "] For device [" + deviceInfo.model + "]", this);
					return _temp[i].Quality;
				}
			}
			CustomLogger.Log("No quality settings decided. Selecting default quality [" + DefaultQuality + "] For device [" + deviceInfo.model + "]", this);
			return DefaultQuality;
		}
		private bool CheckModel(string modelName, QualityData qualityData)
		{
			bool result = IsEqual(modelName, DeviceParameter.MODEL, qualityData);
			CustomLogger.Log(string.Format("• Is Model [{0}] included in [{1}] settings ? [{2}]", modelName, qualityData.Quality, result), this);
			return result;
		}

		private bool CheckRAM(int memorySize, QualityData qualityData)
		{
			bool result = IsWithinRange(memorySize, DeviceParameter.RAM, qualityData);
			CustomLogger.Log(string.Format("• Is Memory Size [{0}] included in [{1}] settings ? [{2}]", memorySize, qualityData.Quality, result), this);
			return result;
		}

		private bool CheckCPU(string cpuName, QualityData qualityData)
		{
			bool result = IsEqual(cpuName, DeviceParameter.CPU, qualityData);
			CustomLogger.Log(string.Format("• Is CPU [{0}] included in [{1}] settings ? [{2}]", cpuName, qualityData.Quality, result), this);
			return result;
		}

		private bool CheckCPUArchitecture(string cpuArchitecture, QualityData qualityData)
		{
			bool result = IsEqual(cpuArchitecture, DeviceParameter.CPUARCHITECTURE, qualityData, true);
			CustomLogger.Log(string.Format("• Is CPU Architecture [{0}] included in [{1}] settings ? [{2}]", cpuArchitecture, qualityData.Quality, result), this);
			return result;
		}

		private bool CheckCPUCount(int cpuCount, QualityData qualityData)
		{
			bool result = IsWithinRange(cpuCount, DeviceParameter.CPUCOUNT, qualityData);
			CustomLogger.Log(string.Format("• Is CPU Count [{0}] included in [{1}] settings ? [{2}]", cpuCount, qualityData.Quality, result), this);
			return result;
		}

		private bool CheckGFXName(string gfxName, QualityData qualityData)
		{
			bool result = IsEqual(gfxName, DeviceParameter.GFXNAME, qualityData);
			CustomLogger.Log(string.Format("• Is GFX Name [{0}] included in [{1}] settings ? [{2}]", gfxName, qualityData.Quality, result), this);
			return result;
		}

		private bool CheckGFXVendor(string gfxVendor, QualityData qualityData)
		{
			bool result = IsEqual(gfxVendor, DeviceParameter.GFXVENDOR, qualityData);
			CustomLogger.Log(string.Format("• Is GFX Vendor [{0}] included in [{1}] settings ? [{2}]", gfxVendor, qualityData.Quality, result), this);
			return result;

		}

		private bool CheckGFXVersion(string gfxVersion, QualityData qualityData)
		{
			bool result = IsEqual(gfxVersion, DeviceParameter.GFXVERSION, qualityData, true);
			CustomLogger.Log(string.Format("• Is GFX Version [{0}] included in [{1}] settings ? [{2}]", gfxVersion, qualityData.Quality, result), this);
			return result;

		}

		private bool CheckScreenResolution(string screenResolution, QualityData qualityData)
		{
			bool result = IsEqual(screenResolution, DeviceParameter.SCREENRESOLUTION, qualityData);
			CustomLogger.Log(string.Format("• Is Screen Resolution [{0}] included in [{1}] settings ? [{2}]", screenResolution, qualityData.Quality, result), this);
			return result;

		}

		private bool CheckDPI(int dpi, QualityData qualityData)
		{
			bool result = IsWithinRange(dpi, DeviceParameter.DPI, qualityData);
			CustomLogger.Log(string.Format("• Is DPI [{0}] included in [{1}] settings ? [{2}]", dpi, qualityData.Quality, result), this);
			return result;

		}

		private bool CheckOSVersion(string osVersion, QualityData qualityData)
		{
			bool result = IsEqual(osVersion, DeviceParameter.OSVERSION, qualityData, true);
			CustomLogger.Log(string.Format("• Is OS Version [{0}] included in [{1}] settings ? [{2}]", osVersion, qualityData.Quality, result), this);
			return result;

		}

		#region HELPER METHODS FOR THIS CLASS

		/// <summary>
		/// Helper method for whether the given value for the <see cref="DeviceParameter"/> is equal with given <see cref="QualityqualityData"/>'s <see cref="TwoFoldValue.FirstValue"/>
		/// </summary>
		/// <param name="value">Value to check for.</param>
		/// <param name="deviceParameter">Which <see cref="DeviceParameter"/> should be checked?</param>
		/// <param name="qualityData">Which <see cref="QualityqualityData"/> should be checked?</param>
		/// <returns></returns>
		private bool IsEqual(string value, DeviceParameter deviceParameter, QualityData qualityData, bool runWithContains = false)
		{
			for (int i = 0; i < qualityData.QualityProperties.Count; i++)
			{
				if (qualityData.QualityProperties[i].DeviceParameter == deviceParameter)
				{
					for (int j = 0; j < qualityData.QualityProperties[i].Argument.Count; j++)
					{
						string expectedValue = qualityData.QualityProperties[i].Argument[j].FirstValue;
						if (runWithContains)
						{
							if(value.Contains(expectedValue, StringComparison.OrdinalIgnoreCase))
							{
								return true;
							}
						}
						if (value == expectedValue)
						{
							return true;
						}
					}
					return false;
				}
			}
			return false;
		}
		/// <summary>
		/// Helper method for whether the given value for the <see cref="DeviceParameter"/> is within the range of given <see cref="QualityqualityData"/>'s <see cref="TwoFoldValue"/>
		/// </summary>
		/// <param name="value">Value to check for.</param>
		/// <param name="deviceParameter">Which <see cref="DeviceParameter"/> should be checked?</param>
		/// <param name="qualityData">Which <see cref="QualityqualityData"/> should be checked?</param>
		/// <returns></returns>
		private bool IsWithinRange(int value, DeviceParameter deviceParameter, QualityData qualityData)
		{
			for (int i = 0; i < qualityData.QualityProperties.Count; i++)
			{
				if (qualityData.QualityProperties[i].DeviceParameter == deviceParameter)
				{
					for (int j = 0; j < qualityData.QualityProperties[i].Argument.Count; j++)
					{
						int expectedMiValue;
						int expectedMaxValue;
						string trimmedFirstValue = qualityData.QualityProperties[i].Argument[j].FirstValue.Trim();
						string trimmedSecondValue = qualityData.QualityProperties[i].Argument[j].SecondValue.Trim();

						if (!int.TryParse(trimmedFirstValue, out expectedMiValue))
						{
							Debug.LogError("[" + qualityData.QualityProperties[i].DeviceParameter + "] first value for [" + qualityData.Quality + "] setting can not be converted to integer. Please check the value given! Value : " + trimmedFirstValue);
							return false;
						}
						if (!int.TryParse(trimmedSecondValue, out expectedMaxValue))
						{
							Debug.LogError("[" + qualityData.QualityProperties[i].DeviceParameter + "] second value for [" + qualityData.Quality + "] setting can not be converted to integer. Please check the value given! Value : " + trimmedSecondValue);
							return false;
						}
						if (value >= expectedMiValue && value <= expectedMaxValue)
						{
							return true;
						}
					}
					return false;
				}
			}
			return false;

		}
		#endregion

	} 
	public enum Qualities
	{
		VeryLow,
		Low,
		Medium,
		High,
		VeryHigh,
		Ultra
	}
}
