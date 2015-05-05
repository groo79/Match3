using UnityEngine;
using UnityEditor;
using System.Collections;

#if UNITY_3_5
using u2dex;
#endif

#if !UNITY_3_5
namespace u2dex
{
#endif
    /// <summary>
    /// The inspector editor for the grid component.
    /// </summary>
    [CustomEditor(typeof(u2dexGrid))]
    public class u2dexGridInspector : Editor
    {
        static u2dexGrid currentGrid;

        //static GUIStyle inspectorLabelStyle;
        static GUIStyle nonInspectorLabelStyle;

        public void OnEnable()
        {
            currentGrid = (u2dexGrid)target;            
        }

        public override void OnInspectorGUI()
        {
            //inspectorLabelStyle = new GUIStyle(GUI.skin.label);
            nonInspectorLabelStyle = new GUIStyle(GUI.skin.label);

            nonInspectorLabelStyle.margin = new RectOffset(20, -100, nonInspectorLabelStyle.margin.top,
                                                        nonInspectorLabelStyle.margin.bottom);
            nonInspectorLabelStyle.padding = new RectOffset(0, -100, nonInspectorLabelStyle.padding.top,
                                                        nonInspectorLabelStyle.padding.bottom);

            DrawGUI(true);
        }

        /// <summary>
        /// Handles all of the drawing for this Inspector.  Also allows us to externally call the rendering for our
        /// preference menu.
        /// </summary>
        /// <param name="isInspector">Whether the caller is an Inspector or not.</param>
        public static void DrawGUI(bool isInspector)
        {
            //Only draw this if we've got a grid...
            if (currentGrid != null)
            {
                currentGrid.gridSize = GlobalSnappingData.GridSize;

                if (GlobalSnappingData.UsePixelsPerMeter)
                {
                    currentGrid.gridSize = currentGrid.gridSize / GlobalSnappingData.PixelsPerMeter;
                }

                if (isInspector)
                {
                     currentGrid.gridSize = EditorGUILayout.Vector2Field("Grid Size", currentGrid.gridSize);
                }
                else
                {
                    GUILayout.BeginHorizontal();

                    GUILayout.Label("Grid Size");

                    GUILayout.Label("X", nonInspectorLabelStyle);

                    currentGrid.gridSize.x = EditorGUILayout.FloatField(currentGrid.gridSize.x,
                                                                            GUILayout.Width(55));

                    GUILayout.Label("Y", nonInspectorLabelStyle);

                    currentGrid.gridSize.y = EditorGUILayout.FloatField(currentGrid.gridSize.y,
                                                                            GUILayout.Width(55));
                    GUILayout.EndHorizontal();
                }
                GUILayout.BeginHorizontal();

                currentGrid.color = EditorGUILayout.ColorField("Grid Line Color", currentGrid.color);
                
                GUILayout.EndHorizontal();

                //repaint the scene with the grid.  (May not be needed, it just introduces a slight delay to the grid
                //updating if I don't have it)
                SceneView.RepaintAll();

                if (GUI.changed)
                {
                    currentGrid.gridSize = FixIfNaNOrZero(currentGrid.gridSize);
                }

                if (!GlobalSnappingData.UsePixelsPerMeter)
                {
                    GlobalSnappingData.GridSize = currentGrid.gridSize;
                }
                else
                {
                    //Return this value back to normal, now that we're done with it.
                    GlobalSnappingData.GridSize = currentGrid.gridSize * GlobalSnappingData.PixelsPerMeter;
                }
            }
            else //otherwise, display an error
            {
                EditorGUILayout.HelpBox("Cannot edit Grid Size and Color, as we couldn't find a suitable U2DEX Grid to edit.\n\n" + 
                                        "Try selecting the object in the Hierarchy tab that has the U2DEX Grid you want to edit, and then come back.", 
                                        MessageType.Error);
            }
        }

        /// <summary>
        /// Checks if a Vector2 is NaN (Not a Number) or zero, and fixes it accordingly.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        private static Vector2 FixIfNaNOrZero(Vector2 v)
        {
            if (float.IsNaN(v.x))
            {
                v.x = 0.001f; //grid can't be 0, since we'll by dividing by it later.
            }
            if (float.IsNaN(v.y))
            {
                v.y = 0.001f; //grid can't be 0, since we'll by dividing by it later.
            }

            if (v.x < 0.001f)
            {
                v.x = 0.001f;
            }
            if (v.y < 0.001f)
            {
                v.y = 0.001f;
            }

            return v;
        }
    }
#if !UNITY_3_5
}
#endif
