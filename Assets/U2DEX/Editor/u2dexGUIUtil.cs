using UnityEngine;
using UnityEditor;

namespace u2dex
{
    /// <summary>
    /// A GUI Utility class for 2D Transform Inspectors.
    /// </summary>
    static class u2dexGUIUtil
    {
        private static GUIStyle openFoldoutStyle;
        private static GUIStyle closedFoldoutStyle;
        private static GUIStyle openArrowStyle;
        private static GUIStyle closedArrowStyle;
        private static bool initted;

        //unity 3.5 doesn't have the font icons we want to use, so we need to use something else.
#if UNITY_3_5
        public static string CheckMark = "\u25A0";
        public static string XMark = "\u25A0";
#else
        public static string CheckMark = '\u2714'.ToString();
        public static string XMark = '\u2718'.ToString();
#endif

        private static void Init()
        {
            openFoldoutStyle = new GUIStyle();
            //openFoldoutStyle.fontStyle = (FontStyle)1;
            openFoldoutStyle.stretchHeight = true;

            openFoldoutStyle.padding = new RectOffset(5, 0, 4, 0);
            //openFoldoutStyle.fixedHeight = 1;

            closedFoldoutStyle = new GUIStyle(openFoldoutStyle);
            openFoldoutStyle.normal = openFoldoutStyle.onNormal;
            openFoldoutStyle.active = openFoldoutStyle.onActive;

            openArrowStyle = new GUIStyle(openFoldoutStyle);

            //if we're on a supported version of Unity that's older than 4.3
#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2
            openArrowStyle.padding = new RectOffset(3, 1, 3, 0);
#else
            //we have to totally doctor the padding for 4.3 forward since Unity changed how Editor GUI items
            //are spaced by default.  It's kind of messy, but by doing this we maintain the feature of the whole
            //foldout being clickable.
            openArrowStyle.padding = new RectOffset(-11, -11, 3, 0);
            openFoldoutStyle.padding = new RectOffset(openFoldoutStyle.padding.left + 11, openFoldoutStyle.padding.right,
                                                      openFoldoutStyle.padding.top, openFoldoutStyle.padding.bottom);
#endif

            openArrowStyle.fontSize = 12;
            openArrowStyle.normal.textColor = Color.grey;
            openArrowStyle.onNormal.textColor = Color.grey;
            openArrowStyle.hover.textColor = Color.grey;
            openArrowStyle.onHover.textColor = Color.grey;
            openArrowStyle.focused.textColor = Color.grey;
            openArrowStyle.onFocused.textColor = Color.grey;
            openArrowStyle.active.textColor = Color.grey;
            openArrowStyle.onActive.textColor = Color.grey;

            closedArrowStyle = new GUIStyle(openArrowStyle);

            //if we're on a supported version of Unity that's older than 4.3
#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2
            closedArrowStyle.padding = new RectOffset(3, 2, -2, 0);
#else
            //we have to totally doctor the padding for 4.3 forward since Unity changed how Editor GUI items
            //are spaced by default.  It's kind of messy, but by doing this we maintain the feature of the whole
            //foldout being clickable.
            closedArrowStyle.padding = new RectOffset(-11, -11, -2, 0);
            closedFoldoutStyle.padding = new RectOffset(closedFoldoutStyle.padding.left + 12, closedFoldoutStyle.padding.right,
                                                      closedFoldoutStyle.padding.top, closedFoldoutStyle.padding.bottom);
#endif
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
#if !UNITY_3_0 || !UNITY_3_1 || !UNITY_3_2 || !UNITY_3_3 || !UNITY_3_4 || !UNITY_3_5 || !UNITY_4_0 || !UNITY_4_0_1 || !UNITY_4_1 || !UNITY_4_2
            GUILayout.Space(2);
#endif
        }

        /// <summary>
        /// Adds some vertical space for our GUI, but uses less space on Unity 4.3 forward, to maintain a uniform look.
        /// </summary>
        public static void Unity3EditorGUILayoutSpace()
        {
#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2
            //Leave some vertical space between areas!
            EditorGUILayout.Space();
#else
                    Unity4Space();
#endif
        }
    }
}
