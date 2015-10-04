using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

using UnfinityGames.Common.EditorEvents;

namespace UnfinityGames.U2DEX
{
	//InitializeOnLoad sets this class up to be initialized automatically when the editor starts.
	[InitializeOnLoad]
	public static class AdvancedSnapping
	{
		private static Vector3 prevPosition;

		private static Vector3 scale;

		static AdvancedSnapping()
		{
			//This initializer is called automatically when the editor loads.

			//Hook the Initialize method into the EditorApplication's Update.
			//This ensures that the initialization and loading isn't done until the scene is fully loaded.
			EditorApplication.update += Initialize;            
		}

		private static void Initialize()
		{
			//Hook this class's update to the EditorApplication's update via a callback.

			//remove the method from the event, just in case this gets called more than once
			//(it shouldn't, but better safe than sorry!)  If it's not already assigned, the removal is ignored
			EditorApplication.update -= Update;
			EditorApplication.update += Update;

			//Load our settings data...
			GlobalSnappingData.Load();

			//remove the method from the event, just in case this gets called more than once
			//(it shouldn't, but better safe than sorry!)  If it's not already assigned, the removal is ignored
			EditorSavingEvent.OnEditorSave -= GlobalSnappingData.Save;
			EditorSavingEvent.OnEditorSave += GlobalSnappingData.Save;//hook into an event that's triggered when the Editor saves.

			//remove the method from the event, just in case this gets called more than once
			//(it shouldn't, but better safe than sorry!)  If it's not already assigned, the removal is ignored
			SaveOnPlayEvent.OnPlayModeStarted -= GlobalSnappingData.Save;
			SaveOnPlayEvent.OnPlayModeStarted += GlobalSnappingData.Save;//hook into an event that's triggered when the Editor saves.

			//Remove this method from the update queue
			EditorApplication.update -= Initialize;
		}

		/// <summary>
		/// Draw's the GUI for the Snapping Tool.
		/// </summary>
		public static void DrawGUI()
		{
			//Set whether this is enabled or not, depending on what SnapToGrid is set to.
			//If SnapToGrid is off, this will be enabled, etc.
			GUI.enabled = !GlobalSnappingData.SnapToGrid;

			GlobalSnappingData.AmountToSnapTo = EditorGUILayout.FloatField(new GUIContent("Snap Amount",
													"The amount of units objects will try to snap to."),
													GlobalSnappingData.AmountToSnapTo);
			//Set this back to True for everything else.
			GUI.enabled = true;

			//Check if we have at least one grid component in the scene...
			//Note: This function may be slow.  Might want to do some kind of delayed check.
			CheckForGrid();

			//Only display auto-snap to grid if we have at least one grid component active...
			if (GlobalSnappingData.HaveAtLeastOneGrid)
			{
				GlobalSnappingData.SnapToGrid = EditorGUILayout.Toggle(new GUIContent("Auto-Snap to Grid",
											   "Automatically snaps objects to the grid."), GlobalSnappingData.SnapToGrid);
			}

			if (GlobalSnappingData.SnapToGrid)
			{
				//Calculate and store our smallest grid axis, for use below.
				float smallestGridSizeAxis = float.MaxValue;
				if (GlobalSnappingData.GridSize.x < smallestGridSizeAxis)
					smallestGridSizeAxis = GlobalSnappingData.GridSize.x;

				if (GlobalSnappingData.GridSize.y < smallestGridSizeAxis)
					smallestGridSizeAxis = GlobalSnappingData.GridSize.y;

				GlobalSnappingData.AmountToSnapTo = smallestGridSizeAxis / 2; //Use our smallest grid axis for snapping.

				if (GlobalSnappingData.UsePixelsPerMeter)
				{
					GlobalSnappingData.AmountToSnapTo = GlobalSnappingData.AmountToSnapTo / GlobalSnappingData.PixelsPerMeter;
				}
			}
		}

		//Checks if we have a grid component in the scene.
		private static void CheckForGrid()
		{
			var grids = Object.FindObjectsOfType(typeof(u2dexGrid));

			if (grids == null || grids.Length == 0)
			{
				GlobalSnappingData.HaveAtLeastOneGrid = false;
			}
			else
			{
				if (grids.Length > 0)
				{
					GlobalSnappingData.HaveAtLeastOneGrid = true;
				}
			}
		}

		/// <summary>
		/// Updates the snapping tool.  Called automatically, as it's hooked into the EditorApplication's update.
		/// </summary>
		private static void Update()
		{
			if (GlobalSnappingData.EnableSnapping
			&& GlobalSnappingData.ApplicableObjectSelected
			&& Selection.transforms.Length > 0
			&& Selection.transforms[0].position != prevPosition)
			{
				SnapTo();
				prevPosition = Selection.transforms[0].position;
			}
		}

		/// <summary>
		/// The snapping method.  Handles applying the Rounding method to all of the selected objects.
		/// </summary>
		private static void SnapTo()
		{
			foreach (var transform in Selection.transforms)
			{
				var pos = transform.transform.position;
				pos.x = Round(pos.x);
				pos.y = Round(pos.y);
				//pos.z = Round(pos.z);//we don't need to round the Z plane, since in 2D it's used for Z depth
				transform.transform.position = FixIfNaN(pos);
			}
		}

		/// <summary>
		/// Actually handles rounding the positions to the designated snapping amount.
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		private static float Round(float input)
		{
			return GlobalSnappingData.AmountToSnapTo * Mathf.Round((input / GlobalSnappingData.AmountToSnapTo));
		}

		/// <summary>
		/// Checks each float in a Vector3, and fixes it if it's NaN (Not a Number)
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		private static Vector3 FixIfNaN(Vector3 v)
		{
			if (float.IsNaN(v.x))
			{
				v.x = 0;
			}
			if (float.IsNaN(v.y))
			{
				v.y = 0;
			}
			if (float.IsNaN(v.z))
			{
				v.z = 0;
			}
			return v;
		}
	}
}
