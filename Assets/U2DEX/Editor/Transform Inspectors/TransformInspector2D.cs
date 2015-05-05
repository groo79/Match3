using UnityEngine;
using UnityEditor;
using System.Collections;

namespace u2dex
{
    /// <summary>
    /// The base class that ALL officially supported 2D Transform Inspectors inherit from.
    /// Contains all the methods for drawing various GUI fields.
    /// </summary>
    public class TransformInspector2D
    {
        int layer = 0;
        bool setLayer = false;

        //There are only 31 layers in Unity, and 7 of them are reserved for the Engine.
        //We add one more since we'll add Default (first entry in the list) to our array.
        string[] FilteredLayerNames = new string[25];

        public Vector3 EulerAngles = Vector3.zero;

        /// <summary>
        /// Draws the snapping tool foldout.
        /// </summary>
        /// <param name="t"></param>
        public void DrawSnappingFoldout(Transform t)
        {
            //if we're on a supported version of Unity that's older than 4.3
#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2
            EditorGUIUtility.LookLikeControls();
#endif
            EditorGUI.indentLevel = 0;

            //Add our toggle+foldout block for snapping.
            EditorGUILayout.BeginHorizontal(GUILayout.Width(15));

            //we have to use a variable here, since we can't pass the EnableSnapping directly into a ref argument.
            var useSnapping = GlobalSnappingData.EnableSnapping;
            //if snapping is enabled, enable our advanced snapping options.
            GlobalSnappingData.SnappingFoldoutOpen = u2dexGUIUtil.FoldoutToggle(GlobalSnappingData.SnappingFoldoutOpen,
                                  ref useSnapping,
                                  new GUIContent("Enable Snapping", "Enable or disable snapping on the selected object."));//, snappingStyle);

            GlobalSnappingData.EnableSnapping = useSnapping;

            //Check for closing everything down if the toggle button is clicked...
            if (GUI.changed)
            {
                if (!GlobalSnappingData.EnableSnapping && GlobalSnappingData.SnappingFoldoutOpen)
                {
                    GlobalSnappingData.SnappingFoldoutOpen = false;
                }
            }

            //if the GUI changed, and the snapping foldout is open, enable snapping
            if (GUI.changed)
            {
                if (GlobalSnappingData.SnappingFoldoutOpen)
                {
                    GlobalSnappingData.EnableSnapping = true;
                }
            }

            EditorGUILayout.EndHorizontal();

            //if the GUI changed, and snapping was disabled, close the advanced foldout
            if (GUI.changed)
            {
                if (!GlobalSnappingData.EnableSnapping)
                {
                    GlobalSnappingData.SnappingFoldoutOpen = false;
                }
            }

            //if the foldout is open, show our advanced options.
            if (GlobalSnappingData.SnappingFoldoutOpen)
            {
                //Indent the following line...
                //if we're on a supported version of Unity that's older than 4.3
#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2
                EditorGUI.indentLevel = 2;
#else
                EditorGUI.indentLevel = 1;
#endif
                AdvancedSnapping.DrawGUI();

            }

            //Reset the indentation
            EditorGUI.indentLevel = 0;
        }

        /// <summary>
        /// Draws the rotation controls (degrees, radians, etc.)
        /// </summary>
        /// <param name="t"></param>
        public void DrawRotationControls(Transform t)
        {
            //Label the Rotation area
            EditorGUILayout.LabelField(new GUIContent("Rotation"));
            //Indent the following line...
            //if we're on a supported version of Unity that's older than 4.3
#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2
                EditorGUI.indentLevel = 2;
#else
            EditorGUI.indentLevel = 1;
#endif

            //Rotation will only be on the Z plane, so we can safely hide the X and Y planes from view.
            //Use local euler angles for rotation (since they go 0-360, essentially degrees for our purposes)
            var eulerAngles = t.localEulerAngles;
            //convert the euler angles (degrees) into radians.
            var displayRotation_Radians = eulerAngles.z * Mathf.Deg2Rad;

            //Also, we can set the rotation in Degrees, if we're so inclined.
            var displayRotation_Degrees = eulerAngles.z;

            displayRotation_Radians = EditorGUILayout.FloatField("Radians", displayRotation_Radians);
            displayRotation_Degrees = EditorGUILayout.FloatField("Degrees", displayRotation_Degrees);

            //check if the GUI changed
            if (GUI.changed)
            {
                //determine what changed...
                //Check degrees first
                if (displayRotation_Degrees != eulerAngles.z)
                {
                    //sync radians
                    displayRotation_Radians = displayRotation_Degrees / Mathf.Deg2Rad;
                }
                //then check radians if the degrees didn't change
                else if (displayRotation_Radians != eulerAngles.z * Mathf.Deg2Rad)
                {
                    //sync degrees
                    displayRotation_Degrees = displayRotation_Radians * Mathf.Rad2Deg;
                }


            }
            //apply our changes!
            eulerAngles.z = displayRotation_Degrees;

            //copy the local eulerAngles to our Public, inheritable EulerAngles
            EulerAngles = eulerAngles;

            //Reset indentation
            EditorGUI.indentLevel = 0;
        }

        /// <summary>
        /// Draws the sorted layer list, and the Z Depth control.
        /// </summary>
        /// <param name="t"></param>
        public void DrawLayerAndDepthControls(Transform t)
        {
            //Label the Layer and Depth area
            EditorGUILayout.LabelField(new GUIContent("Layer and Depth"));
            //if we're on a supported version of Unity that's older than 4.3
#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2
                EditorGUI.indentLevel = 2;
#else
            EditorGUI.indentLevel = 1;
#endif

            //allow the layer to be set.
            FilteredLayerNames = new string[25];
            FilteredLayerNames[0] = LayerMask.LayerToName(0);//this should be layer "Default"

            //i starts at 1, since 0 is already "Default"
            var emptyEntries = 0;
            for (int i = 1; i < FilteredLayerNames.Length; i++)
            {
                //only set the string if it contains something other than empty space.
                if (LayerMask.LayerToName(i + 7) != string.Empty)
                {
                    FilteredLayerNames[i] = LayerMask.LayerToName(i + 7);
                }
                else
                {
                    emptyEntries++;
                }
            }

            //Need to filter out dead space in the array and trim it for displaying
            if (emptyEntries != 0)
            {
                //create the new filtered array by subtracting the number of empty entries from the total size of the array
                //add one to the total since we do < in the for loop -- otherwise the last entry will always be cut off.
                var RefilteredLayerNames = new string[25 - emptyEntries];
                RefilteredLayerNames[0] = LayerMask.LayerToName(0);//this should be layer "Default"

                //j will be used to track where we are in the newly refiltered array
                var j = 1;
                //iterate through the entirety of the old array
                for (int i = 1; i < FilteredLayerNames.Length; i++)
                {
                    //only set the string if it contains something other than empty space.
                    if (LayerMask.LayerToName(i + 7) != string.Empty)
                    {
                        //add this entry and increment the tracker, since this one has text in it.
                        //the user must have tagged this layer.
                        RefilteredLayerNames[j] = LayerMask.LayerToName(i + 7);
                        j++;
                    }
                }

                //copy the refiltered array back to our original filtered array
                FilteredLayerNames = RefilteredLayerNames;
            }

            //if we've already set the layer once (i.e. if it's not 0 and the user changed it)
            if (setLayer)
            {
                layer = EditorGUILayout.Popup("Layer", layer, FilteredLayerNames);
            }
            else if (layer == 0)
            {
                // the user HASN'T set the layer (but it's set to something other than zero), so let's show them the layer it's currently set as
                //Get the name of the assigned layer in the rooted gameobject
                var nameOfLayer = LayerMask.LayerToName(t.gameObject.layer);

                //loop through our filtered names and try to find a match
                for (int i = 0; i < FilteredLayerNames.Length; i++)
                {
                    //find the index of the layer that matches
                    if (FilteredLayerNames[i] == nameOfLayer)
                    {
                        //found a match!  Copy this into our layer variable for displaying.
                        layer = i;
                    }
                    else
                    {
                        //if we're at the end and still haven't found a match.
                        if (i == FilteredLayerNames.Length)
                        {
                            Debug.LogError(u2dexInfo.AbbreviatedName + ": Uh oh!  There's been an error trying to display filtered layer names!");
                        }
                    }
                }
                layer = EditorGUILayout.Popup("Layer", layer, FilteredLayerNames);
                setLayer = true;
            }
        }

        /// <summary>
        /// A public method to get the internal (and inheritable) sorted layer list.
        /// </summary>
        /// <returns></returns>
        public string GetSortedLayer()
        {
            return FilteredLayerNames[layer];
        }
    }
}
