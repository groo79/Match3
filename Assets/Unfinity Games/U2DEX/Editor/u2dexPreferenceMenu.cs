using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnfinityGames.U2DEX
{
	/// <summary>
	/// A helper class that contains everything related to rendering our U2DEX Preferences entry.
	/// </summary>
	public class u2dexPreferenceMenu
	{
		private static Vector2 _preferenceScrollPosition;  

		[PreferenceItem("U2DEX")]
		public static void OnPreferenceWindow()
		{
			OnPreferenceGUI();
		}

		/// <summary>
		/// A private method that handles rendering our Preferences GUI.
		/// </summary>
		private static void OnPreferenceGUI()
		{
			Vector2 scrollPosition = _preferenceScrollPosition;

			Vector2 localScroll = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Width(365));
			_preferenceScrollPosition = localScroll;
			EditorGUILayout.BeginVertical();
		   
			///Draw our settings...
			AdvancedSnapping_UI.DrawGUI(true);

			GUILayout.EndVertical();
			EditorGUILayout.EndScrollView();
		}
	}
}

