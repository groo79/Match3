using UnityEngine;
using UnityEditor;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UnfinityGames.Common.Editor
{
	class UnfinityChangelogViewer : EditorWindow
	{
		string[] changelog = null;

		//default the height to 200 px tall.
		public int height = 300;

		bool hadError = false;

		GUIStyle DefaultTextStyle;
		GUIStyle HeadingTextStyle;
		private static bool initialized = false;

		Vector2 scrollPos = Vector2.zero;

		public void SetString(string Changelog)
		{
			changelog = FormatString(Changelog);

			//height = padding + ((number of lines * 2) + 1/3 of the space between lines) * roughed font size average.
			//this is kind of a "it just works and looks good" kind of formula, so don't think it's some magical
			//equation...
			//height = 40 + ((int)((changelog.Length * 2) + 3.33f) * 12);

			if (Changelog == UnfinityUpdater.changelogError)
			{
				hadError = true;
			}

			if (hadError)
			{
				//since we know the rough height of the error message, just hardcode the height in. 
				//No need for dynamic height detection.
				height = 60;
			}

			//If we're setting things up in here, we should reinitialize our styles, too.
			initialized = false;
		}

		void InitStyles()
		{
			DefaultTextStyle = new GUIStyle(EditorStyles.label) { wordWrap = true };
			HeadingTextStyle = new GUIStyle(EditorStyles.label) { wordWrap = true, fontSize = 15 };
			initialized = true;
		}

		void OnGUI()
		{
			if (changelog != null)
			{
				//If we haven't initialized yet, manually call the InitStyles method.
				if (!initialized)
					InitStyles();

				//draw the heading first...

				//only center the text if we had an error and need to display an error message
				if (hadError)
				{
					UnfinityGUIUtil.StartCenter();
				}
				string bottomHeaderText = (hadError) ? "" : "\n______________________________________";
				GUILayout.Label(changelog[0] + bottomHeaderText, HeadingTextStyle);

				//only center the text if we had an error and need to display an error message
				if (hadError)
				{
					UnfinityGUIUtil.EndCenter();
				}

				scrollPos = GUILayout.BeginScrollView(scrollPos);
				GUILayout.BeginVertical();

				//start on 1, since we've done the heading already...
				for (int i = 1; i < changelog.Length; i++)
				{
					//only center the text if we had an error and need to display an error message
					if (hadError)
					{
						UnfinityGUIUtil.StartCenter();
					}
					//since we're not on first line, don't use a special font.
					GUILayout.Label(changelog[i], DefaultTextStyle);
					//add a little space between lines...
					GUILayout.Space(10);

					//only center the text if we had an error and need to display an error message
					if (hadError)
					{
						UnfinityGUIUtil.EndCenter();
					}
				}

				GUILayout.EndVertical();
				GUILayout.EndScrollView();

				//Add a button to close the window, if there was no error.
				if (!hadError)
				{
					UnfinityGUIUtil.StartCenter();
					if (GUILayout.Button(new GUIContent("Close", "Close this window"), GUILayout.Width(100)))
					{
						this.Close();
					}
					UnfinityGUIUtil.EndCenter();
				}
			}
			else
			{
				UnfinityGUIUtil.StartCenter();

				GUILayout.Label("Loading the Changelog...");

				UnfinityGUIUtil.EndCenter();
			}
		}

		private string[] FormatString(string Changelog)
		{
			return Changelog.Split(new string[] { "\n", "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);
		}
	}
}