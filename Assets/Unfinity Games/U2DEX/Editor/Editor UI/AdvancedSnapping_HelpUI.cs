using UnityEngine;
using UnityEditor;
using System.Collections;

using UnfinityGames.Common.Editor;

namespace UnfinityGames.U2DEX
{
	public class AdvancedSnapping_HelpUI : EditorWindow
	{
		private static GUIStyle headingStyle;
		private static GUIStyle textStyle;

		private static Texture2D addClass_Light;
		private static Texture2D addClass_Dark;

		private static Texture2D cameraClass_Light;
		private static Texture2D cameraClass_Dark;

		private static Vector2 scrollPosition;

		private static bool initialized = false;

		/// <summary>
		/// Called to initialize our window after we've created it.
		/// </summary>
		public void Init()
		{
			headingStyle = new GUIStyle(EditorStyles.label);
			headingStyle.fontSize = 16;
			headingStyle.fontStyle = FontStyle.BoldAndItalic;

			textStyle = new GUIStyle(GUI.skin.box);
			textStyle.wordWrap = true;
			textStyle.alignment = TextAnchor.UpperLeft;

			textStyle.normal.textColor = EditorStyles.label.normal.textColor;
			textStyle.onNormal.textColor = EditorStyles.label.onNormal.textColor;
			textStyle.hover.textColor = EditorStyles.label.hover.textColor;
			textStyle.onHover.textColor = EditorStyles.label.onHover.textColor;
			textStyle.focused.textColor = EditorStyles.label.focused.textColor;
			textStyle.onFocused.textColor = EditorStyles.label.onFocused.textColor;
			textStyle.active.textColor = EditorStyles.label.active.textColor;
			textStyle.onActive.textColor = EditorStyles.label.onActive.textColor;

			addClass_Light = EditorGUIUtility.Load("Unfinity Games/U2DEX/Images/AddClass_Light.png") as Texture2D;
			addClass_Dark = EditorGUIUtility.Load("Unfinity Games/U2DEX/Images/AddClass_Dark.png") as Texture2D;

			cameraClass_Light = EditorGUIUtility.Load("Unfinity Games/U2DEX/Images/CameraClass_Light.png") as Texture2D;
			cameraClass_Dark = EditorGUIUtility.Load("Unfinity Games/U2DEX/Images/CameraClass_Dark.png") as Texture2D;

			initialized = true;
		}

		/// <summary>
		/// The OnGUI method.  This handles all the drawing for this editor window.
		/// </summary>
		public void OnGUI()
		{
			//If we haven't initialized yet, manually call the Init method.
			if (!initialized)
				Init();

			//Set up some local variables so we can use the appropriate picture for each Unity skin.
			var addClass = EditorGUIUtility.isProSkin ? addClass_Dark : addClass_Light;
			var cameraClass = EditorGUIUtility.isProSkin ? cameraClass_Dark : cameraClass_Light;

			scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

			EditorGUILayout.LabelField("Understanding the 'Applicable Classes'", headingStyle, GUILayout.Height(25));
			GUILayout.Label("Applicable classes are a way for you (yes you!) to apply the "
				+ u2dexInfo.AbbreviatedName + " 2D Transform Inspector to additional classes that are not already" +
				" officially supported by " + u2dexInfo.CompanyName + ".\n\n" +
				"To further understand the practical use of this feature, let's look at a diagram:",
				textStyle, GUILayout.ExpandWidth(true));

			EditorGUILayout.BeginHorizontal();

			GUILayout.FlexibleSpace();
			GUILayout.Label(new GUIContent(addClass, "A diagram representing an added class."), GUILayout.MinWidth(324));
			GUILayout.FlexibleSpace();

			EditorGUILayout.EndHorizontal();

			GUILayout.Space(15);

			GUILayout.Label("• Verification Status", headingStyle);
			GUILayout.Label("'Verification status' reflects whether the class name you entered is valid.  In our example," +
							" the entered name is invalid, as shown by the scary, red " + UnfinityGUIUtil.XMark + "." +
							"  If the symbol had been a green " + UnfinityGUIUtil.CheckMark + " instead, then that would've meant" +
							" the class had been found, and the " + u2dexInfo.AbbreviatedName + " 2D Transform Inspector had" +
							" successfully been applied to it.", textStyle);

			GUILayout.Space(15);

			GUILayout.Label("• Class Name (or Type)", headingStyle);
			GUILayout.Label("'Class name' represents exactly what it says on the tin.  You enter the name" +
							" of the class or Type you would like to apply the " + u2dexInfo.AbbreviatedName +
							" 2D Transform Inspector to and then check if it passes verification. (For an explanation" +
							" on verification, see the above bulletpoint)\n\nFor example, if you wanted" +
							" to add the Unity Camera to the list of classes/types that use the " +
							u2dexInfo.AbbreviatedName + " 2D Transform Inspector, you would simply enter" +
							" 'Camera' (without the quotes) into the text box.  It should end up looking like this:", textStyle);

			EditorGUILayout.BeginHorizontal();

			GUILayout.FlexibleSpace();
			GUILayout.Label(new GUIContent(cameraClass, "A diagram representing an added Camera class."), textStyle);
			GUILayout.FlexibleSpace();

			EditorGUILayout.EndHorizontal();

			GUILayout.Space(15);

			GUILayout.Label("• Remove Class from List", headingStyle);
			GUILayout.Label("'Remove Class from list' also does exactly what it advertises." +
							" When the button is clicked, the class defined on that line is removed from" +
							" the 'Applicable Classes' list and goes off to... wherever retired data goes.\n\n" +
							"Some retirement community in Florida, I guess?", textStyle);

			GUILayout.Space(25);

			GUILayout.Label("Still need some assistance?", headingStyle);
			GUILayout.Label("No problem.  Just click the button below to visit our forums.  Feel free to" +
							" post your question on there, and we'll see what we can do!", textStyle);

			//Forum button
			EditorGUILayout.BeginHorizontal();

			GUILayout.FlexibleSpace();

			if (GUILayout.Button(new GUIContent("Visit our Forums!", "Click to visit our forums")))
			{
				Application.OpenURL(u2dexInfo.ForumURL);
			}
			GUILayout.FlexibleSpace();

			EditorGUILayout.EndHorizontal();

			GUILayout.Space(5);

			//Close button
			EditorGUILayout.BeginHorizontal();

			GUILayout.FlexibleSpace();
			if (GUILayout.Button(new GUIContent("Close", "Click to close this window")))
			{
				this.Close();
			}
			GUILayout.FlexibleSpace();

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.EndScrollView();
		}
	}
}
