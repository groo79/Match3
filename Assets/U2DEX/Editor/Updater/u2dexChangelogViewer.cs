using UnityEngine;
using UnityEditor;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//if we're using unity 3.5, we need this reference.
#if UNITY_3_5
using u2dex;
#endif

//only use namespaces if we aren't using unity 3.5 (only 4.0 or greater)
#if !UNITY_3_5
namespace u2dex
{
#endif   

    class u2dexChangelogViewer : EditorWindow
    {
        string[] changelog = null;

        //default the height to 200 px tall.
        public int height = 300;

        bool hadError = false;

        GUIStyle DefaultTextStyle;
        GUIStyle HeadingTextStyle;

        Vector2 scrollPos = Vector2.zero;

        public void SetString(string Changelog)
        {
            changelog = FormatString(Changelog);

            //height = padding + ((number of lines * 2) + 1/3 of the space between lines) * roughed font size average.
            //this is kind of a "it just works and looks good" kind of formula, so don't think it's some magical
            //equation...
            //height = 40 + ((int)((changelog.Length * 2) + 3.33f) * 12);

            if (Changelog == u2dexUpdater.changelogError)
            {
                hadError = true;
            }

            if (hadError)
            {
                //since we know the rough height of the error message, just hardcode the height in. 
                //No need for dynamic height detection.
                height = 60;
            }

            DefaultTextStyle = new GUIStyle(GUI.skin.label) { wordWrap = true };
            HeadingTextStyle = new GUIStyle(GUI.skin.label) { wordWrap = true, fontSize = 15 };
        }

        void OnGUI()
        {
            if (changelog != null)
            {

                //draw the heading first...

                //only center the text if we had an error and need to display an error message
                if (hadError)
                {
                    u2dexGUIUtil.StartCenter();
                }
                string bottomHeaderText = (hadError) ? "" : "\n______________________________________";
                GUILayout.Label(changelog[0] + bottomHeaderText, HeadingTextStyle);

                //only center the text if we had an error and need to display an error message
                if (hadError)
                {
                    u2dexGUIUtil.EndCenter();
                }

                scrollPos = GUILayout.BeginScrollView(scrollPos);
                GUILayout.BeginVertical();

                //start on 1, since we've done the heading already...
                for (int i = 1; i < changelog.Length; i++)
                {
                    //only center the text if we had an error and need to display an error message
                    if (hadError)
                    {
                        u2dexGUIUtil.StartCenter();
                    }
                    //since we're not on first line, don't use a special font.
                    GUILayout.Label(changelog[i], DefaultTextStyle);
                    //add a little space between lines...
                    GUILayout.Space(10);

                    //only center the text if we had an error and need to display an error message
                    if (hadError)
                    {
                        u2dexGUIUtil.EndCenter();
                    }
                }

                GUILayout.EndVertical();
                GUILayout.EndScrollView();

                //Add a button to close the window, if there was no error.
                if (!hadError)
                {
                    u2dexGUIUtil.StartCenter();
                    if (GUILayout.Button(new GUIContent("Close", "Close this window"), GUILayout.Width(100)))
                    {
                        this.Close();
                    }
                    u2dexGUIUtil.EndCenter();
                }
            }
            else
            {
                u2dexGUIUtil.StartCenter();

                GUILayout.Label("Loading the Changelog...");

                u2dexGUIUtil.EndCenter();
            }
        }

        private string[] FormatString(string Changelog)
        {
            return Changelog.Split(new string[] { "\n", "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);
        }
    }
    //only use namespaces if we aren't using unity 3.5 (only 4.0 or greater)
#if !UNITY_3_5
}
#endif