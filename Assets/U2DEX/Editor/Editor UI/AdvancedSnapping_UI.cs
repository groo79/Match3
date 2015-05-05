using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;
using System;

//if we're using unity 3.5, we need this reference.
#if UNITY_3_5
using u2dex;
#endif

//only use namespaces if we aren't using unity 3.5 (only 4.0 or greater)
#if !UNITY_3_5
namespace u2dex
{
#endif

    public static class AdvancedSnapping_UI
    {
        static GUIStyle labelStyle;
        static GUIStyle verifiedTextStyle;
        static GUIStyle unverifiedTextStyle;

        static Vector2 scrollPosition;

        static bool hide = false;

        /// <summary>
        /// Initializes the Snapping window.
        /// </summary>
        public static void Init()
        {

            verifiedTextStyle = new GUIStyle(EditorStyles.label);
            unverifiedTextStyle = new GUIStyle(EditorStyles.label);

            verifiedTextStyle.padding.left = 7;

            verifiedTextStyle.fontSize = 12;
            verifiedTextStyle.normal.textColor = Color.green;
            verifiedTextStyle.onNormal.textColor = Color.green;
            verifiedTextStyle.hover.textColor = Color.green;
            verifiedTextStyle.onHover.textColor = Color.green;
            verifiedTextStyle.focused.textColor = Color.green;
            verifiedTextStyle.onFocused.textColor = Color.green;
            verifiedTextStyle.active.textColor = Color.green;
            verifiedTextStyle.onActive.textColor = Color.green;

            unverifiedTextStyle.padding.left = 7;

            unverifiedTextStyle.fontSize = 12;
            unverifiedTextStyle.normal.textColor = Color.red;
            unverifiedTextStyle.onNormal.textColor = Color.red;
            unverifiedTextStyle.hover.textColor = Color.red;
            unverifiedTextStyle.onHover.textColor = Color.red;
            unverifiedTextStyle.focused.textColor = Color.red;
            unverifiedTextStyle.onFocused.textColor = Color.red;
            unverifiedTextStyle.active.textColor = Color.red;
            unverifiedTextStyle.onActive.textColor = Color.red;
        }

        /// <summary>
        /// The DrawGUI function for this Editor Window.  
        /// Exposes the private GUI functions, draws the GUI and initializes everything if needed.
        /// </summary>
        public static void DrawGUI(bool DrawTitles)
        {
            Init();
            OnGUI(DrawTitles);
        }

        /// <summary>
        /// The OnGUI function for this Editor Window.  Handles all the GUI drawing.
        /// </summary>
        static void OnGUI(bool DrawTitles)
        {
            labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.padding.top = 0;
            labelStyle.padding.left = 0;

            if(DrawTitles)
                GUILayout.Label("Grid Preferences", EditorStyles.boldLabel, new GUILayoutOption[0]);

            DrawGridOptions(340);

            if (DrawTitles)
                GUILayout.Label("Snapping Preferences", EditorStyles.boldLabel, new GUILayoutOption[0]);

            DrawSnapOptions(340);

            GUILayout.Space(10);

            if (DrawTitles)
                GUILayout.Label("2D Transform Inspector", EditorStyles.boldLabel, new GUILayoutOption[0]);

            DisplayPluginToggle();
            DrawApplicableClassesOptions(345);

            //Only draw this if we've found NGUI, and it's our modified NGUITransformInspector
            if (TransformInspectorUtility.GetType("NGUITransformInspector") != null
                && TransformInspectorUtility.GetType("NGUITransformInspector").GetMethod("DrawInspector") != null)
            {
                GUILayout.Space(10);

                if (DrawTitles)
                    GUILayout.Label("Third-party Compatibility and Support", EditorStyles.boldLabel, new GUILayoutOption[0]);

                    DisplayThirdPartySupport();
            }

            //GUILayout.Space(10); //We don't need a space here -- it's already 'added' by the bottom of the ScrollView

            if (DrawTitles)
                GUILayout.Label("Help and Updates", EditorStyles.boldLabel, new GUILayoutOption[0]);

            DisplayHelpAndUpdates(340);

            GUILayout.Space(10);                       
        }

        static void DrawGridOptions(int Width)
        {
            u2dexGridInspector.DrawGUI(false);

            GlobalSnappingData.UsePixelsPerMeter = EditorGUILayout.Toggle(
                new GUIContent("Use Pixels Per Meter",
                    "Whether the Grid component should use Pixels Per Meter in its calculations."),
                    GlobalSnappingData.UsePixelsPerMeter);
            
            if (GlobalSnappingData.UsePixelsPerMeter)
            {
                GlobalSnappingData.PixelsPerMeter = Mathf.Max(0.001f, EditorGUILayout.FloatField(
                new GUIContent("Pixels Per Meter",
                    "The number of Pixels Per Meter you use in your project.  If you're unsure, it's better to leave 'Pixels Per Meter' unchecked."),
                    GlobalSnappingData.PixelsPerMeter, GUILayout.Width(Width)));
            }

            //if something in here was changed, refresh the SceneView to show the new grid.
            if (GUI.changed)
            {
                SceneView.RepaintAll();
            }
        }

        static void DrawSnapOptions(int Width)
        {
            GlobalSnappingData.EnableSnapping = EditorGUILayout.Toggle(
                new GUIContent("Enable Snapping",
                    "Whether snapping should default to enabled or disabled on all objects."),
                    GlobalSnappingData.EnableSnapping);

            //Set whether this is enabled or not, depending on what SnapToGrid is set to.
            //If SnapToGrid is off, this will be enabled, etc.
            GUI.enabled = !GlobalSnappingData.SnapToGrid;

            GlobalSnappingData.AmountToSnapTo = EditorGUILayout.FloatField(
                new GUIContent("Snap Amount",
                    "The amount of units objects will try to snap to by default.  Can be a float or integer."),
                    GlobalSnappingData.AmountToSnapTo, GUILayout.Width(Width));

            //Set this back to True for everything else.
            GUI.enabled = true;

            //Only display auto-snap to grid if we have at least one grid component active...
            if (GlobalSnappingData.HaveAtLeastOneGrid)
            {
                GlobalSnappingData.SnapToGrid = EditorGUILayout.Toggle(new GUIContent("Auto-Snap to Grid",
                                               "Automatically snaps objects to the grid."), GlobalSnappingData.SnapToGrid);
            }

            if (GlobalSnappingData.SnapToGrid)
            {
                GlobalSnappingData.AmountToSnapTo = GlobalSnappingData.GridSize.x / 2;//assume that grids are square...

                if (GlobalSnappingData.UsePixelsPerMeter)
                {
                    GlobalSnappingData.AmountToSnapTo = GlobalSnappingData.AmountToSnapTo / GlobalSnappingData.PixelsPerMeter;
                }
            }

            //if something in here was changed, refresh the SceneView to show the new grid.
            if (GUI.changed)
            {
                SceneView.RepaintAll();
            }
        }

        static void DrawApplicableClassesOptions(int Width)
        {
            // Use a Horizanal space or the toolbar will extend to the left no matter what
            EditorGUILayout.BeginHorizontal(GUILayout.Width(Width));

            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            if (GlobalSnappingData.ApplicableClasses.Count > 0)
            {
                GUIContent content;
                GUIStyle iconStyle;

                iconStyle = new GUIStyle(EditorStyles.toolbarButton);

                //use a different font size for each icon, since they're not the same size...
                if (hide)
                {
                    iconStyle.fontSize = 15;
                }
                else
                {
                    iconStyle.fontSize = 10;
                }

                var icon = (hide == true) ? "\u25B6" : "\u25BC";
                var tooltip = (hide == true) ? "Click to show all" : "Click to hide all";
                content = new GUIContent(icon, tooltip);

                //If the button if pressed, toggle the hide boolean.
                if (GUILayout.Button(content, iconStyle, GUILayout.Width(25)))
                {
                    //Hide equals the opposite of Hide...
                    hide = !hide;
                }
            }

            EditorGUILayout.LabelField("Applicable Classes", labelStyle);

            //Add the + button...
            EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(75));

            GUIStyle style = new GUIStyle(EditorStyles.toolbarButton);
            style.fontSize = 16;
            style.padding.bottom = 2;

            //Add button
            if (GUILayout.Button(new GUIContent("+", "Click to add a class"), style))
            {
                GlobalSnappingData.ApplicableClasses.Add("Give me a class name!");

                //if we're hiding the entries, unhide them since we just added one.
                if (hide)
                    hide = false;
            }

            //switch the font size and padding for this button...
            style.padding.bottom = 1;
            style.fontSize = 10;

            //Help button
            if (GUILayout.Button(new GUIContent("?", "Click to view help"), style, GUILayout.Width(23)))
            {
                //Open a new window instance, or focus on the old one, if we have one open.
                var help_Window = (AdvancedSnapping_HelpUI)EditorWindow.GetWindow(typeof(AdvancedSnapping_HelpUI),
                                                true, u2dexInfo.FullName + ": Snap Tool Help");

                /*Let's take a minute to reflect, and get the Preferences window...*/
                //Get the assembly...
                var assembly = System.Reflection.Assembly.GetAssembly(typeof(EditorWindow));
                
                //Get the type...
                var type = assembly.GetType("UnityEditor.PreferencesWindow");

                //Finally, get the preference window from the newly-reflected private type.
                var preference_Window = EditorWindow.GetWindow(type);

                //Now, use the preference window's position.
                Rect position = preference_Window.position;

                //Position the help window on top of the current window, in a cascading, slightly-offset manner...
                help_Window.position = new Rect(position.xMin + 200, position.yMin + 100,
                                                position.width, position.height);
                help_Window.Init();
                help_Window.minSize = new Vector2(500, 300);
                help_Window.maxSize = new Vector2(500, 300);

                //Put the help window in focus...
                help_Window.Focus();
            }

            //switch it back for any buttons after this...
            style.padding.bottom = 2;
            style.fontSize = 16;

            GUILayout.Space(1);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndHorizontal();

            //Keep these outside the !hide if, so the toggle button at the bottom stays at the bottom.
            EditorGUILayout.BeginVertical(GUILayout.Width(Width));

            //Start the ScrollView, but also cap it's height at between 75 and 100 (otherwise it takes up all the space it can...)
            //We also use fancy single-line evaluation statements to check if we're supposed to hide the list.  If we are
            //hiding the list, we remove the height settings (set to zero) so we don't take up a ton of empty room.
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition,
                GUILayout.MinHeight(hide ? 0 : 75), GUILayout.MaxHeight(hide ? 0 : 100));

            if (!hide)
            {
                //center the remove buttons...
                GUIStyle removeStyle = new GUIStyle(style);
                removeStyle.margin.top = 1;

                for (int i = 0; i < GlobalSnappingData.ApplicableClasses.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();

                    Type objectType = null;
                    if (GlobalSnappingData.ApplicableClasses[i] != string.Empty)
                    {
                        objectType = TransformInspectorUtility.GetType(GlobalSnappingData.ApplicableClasses[i]);
                        //Debug.Log(objectType); //BEWARE!  Massive performance hit!  Only use for debugging.
                    }

                    GUIContent verifyContent;
                    if (objectType != null)
                    {
                        verifyContent = new GUIContent(u2dexGUIUtil.CheckMark, "This class has been found");
                        EditorGUILayout.LabelField(verifyContent,
                                             verifiedTextStyle, GUILayout.Width(25));
                    }
                    else
                    {
                        verifyContent = new GUIContent(u2dexGUIUtil.XMark, "This class cannot be found");
                        EditorGUILayout.LabelField(verifyContent,
                                             unverifiedTextStyle, GUILayout.Width(25));
                    }

                    EditorGUILayout.LabelField(GetClassStringFromIndex(i),
                                                GUILayout.Width(100));

                    //Set the enabled field of the GUI to the opposite of whether the class is 
                    //officially supported or not.  Essentially, if it IS supported, disable the GUI, etc.
                    GUI.enabled = !IsSupportedClass(GlobalSnappingData.ApplicableClasses[i]);

                    GlobalSnappingData.ApplicableClasses[i] =
                            EditorGUILayout.TextField(GlobalSnappingData.ApplicableClasses[i]);

                    //Add the x (remove) button...
                    EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(20));

                    //Add button
                    if (GUILayout.Button(new GUIContent("×", "Click to remove this class"),
                        removeStyle))
                    {
                        GlobalSnappingData.ApplicableClasses.Remove(GlobalSnappingData.ApplicableClasses[i]);
                    }

                    GUILayout.Space(7);

                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.EndHorizontal();

                    //if the GUI was disabled, enable it here.
                    if (!GUI.enabled)
                    {
                        GUI.enabled = true;
                    }
                }
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        static void DisplayThirdPartySupport()
        {
            //Store the current state, before the user has a chance to toggle anything.
            bool previousToggleState = GlobalSnappingData.UseNGUIInspector;

            //Display the toggle for toggling the whole 2D Transform Inspector.
            GlobalSnappingData.UseNGUIInspector = EditorGUILayout.Toggle(
                new GUIContent("Use NGUI Transform Inspector",
                        "Whether the NGUI Transform Inspector should be used instead of the U2DEX Inspector."),
                        GlobalSnappingData.UseNGUIInspector);

            //If we're enabled right now, and we previously weren't, display the warning popup.
            if (GlobalSnappingData.UseNGUIInspector &&
                GlobalSnappingData.UseNGUIInspector != previousToggleState)
            {
                var result = EditorUtility.DisplayDialog("Override the " + u2dexInfo.AbbreviatedName + " Transform Inspector?",
                                                           "Enabling the NGUI Transform Inspector" +
                                                           " will override the U2DEX Transform Inspector.  This means" +
                                                           " you'll miss out on the extra features U2DEX provides." +
                                                           "\n\nAre you sure you want to do this?",
                                                           "Yes, override.", "Wait!  I changed my mind!");
                //React to the user's choices.
                if (result == true)
                {
                    GlobalSnappingData.UseNGUIInspector = true;
                }
                else
                {
                    GlobalSnappingData.UseNGUIInspector = false;
                }
            }
        }

        static void DisplayHelpAndUpdates(int Width)
        {
            GUIStyle bStyle_left = new GUIStyle(EditorStyles.miniButtonLeft);
            bStyle_left.fontSize = 12;
            bStyle_left.padding.bottom = 2;

            GUIStyle bStyle_center = new GUIStyle(EditorStyles.miniButtonMid);
            bStyle_center.fontSize = 12;
            bStyle_center.padding.bottom = 2;

            GUIStyle bStyle_right = new GUIStyle(EditorStyles.miniButtonRight);
            bStyle_right.fontSize = 12;
            bStyle_right.padding.bottom = 2;

            EditorGUILayout.BeginHorizontal(GUILayout.Width(Width));
            //Add button
            if (GUILayout.Button(new GUIContent("About", "Click to view information about U2DEX"), bStyle_left))
            {
                u2dexInfo.AboutU2DEX();
            }

            //Add button
            if (GUILayout.Button(new GUIContent("Check for updates", "Click to open the update utility"), bStyle_center))
            {
                u2dexUpdater.ShowUpdater();
            }

            //Add button
            if (GUILayout.Button(new GUIContent("Forum", "Click to go to our forum"), bStyle_right))
            {
                u2dexInfo.OpenForum();
            }

            EditorGUILayout.EndHorizontal();
        }

        static void DisplayPluginToggle()
        {
            //Store the current state, before the user has a chance to toggle anything.
            bool previousToggleState = GlobalSnappingData.TransformInspectorEnabled;

            //Display the toggle for toggling the whole 2D Transform Inspector.
            GlobalSnappingData.TransformInspectorEnabled = EditorGUILayout.Toggle(
                new GUIContent("Enable the " + u2dexInfo.AbbreviatedName + " Inspector",
                        "Whether the U2DEX Transform Inspector should be applied to applicable objects."),
                        GlobalSnappingData.TransformInspectorEnabled);

            //If we're disabled right now, and we previously weren't, display the warning popup.
            if (!GlobalSnappingData.TransformInspectorEnabled &&
                GlobalSnappingData.TransformInspectorEnabled != previousToggleState)
            {
                var result = EditorUtility.DisplayDialog("Disable " + u2dexInfo.AbbreviatedName + " Transform Inspector?",
                                                           "Disabling the " + u2dexInfo.AbbreviatedName + " Transform Inspector" +
                                                           " will revert all the Transform Inspectors on the applicable objects" +
                                                           " to the Unity defaults.\n\nAre you sure you want to disable it?",
                                                           "Yes, disable.", "Wait!  I changed my mind!");
                //React to the user's choices.
                if (result == true)
                {
                    GlobalSnappingData.TransformInspectorEnabled = false;
                }
                else
                {
                    GlobalSnappingData.TransformInspectorEnabled = true;
                }
            }
        }

        static bool IsSupportedClass(string Name)
        {
            //if we have officially supported types, then gray them out so they can't be deleted.
            foreach (string Class in GlobalSnappingData.SupportedClasses)
            {
                if (Name == Class)
                {
                    return true;
                }
            }

            //if we made it out of the loop without returning true, then there was no match.  Return false instead.

            //otherwise, do nothing special.  Just double-check that we're enabled back where we're called to begin with.
            return false;
        }

        static string GetClassStringFromIndex(int index)
        {
            switch (index)
            {
                //Note: This makes a lot of assumptions about the location of the various names in our array...
                //... Not exactly stellar, but it'll do in a pinch.
                case 0:
                    return "2D Toolkit Sprite";

                case 1:
                    return "Orthello Sprite";

                case 2:
                    return "Unity Sprite";

                default:
                    return "Class name " + ((index + 1) - GlobalSnappingData.SupportedClasses.Length);
                //we add i+1 since i starts at zero.  Then we subtract the number of supported classes. 
                //We want to start at 1.
            }
        }
    }

    //only use namespaces if we aren't using unity 3.5 (only 4.0 or greater)
#if !UNITY_3_5
}
#endif

