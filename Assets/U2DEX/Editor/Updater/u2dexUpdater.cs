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

    public class u2dexUpdater : EditorWindow
    {
        bool haveCheckedForUpdate = false;

        class ReleaseData
        {
            public double version;
            public string changelog;
            public string changelogURL;
        }

        ReleaseData[] releases = null;
        ReleaseData release = null;

        string updateInfoUrl = "http://www.unfinitygames.com/u2dex/version_info.xml";
        WWW updateInfo = null;
        bool retrievingFile = false;

        //split these up into two string so each string can be centered.
        const string newUpdate = "There is a new update available!";
        const string newUpdate2 = "Please download it from the Asset Store.";

        const string hasCurrentUpdate = "There are no new updates available.";
        const string hasCurrentUpdate2 = "You've got the latest one!";

        const string hasFutureUpdate = "You have a version that's newer than the released version.";
        const string hasFutureUpdate2 = "Lucky you!";

        string startMessage = "Click the button to check for updates.";

        const string errorMessage = "Unable to check for updates.";
        const string errorMessage2 = "Try again later, or contact support if this issue persists.";

        public static string changelogError = "Error loading changelog.\n\nPlease close the updater and try again.";

        bool isNewUpdate = false;

        void CheckForUpdates()
        {
            //try to retrieve the version info file.
            try
            {
                if (!retrievingFile)
                {
                    startMessage = "";

                    //start retrieving the file...
                    //WWW files don't download immediately, and they don't block the main update thread, so we need
                    //to make sure that we wait until it's done downloading.  We check for this below.
                    updateInfo = new WWW(updateInfoUrl);
                }

                //once we're sure we have the file (and it's completely downloaded), proceed.
                if (updateInfo != null && updateInfo.isDone)
                {
                    startMessage = "";
                    retrievingFile = false;

                    EditorApplication.update -= CheckForUpdates;

                    //create a new XmlDocument...
                    XmlDocument xmlDocument = new XmlDocument();

                    //And fill it with the raw Xml text from our WWW file.
                    xmlDocument.LoadXml(updateInfo.text);

                    //Start searching nodes under updateinfo..
                    System.Xml.XmlNodeList releaseNodes = xmlDocument.SelectNodes("/updateinfo/release");
                    releases = new ReleaseData[releaseNodes.Count];

                    int currentNode = 0;
                    foreach (XmlNode node in releaseNodes)
                    {
                        ReleaseData releaseInfo = new ReleaseData();

                        try //try to retrieve the node...
                        {
                            //pull out our version number...
                            releaseInfo.version = double.Parse(node.Attributes["version"].Value, System.Globalization.NumberFormatInfo.InvariantInfo);
                        }
                        catch //if we can't retrieve the version node, something horrible has happened.
                        {
                            //tell the user to contact support, since there's probably an issue with the XML file...
                            startMessage = "Could not parse version number.  Please contact support.";
                            releases[currentNode] = null;

                            //force the GUI to redraw, since our message should need changing.
                            GUI.changed = true;
                            Repaint();
                        }
                        try //try to retrieve the node...
                        {
                            //and then pull out our changelog.
                            releaseInfo.changelog = node.Attributes["changelog"].Value;
                        }
                        catch { releaseInfo.changelog = ""; } //if we can't retrieve the node, set it to nothing

                        try //try to retrieve the node...
                        {
                            //and then pull out our changelog's URL.
                            releaseInfo.changelogURL = node.Attributes["changelogURL"].Value;
                        }
                        catch { releaseInfo.changelogURL = ""; } //if we can't retrieve the node, set it to nothing

                        releases[currentNode] = releaseInfo;

                        //increment the node...
                        currentNode++;
                    }

                    //set the release data that we use...
                    //We only use the most recent update, but we may allow for old version checking later, since we
                    //already have the data...
                    release = releases[0];

                    //if we have a new update, or if the user has a newer update (future update), disable the button.
                    if (release.version != u2dexInfo.version)
                    {
                        isNewUpdate = true;
                    }

                    //force the GUI to redraw, since our message should need changing.
                    GUI.changed = true;
                    Repaint();
                }
                else
                {
                    //Update the main message so the user knows something is happening.
                    startMessage = "Retrieving version information...";

                    //make sure we know we're retrieving the file.
                    retrievingFile = true;
                }
            }
            catch //if we can't retrieve it, let the user know that we couldn't retrieve it.
            {
                startMessage = "Error!";
                release = null;

                //force the GUI to redraw, since our message should need changing.
                GUI.changed = true;
                Repaint();
            }
        }

        void OnGUI()
        {
            //if we've checked for an update and our changelog is empty, disable our new changelog button.
            //OR if we've checked for an update, and the update isn't new, disable the button.
            if (haveCheckedForUpdate && (release != null && release.changelog == "")
                || (haveCheckedForUpdate && !isNewUpdate))
            {
                //if we got an update, disable the update button.
                GUI.enabled = false;
            }

            u2dexGUIUtil.StartCenter();
            //if we haven't checked for an update, have a button that checks for updates.
            if (!haveCheckedForUpdate || !isNewUpdate)
            {
                //if the button was pressed, or if we're retrieving the file.
                if (GUILayout.Button("Check for Updates", GUILayout.MaxWidth(150)))
                {
                    EditorApplication.update -= CheckForUpdates;
                    EditorApplication.update += CheckForUpdates;
                }
            }
            else // if we HAVE checked for updates, have a "view changelog" button.
            {
                string changelogText = (release != null && release.changelog == "") ? "Cannot view Changelog"
                                                                                    : "View Changelog";
                //if the button was pressed, or if we're retrieving the file.
                if (GUILayout.Button(changelogText, GUILayout.MaxWidth(150)))
                {
                    //if we don't have a changelog URL, don't present the user with an option to view on the internet.
                    if (release != null && release.changelogURL == "")
                    {
                        OpenChangeLogViewer();
                    }
                    else //if we DO have a changelog URL, let the user decide if they want to view it in Unity, or a browser
                    {
                        //in this case, true equals 'In Unity' and false equals 'On the internet'
                        int location = EditorUtility.DisplayDialogComplex("Changelog Location", 
                                    "How would you like to view the changelog?", "In Unity", "On the internet","Nevermind!");

                        //if it's the first button
                        if (location == 0)
                        {
                            OpenChangeLogViewer();
                        }
                        else if(location == 1) //else, if it's the second button
                        {
                            if (release != null)
                            {
                                Application.OpenURL(release.changelogURL);
                            }
                            else
                            {
                                EditorUtility.DisplayDialog("Cannot open URL",
                                    "There was an error opening the changelog URL.  Please close the updater and try again.", "OK");
                            }
                        }

                        //if it was the third button, we don't really care, since the dialog will close on its own.
                    }
                                                            
                }
            }
            u2dexGUIUtil.EndCenter();

            if (haveCheckedForUpdate)
            {
                //if we got an update, re-enable the GUI for the text labels.
                GUI.enabled = true;
            }

            //if we have an error message, display it.
            if (startMessage != "")
            {
                if (startMessage != "Error!")
                {
                    u2dexGUIUtil.StartCenter();
                    GUILayout.Label(startMessage);
                    u2dexGUIUtil.EndCenter();
                }
                else
                {
                    GUILayout.BeginVertical();

                    u2dexGUIUtil.StartCenter();
                    GUILayout.Label(errorMessage);
                    u2dexGUIUtil.EndCenter();

                    u2dexGUIUtil.StartCenter();
                    GUILayout.Label(errorMessage2);
                    u2dexGUIUtil.EndCenter();

                    GUILayout.EndVertical();
                }
            }
            else// otherwise, continue.
            {
                //if we have a release...
                if (release != null)
                {
                    haveCheckedForUpdate = true;
                    if (release.version < u2dexInfo.version)
                    {
                        GUILayout.BeginVertical();

                        u2dexGUIUtil.StartCenter();
                        GUILayout.Label(hasFutureUpdate);
                        u2dexGUIUtil.EndCenter();

                        u2dexGUIUtil.StartCenter();
                        GUILayout.Label(hasFutureUpdate2);
                        u2dexGUIUtil.EndCenter();

                        GUILayout.EndVertical();
                    }
                    else
                    {
                        if (release.version == u2dexInfo.version)
                        {
                            GUILayout.BeginVertical();

                            u2dexGUIUtil.StartCenter();
                            GUILayout.Label(hasCurrentUpdate);
                            u2dexGUIUtil.EndCenter();

                            u2dexGUIUtil.StartCenter();
                            GUILayout.Label(hasCurrentUpdate2);
                            u2dexGUIUtil.EndCenter();

                            GUILayout.EndVertical();
                        }
                        else
                        {
                            if (release.version > u2dexInfo.version)
                            {
                                GUILayout.BeginVertical();

                                u2dexGUIUtil.StartCenter();
                                GUILayout.Label(newUpdate);
                                u2dexGUIUtil.EndCenter();

                                u2dexGUIUtil.StartCenter();
                                GUILayout.Label(newUpdate2);
                                u2dexGUIUtil.EndCenter();

                                GUILayout.EndVertical();
                            }
                        }
                    }
                }
            }
        }

        void OpenChangeLogViewer()
        {
            var changelog_Window = EditorWindow.GetWindow(typeof(u2dexChangelogViewer),
                            true, u2dexInfo.FullName + ": Changelog Viewer");

            if (release != null)
            {
                (changelog_Window as u2dexChangelogViewer).SetString(release.changelog);
            }
            else
            {
                (changelog_Window as u2dexChangelogViewer).SetString(changelogError);
            }

            changelog_Window.minSize = new Vector2(425, (changelog_Window as u2dexChangelogViewer).height);
            changelog_Window.maxSize = new Vector2(125, (changelog_Window as u2dexChangelogViewer).height);

            //position the new window near the parent window...
            changelog_Window.position = new Rect(EditorWindow.GetWindow(typeof(u2dexUpdater)).position.xMin + 50,
                                                 EditorWindow.GetWindow(typeof(u2dexUpdater)).position.yMin + 50,
                                                 changelog_Window.minSize.x,
                                                 changelog_Window.minSize.y);
            changelog_Window.Focus();
        }


        //[MenuItem(u2dexMenu.Root + "Check for Updates", false, 20100)]
        public static void ShowUpdater()
        {
            var update_Window = EditorWindow.GetWindow(typeof(u2dexUpdater), true, u2dexInfo.FullName + ": Updater");
            update_Window.minSize = new Vector2(350, 75);
            update_Window.maxSize = new Vector2(350, 75);
        }

    }
//only use namespaces if we aren't using unity 3.5 (only 4.0 or greater)
#if !UNITY_3_5
}
#endif
