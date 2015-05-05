using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;

//only use namespaces if we aren't using unity 3.5 (only 4.0 or greater)
#if !UNITY_3_5
namespace u2dex
{
#endif

    /// <summary>
    /// A container for all of the data that we need to save (settings, etc.)
    /// </summary>
    //[Serializable]
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
            u2dexEditorPrefs.SetInt("u2dex_versionNumber", versionNumber);

            u2dexEditorPrefs.SetBool("u2dex_transformInspectorEnabled", transformInspectorEnabled);

            u2dexEditorPrefs.SetBool("u2dex_enableSnapping", enableSnapping);

            u2dexEditorPrefs.SetBool("u2dex_foldoutOpen", foldoutOpen);

            u2dexEditorPrefs.SetFloat("u2dex_amountToSnapTo", amountToSnapTo);

            u2dexEditorPrefs.SetBool("u2dex_foldoutOpen", snapToGrid);

            u2dexEditorPrefs.SetStringArray("u2dex_applicableClasses", applicableClasses);

            u2dexEditorPrefs.SetVector2("u2dex_gridSize", gridSize);

            u2dexEditorPrefs.SetColor("u2dex_gridColor", gridColor);

            u2dexEditorPrefs.SetBool("u2dex_usePixelsPerMeter", usePixelsPerMeter);

            u2dexEditorPrefs.SetFloat("u2dex_pixelsPerMeter", pixelsPerMeter);

            u2dexEditorPrefs.SetBool("u2dex_useNGUIInspector", useNGUIInspector);
        }

        public void Load(string[] DefaultClasses)
        {
            versionNumber = u2dexEditorPrefs.GetInt("u2dex_versionNumber", 0);

            transformInspectorEnabled = u2dexEditorPrefs.GetBool("u2dex_transformInspectorEnabled", true);

            enableSnapping = u2dexEditorPrefs.GetBool("u2dex_enableSnapping", false);

            foldoutOpen = u2dexEditorPrefs.GetBool("u2dex_foldoutOpen", false);

            amountToSnapTo = u2dexEditorPrefs.GetFloat("u2dex_amountToSnapTo", 32);

            snapToGrid = u2dexEditorPrefs.GetBool("u2dex_foldoutOpen", false);

            //If we have a key and the array has the greater than or equal classes in it (so we always have the defaults)
            if (EditorPrefs.HasKey("u2dex_applicableClasses") 
                && u2dexEditorPrefs.GetStringArray("u2dex_applicableClasses").Length >= DefaultClasses.Length)
            {
                applicableClasses = u2dexEditorPrefs.GetStringArray("u2dex_applicableClasses");
            }
            else //otherwise, initialize it with the defaults provided.
            {
                applicableClasses = DefaultClasses;
            }

            gridSize = u2dexEditorPrefs.GetVector2("u2dex_gridSize", new Vector2(64,64));

            gridColor = u2dexEditorPrefs.GetColor("u2dex_gridColor", Color.white);

            usePixelsPerMeter = u2dexEditorPrefs.GetBool("u2dex_usePixelsPerMeter", false);

            pixelsPerMeter = u2dexEditorPrefs.GetFloat("u2dex_pixelsPerMeter", 1);

            useNGUIInspector = u2dexEditorPrefs.GetBool("u2dex_useNGUIInspector", false);
        }
    }

//only use namespaces if we aren't using unity 3.5 (only 4.0 or greater)
#if !UNITY_3_5
}
#endif
