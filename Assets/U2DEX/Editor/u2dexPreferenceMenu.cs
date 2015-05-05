using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace u2dex
{
    public class u2dexPreferenceMenu
    {
        private static Vector2 _preferenceScrollPosition;  

        [PreferenceItem("U2DEX")]
        public static void OnPreferenceWindow()
        {
            OnPreferenceGUI();
        }

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

