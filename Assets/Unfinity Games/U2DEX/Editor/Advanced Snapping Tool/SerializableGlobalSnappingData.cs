using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;

using UnfinityGames.Common.Editor;

namespace UnfinityGames.U2DEX
{

	/// <summary>
	/// A container for all of the data that we need to save (settings, etc.)
	/// </summary>
	public class SerializableGlobalSnappingData //: ScriptableObject
	{
		[SerializeField]
		public int versionNumber = 0;

		public static int CurrentVersion = 2; //version number changed to 2 on November, 2013

		[SerializeField]
		public bool transformInspectorEnabled = true;

		[SerializeField]
		public bool enableSnapping = false;

		[SerializeField]
		public bool foldoutOpen = false;

		[SerializeField]
		public float amountToSnapTo = 1;

		[SerializeField]
		public bool snapToGrid = false;

		[SerializeField]
		private string[] applicableClasses;

		[SerializeField]
		public Vector2 gridSize = new Vector2(64, 64);

		[SerializeField]
		public Color gridColor = Color.white;

		[SerializeField]
		public int gridScale = (int)u2dexGrid.GridScale.NearlyUnlimited;

		[SerializeField]
		public bool usePixelsPerMeter = false;

		[SerializeField]
		public float pixelsPerMeter = 1;

		[SerializeField]
		public bool useNGUIInspector = false;

		/// <summary>
		/// Retrieves a list from the internal string array.
		/// </summary>
		/// <returns></returns>
		public List<string> GetClasses()
		{
			List<string> List = new List<string>(applicableClasses.Length);
			foreach (string Class in applicableClasses)
			{
				if (!List.Contains(Class))
				{
					List.Add(Class);
				}
			}

			return List;
		}

		public void SetClasses(List<string> Classes)
		{
			applicableClasses = Classes.ToArray();
		}

		public void Save()
		{
			UnfinityEditorPrefs.SetInt("u2dex_versionNumber", versionNumber);

			UnfinityEditorPrefs.SetBool("u2dex_transformInspectorEnabled", transformInspectorEnabled);

			UnfinityEditorPrefs.SetBool("u2dex_enableSnapping", enableSnapping);

			UnfinityEditorPrefs.SetBool("u2dex_foldoutOpen", foldoutOpen);

			UnfinityEditorPrefs.SetFloat("u2dex_amountToSnapTo", amountToSnapTo);

			UnfinityEditorPrefs.SetBool("u2dex_foldoutOpen", snapToGrid);

			UnfinityEditorPrefs.SetStringArray("u2dex_applicableClasses", applicableClasses);

			UnfinityEditorPrefs.SetVector2("u2dex_gridSize", gridSize);

			UnfinityEditorPrefs.SetColor("u2dex_gridColor", gridColor);

			UnfinityEditorPrefs.SetInt("u2dex_gridScale", gridScale);

			UnfinityEditorPrefs.SetBool("u2dex_usePixelsPerMeter", usePixelsPerMeter);

			UnfinityEditorPrefs.SetFloat("u2dex_pixelsPerMeter", pixelsPerMeter);

			UnfinityEditorPrefs.SetBool("u2dex_useNGUIInspector", useNGUIInspector);
		}

		public void Load(string[] DefaultClasses)
		{
			versionNumber = UnfinityEditorPrefs.GetInt("u2dex_versionNumber", 0);

			transformInspectorEnabled = UnfinityEditorPrefs.GetBool("u2dex_transformInspectorEnabled", true);

			enableSnapping = UnfinityEditorPrefs.GetBool("u2dex_enableSnapping", false);

			foldoutOpen = UnfinityEditorPrefs.GetBool("u2dex_foldoutOpen", false);

			amountToSnapTo = UnfinityEditorPrefs.GetFloat("u2dex_amountToSnapTo", 32);

			snapToGrid = UnfinityEditorPrefs.GetBool("u2dex_foldoutOpen", false);

			//If we have a key and the array has the greater than or equal classes in it (so we always have the defaults)
			if (EditorPrefs.HasKey("u2dex_applicableClasses")
				&& UnfinityEditorPrefs.GetStringArray("u2dex_applicableClasses").Length >= DefaultClasses.Length)
			{
				applicableClasses = UnfinityEditorPrefs.GetStringArray("u2dex_applicableClasses");
			}
			else //otherwise, initialize it with the defaults provided.
			{
				applicableClasses = DefaultClasses;
			}

			gridSize = UnfinityEditorPrefs.GetVector2("u2dex_gridSize", new Vector2(64, 64));

			gridColor = UnfinityEditorPrefs.GetColor("u2dex_gridColor", Color.white);

			usePixelsPerMeter = UnfinityEditorPrefs.GetBool("u2dex_usePixelsPerMeter", false);

			pixelsPerMeter = UnfinityEditorPrefs.GetFloat("u2dex_pixelsPerMeter", 1);

			useNGUIInspector = UnfinityEditorPrefs.GetBool("u2dex_useNGUIInspector", false);
		}
	}
}
