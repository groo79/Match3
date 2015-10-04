using UnityEngine;
using UnityEditor;

namespace UnfinityGames.Common.Editor
{
	/// <summary>
	/// A GUI Utility class for 2D Transform Inspectors.
	/// </summary>
	static class UnfinityGUIUtil
	{
		private static GUIStyle openFoldoutStyle;
		private static GUIStyle closedFoldoutStyle;
		private static GUIStyle openArrowStyle;
		private static GUIStyle closedArrowStyle;
		private static bool initted;

		public static string CheckMark = '\u2714'.ToString();
		public static string XMark = '\u2718'.ToString();

		private static void Init()
		{
			openFoldoutStyle = new GUIStyle();
			//openFoldoutStyle.fontStyle = (FontStyle)1;
			openFoldoutStyle.stretchHeight = true;

			openFoldoutStyle.padding = new RectOffset(5, 0, 4, 0);
			//openFoldoutStyle.fixedHeight = 1;

			openFoldoutStyle.normal.textColor = EditorStyles.label.normal.textColor;
			openFoldoutStyle.onNormal.textColor = EditorStyles.label.onNormal.textColor;
			openFoldoutStyle.hover.textColor = EditorStyles.label.hover.textColor;
			openFoldoutStyle.onHover.textColor = EditorStyles.label.onHover.textColor;
			openFoldoutStyle.focused.textColor = EditorStyles.label.focused.textColor;
			openFoldoutStyle.onFocused.textColor = EditorStyles.label.onFocused.textColor;
			openFoldoutStyle.active.textColor = EditorStyles.label.active.textColor;
			openFoldoutStyle.onActive.textColor = EditorStyles.label.onActive.textColor;

			closedFoldoutStyle = new GUIStyle(openFoldoutStyle);
			openFoldoutStyle.normal = openFoldoutStyle.onNormal;
			openFoldoutStyle.active = openFoldoutStyle.onActive;

			openArrowStyle = new GUIStyle(openFoldoutStyle);
		   
			//we have to totally doctor the padding for 4.3 forward since Unity changed how Editor GUI items
			//are spaced by default.  It's kind of messy, but by doing this we maintain the feature of the whole
			//foldout being clickable.
			openArrowStyle.padding = new RectOffset(-11, -11, 3, 0);
			openFoldoutStyle.padding = new RectOffset(openFoldoutStyle.padding.left + 11, openFoldoutStyle.padding.right,
													  openFoldoutStyle.padding.top, openFoldoutStyle.padding.bottom);

			openArrowStyle.fontSize = 12;
			//Create a color for our foldout arrow that matches Unity's colors for each skin type.
			var foldoutArrowColor = EditorGUIUtility.isProSkin ? new Color(0.411f, 0.411f, 0.411f) : new Color(0.462f, 0.462f, 0.462f);

			openArrowStyle.normal.textColor = foldoutArrowColor;
			openArrowStyle.onNormal.textColor = foldoutArrowColor;
			openArrowStyle.hover.textColor = foldoutArrowColor;
			openArrowStyle.onHover.textColor = foldoutArrowColor;
			openArrowStyle.focused.textColor = foldoutArrowColor;
			openArrowStyle.onFocused.textColor = foldoutArrowColor;
			openArrowStyle.active.textColor = foldoutArrowColor;
			openArrowStyle.onActive.textColor = foldoutArrowColor;

			closedArrowStyle = new GUIStyle(openArrowStyle);

			//we have to totally doctor the padding for 4.3 forward since Unity changed how Editor GUI items
			//are spaced by default.  It's kind of messy, but by doing this we maintain the feature of the whole
			//foldout being clickable.
			closedArrowStyle.padding = new RectOffset(-11, -11, -2, 0);
			closedFoldoutStyle.padding = new RectOffset(closedFoldoutStyle.padding.left + 12, closedFoldoutStyle.padding.right,
													  closedFoldoutStyle.padding.top, closedFoldoutStyle.padding.bottom);
			closedArrowStyle.fontSize = 15;

			initted = true;
		}

		/// <summary>
		/// Draws a custom foldout that also comes with a toggle-box attached.  Also allows for the foldout text to be clicked.
		/// </summary>
		/// <param name="open"></param>
		/// <param name="toggled"></param>
		/// <param name="text"></param>
		/// <returns></returns>
		public static bool FoldoutToggle(bool open, ref bool toggled, string text) { return FoldoutToggle(open, ref toggled, new GUIContent(text)); }

		/// <summary>
		/// Draws a custom foldout that also comes with a toggle-box attached.  Also allows for the foldout text to be clicked.
		/// </summary>
		/// <param name="open"></param>
		/// <param name="toggled"></param>
		/// <param name="text"></param>
		/// <returns></returns>
		//Based off of code from http://answers.unity3d.com/questions/361836/particle-system-toggle-in-custom-editor.html
		public static bool FoldoutToggle(bool open, ref bool toggled, GUIContent text)
		{
			if (!initted) Init();

			if (open)
			{
				GUILayout.BeginHorizontal();
				if (GUILayout.Button(new GUIContent("▼", text.tooltip), openArrowStyle, GUILayout.Height(20))
					|| GUILayout.Button(text, openFoldoutStyle, GUILayout.Height(20)))
				{
					GUI.FocusControl("");
					GUI.changed = false; // force change-checking group to take notice
					GUI.changed = true;
					return false;
				}
				GUILayout.Space(10);
				toggled = GUILayout.Toggle(toggled, new GUIContent("", text.tooltip), GUILayout.Width(30));
				GUILayout.EndHorizontal();
			}
			else
			{
				closedArrowStyle.fontSize = 19;
				GUILayout.BeginHorizontal();
				if (GUILayout.Button(new GUIContent("\u25B6", text.tooltip), closedArrowStyle, GUILayout.Height(20))
					|| GUILayout.Button(text, closedFoldoutStyle, GUILayout.Height(20)))
				{
					GUI.FocusControl("");
					GUI.changed = false; // force change-checking to take notice
					GUI.changed = true;

					//Don't forget to set the toggle button here, since we return before doing anything with it!
					toggled = true;

					return true;
				}
				GUILayout.Space(10);
				toggled = GUILayout.Toggle(toggled, new GUIContent("", text.tooltip), GUILayout.Width(30));
				GUILayout.EndHorizontal();
			}
			return open;
		}

		/// <summary>
		/// Starts a horizontal region that will center whatever is inside of it.
		/// </summary>
		public static void StartCenter()
		{
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
		}

		/// <summary>
		/// Ends a horizontal region that will center whatever is inside of it.
		/// </summary>
		public static void EndCenter()
		{
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}

		/// <summary>
		/// Adds some vertical space for our GUI, but only on Unity 4.3 forward (since older versions don't need it).
		/// </summary>
		public static void Unity4Space()
		{
			GUILayout.Space(2);
		}
	}
}
