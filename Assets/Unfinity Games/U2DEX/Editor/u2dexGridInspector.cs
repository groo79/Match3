using UnityEngine;
using UnityEditor;
using System.Collections;

namespace UnfinityGames.U2DEX
{
	/// <summary>
	/// The inspector editor for the grid component.
	/// </summary>
	[CustomEditor(typeof(u2dexGrid))]
	public class u2dexGridInspector : Editor
	{
		private static u2dexGrid currentGrid;

		//static GUIStyle inspectorLabelStyle;
		private static GUIStyle nonInspectorLabelStyle;

		private static bool initialized = false;

		public void Init()
		{
			currentGrid = (u2dexGrid)target;

			//inspectorLabelStyle = new GUIStyle(GUI.skin.label);
			nonInspectorLabelStyle = new GUIStyle(EditorStyles.label);

			//If we're on a supported version before Unity 5, use Unity 4's spacing.  
#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6
			nonInspectorLabelStyle.margin = new RectOffset(20, -100, nonInspectorLabelStyle.margin.top,
														nonInspectorLabelStyle.margin.bottom);
			nonInspectorLabelStyle.padding = new RectOffset(0, -100, nonInspectorLabelStyle.padding.top,
														nonInspectorLabelStyle.padding.bottom);
#else //Otherwise, if we're on Unity 5 or above, use Unity 5's spacing.
			nonInspectorLabelStyle.margin = new RectOffset(0, -139, nonInspectorLabelStyle.margin.top,
														nonInspectorLabelStyle.margin.bottom);
			nonInspectorLabelStyle.padding = new RectOffset(0, -139, nonInspectorLabelStyle.padding.top,
														nonInspectorLabelStyle.padding.bottom);
#endif
			initialized = true;
		}

		public override void OnInspectorGUI()
		{
			//If we haven't initialized already, manually call our Init method 
			//(as long as we're in the GUI's Layout event, otherwise currentGrid may not match between Layout and Repaint)
			if (!initialized && Event.current.type == EventType.Layout)
				Init();

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

				//If we're using PPM, adjust our gridSize accordingly.
				if (GlobalSnappingData.UsePixelsPerMeter)
				{
					currentGrid.gridSize = currentGrid.gridSize / GlobalSnappingData.PixelsPerMeter;
				}

				//Cache our last grid size, now that we're done pre-editing it.
				var lastGridSize = currentGrid.gridSize;

				//If this is an inspector, and not the preference menu, render a normal Vector2 field.
				if (isInspector)
					currentGrid.gridSize = EditorGUILayout.Vector2Field(new GUIContent("Grid Size", 
																		"The size of the individual grid cells."),
																		currentGrid.gridSize);
				else //Otherwise, if this is the preferences menu, create a custom line that uses less horizontal space.
				{
					GUILayout.BeginHorizontal();

					GUILayout.Label(new GUIContent("Grid Size", "The size of the individual grid cells."));
					GUILayout.Label("X", nonInspectorLabelStyle);
					currentGrid.gridSize.x = EditorGUILayout.FloatField(currentGrid.gridSize.x,
																			GUILayout.Width(55));

					GUILayout.Label("Y", nonInspectorLabelStyle);
					currentGrid.gridSize.y = EditorGUILayout.FloatField(currentGrid.gridSize.y,
																			GUILayout.Width(55));
		
					GUILayout.EndHorizontal();
				}

				//Finally, render our grid color and grid scale fields.
				currentGrid.color = EditorGUILayout.ColorField(new GUIContent("Grid Line Color", 
																"The color of the grid lines."), 
																currentGrid.color);

				currentGrid.gridScale = (u2dexGrid.GridScale)EditorGUILayout.EnumPopup(new GUIContent("Grid Scale",
																			"The overall scale of the grid."),
																			currentGrid.gridScale);

				//repaint the scene with the grid.  (May not be needed, it just introduces a slight delay to the grid
				//updating if we don't have it)
				SceneView.RepaintAll();

				if (GUI.changed)
				{
					currentGrid.gridSize = FixIfNaNOrZero(currentGrid.gridSize);

					//If our gridSize was changed...
					if(currentGrid.gridSize != lastGridSize)
					{
						//Find the axis that was altered, and make sure that both axis match the changed one.
						//This enforces a square grid, which, honestly, should be the only grid type, as uneven
						//grids have too many problems for the end-user.
						if (currentGrid.gridSize.x != lastGridSize.x)
							currentGrid.gridSize.y = currentGrid.gridSize.x;

						if (currentGrid.gridSize.y != lastGridSize.y)
							currentGrid.gridSize.x = currentGrid.gridSize.y;

					}
				}

				//Set our global grid size to our current grid's size, now that we're done.
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

				//Set our initialized flag to false, so we can try to grab a grid the next time through.
				initialized = false;
			}
		}

		/// <summary>
		/// Checks if a Vector2 is NaN (Not a Number) or zero, and fixes it accordingly.
		/// This also ensures that the axis of the Vector2 aren't less than 1.
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		private static Vector2 FixIfNaNOrZero(Vector2 v)
		{
			if (float.IsNaN(v.x))
			{
				v.x = 1f; //grid can't be 0, since we'll by dividing by it later.
			}
			if (float.IsNaN(v.y))
			{
				v.y = 1f; //grid can't be 0, since we'll by dividing by it later.
			}

			if (v.x < 1f)
			{
				v.x = 1f;
			}
			if (v.y < 1f)
			{
				v.y = 1f;
			}

			return v;
		}
	}
}
