using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AutoQualityChooser
{
	[CustomEditor(typeof(TargetQualitySettings))]
	[CanEditMultipleObjects]
	public class TargetQualitySettingsEditor : Editor
	{
		private TargetQualitySettings _qualitySettings;
		private void OnEnable()
		{
			_qualitySettings = (TargetQualitySettings)target;
		}
		public override void OnInspectorGUI()
		{
			if (GUILayout.Button("Add New Quality Data"))
			{
				_qualitySettings.QualityData.Add(new QualityData());
			}
			EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
			int? markQualityDataForRemoval = null;
			int? markQualityDataForRowExchange = null;
			int? targetMarkQualityDataForRowExchange = null;
			for (int i = 0; i < _qualitySettings.QualityData.Count; i++)
			{
				QualityData qualityData = _qualitySettings.QualityData[i];
				EditorGUILayout.BeginVertical(GUI.skin.box);
				{
					EditorGUI.indentLevel++;
					EditorGUILayout.BeginHorizontal();
					if (i != 0)
					{
						if (GUILayout.Button("Up", GUILayout.MaxWidth(30)))
						{
							markQualityDataForRowExchange = i;
							targetMarkQualityDataForRowExchange = i - 1;
						}

					}
					if (i != _qualitySettings.QualityData.Count-1)
					{
						if (GUILayout.Button("Down", GUILayout.MaxWidth(45)))
						{
							markQualityDataForRowExchange = i;
							targetMarkQualityDataForRowExchange = i + 1;
						}
					}
					qualityData.Foldout = EditorGUILayout.Foldout(qualityData.Foldout, "Quality " + qualityData.Quality);
					if (DrawButton("", "ol minus", GUILayout.MaxWidth(30)))
					{
						markQualityDataForRemoval = i;
					}
					EditorGUILayout.EndHorizontal();
					EditorGUI.indentLevel--;
					if (qualityData.Foldout)
					{
						qualityData.Quality = (Qualities)EditorGUILayout.EnumPopup("Quality ", qualityData.Quality);
						Color color;//green
						Color defaultColor = GUI.backgroundColor;
						ColorUtility.TryParseHtmlString("#14b319", out color);
						GUI.backgroundColor = color;
						if (GUILayout.Button("Add New Property"))
						{
							qualityData.QualityProperties.Add(new QualityProperty());
						}
						GUI.backgroundColor = defaultColor;
						EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
						int? markQualityPropertyForRemoval = null;
						for (int j = 0; j < qualityData.QualityProperties.Count; j++)
						{
							QualityProperty qualityProperty = qualityData.QualityProperties[j];
							bool isTwofold = IsTwoFold(qualityProperty.DeviceParameter);

							EditorGUILayout.BeginHorizontal();
							{
								qualityProperty.DeviceParameter = (DeviceParameter)EditorGUILayout.EnumPopup("Device Parameter", qualityProperty.DeviceParameter, GUILayout.MinWidth(Screen.width * .8f));

								GUILayout.FlexibleSpace();
								EditorGUILayout.BeginVertical();
								{
									if (DrawButton("", "ol minus", GUILayout.MaxWidth(25), GUILayout.MaxHeight(16), GUILayout.MinWidth(25)))
									{
										markQualityPropertyForRemoval = j;
									}
								}
								EditorGUILayout.EndVertical();
							}
							EditorGUILayout.EndHorizontal();

							GUI.enabled = !isTwofold;
							if (GUILayout.Button("Add Argument"))
							{
								qualityProperty.Argument.Add(new TwoFoldValue());
							}
							GUI.enabled = true;

							EditorGUILayout.Separator();
								if (isTwofold && (qualityProperty.Argument.Count == 0 || qualityProperty.Argument.Count > 1))
								{
									qualityProperty.Argument.Clear();
									qualityProperty.Argument.Add(new TwoFoldValue());
								}
								EditorGUILayout.BeginVertical();
								int? markForRemoval = null;
								for (int k = 0; k < qualityProperty.Argument.Count; k++)
								{
									TwoFoldValue argument = qualityProperty.Argument[k];
									if (argument == null)
									{
										argument = new TwoFoldValue();
									}
									EditorGUILayout.BeginHorizontal();
									{
										if (isTwofold)
										{
											argument.FirstValue = EditorGUILayout.TextField(argument.FirstValue);
											EditorGUILayout.LabelField(" to ", GUILayout.MaxWidth(25));
											argument.SecondValue = EditorGUILayout.TextField(argument.SecondValue);
										}
										else
									{
										EditorGUILayout.BeginHorizontal();
										{
											GUILayout.Space(10);
											argument.FirstValue = EditorGUILayout.TextField(argument.FirstValue, GUILayout.MinWidth(Screen.width * .7f));
											GUILayout.FlexibleSpace();
											EditorGUILayout.BeginVertical();
											{
												if (DrawButton("", "ol minus", GUILayout.Width(25), GUILayout.MaxHeight(16)))
												{
													markForRemoval = k;
												}
											}
											EditorGUILayout.EndVertical();
										}
										EditorGUILayout.EndHorizontal();
										}

									}
									EditorGUILayout.EndHorizontal();
								}
								if (markForRemoval != null)
								{
									qualityProperty.Argument.RemoveAt(markForRemoval.Value);
								}
								EditorGUILayout.EndVertical();
							if (j != qualityData.QualityProperties.Count -1)
							{
								EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
							}
						}
						if (markQualityPropertyForRemoval != null)
						{
							qualityData.QualityProperties.RemoveAt(markQualityPropertyForRemoval.Value);
						}
					}
				}
				EditorGUILayout.EndVertical();
				GUILayout.Space(10);
			}
			if (markQualityDataForRemoval != null)
			{
				_qualitySettings.QualityData.RemoveAt(markQualityDataForRemoval.Value);
			}
			if (markQualityDataForRowExchange != null)
			{
				List<QualityData> tempQualitySettings = new List<QualityData>(_qualitySettings.QualityData);
				_qualitySettings.QualityData[markQualityDataForRowExchange.Value] = tempQualitySettings[targetMarkQualityDataForRowExchange.Value];
				_qualitySettings.QualityData[targetMarkQualityDataForRowExchange.Value] = tempQualitySettings[markQualityDataForRowExchange.Value];
			}
			if (GUILayout.Button("Save"))
			{
				EditorUtility.SetDirty(target);
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}
		}

		private bool IsTwoFold(DeviceParameter deviceParameter)
		{
			switch (deviceParameter)
			{
				case DeviceParameter.RAM:
				case DeviceParameter.CPUCOUNT:
				case DeviceParameter.DPI:
					return true;
				default:
					return false;
			}
		}

		private bool DrawButton(string name, string iconName, params GUILayoutOption[] guiOptions)
		{
			GUILayoutOption[] options = guiOptions;

			if (iconName != null || iconName != "")
			{

				GUIContent c = new GUIContent(EditorGUIUtility.IconContent(iconName));
				c.text = name;
				return GUILayout.Button(c, options);
			}
			else
			{
				Debug.LogError("Icon Name was null!");
				return false;
			}


		}
	} 
}
