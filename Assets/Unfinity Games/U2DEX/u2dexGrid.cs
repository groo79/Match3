using UnityEngine;
using System.Collections;

namespace UnfinityGames.U2DEX
{
	/// <summary>
	/// The Grid component that's used on a camera to visualize a 2D grid.
	/// </summary>
	[ExecuteInEditMode]
	[AddComponentMenu("Unfinity Games/U2DEX/Camera/U2DEX Grid")]
	public class u2dexGrid : MonoBehaviour
	{
		public Vector2 gridSize = new Vector2(64, 64);
		public Color color = Color.white;
		public GridScale gridScale = GridScale.NearlyUnlimited;

		private float lineLength; //The length of our grid lines.
		private const float frustumHeight = 100000; //The height of our "frustum".
		private float maximumDistance; //The maximum drawing distance for our grid.

		//Note: we assign integers to our GridScale types so we can directly cast them to integers later.
		//The integers assigned correlate to the draw distance of the grid, in world units.
		public enum GridScale
		{
			NearlyUnlimited = 100000,
			Tiny = 250,
			Small = 500,
			Medium = 1000,
			Large = 2000,
			ExtraLarge = 5000,
			Massive = 10000
		}		

		void OnDrawGizmos()
		{
			var originalColor = Gizmos.color; //Cache the current gizmo color.
			Gizmos.color = color; //Set the gizmo color to our grid's color.

			//Some objects to hold our Scene Camera.
			GameObject sceneCamObj = GameObject.Find("SceneCamera");
			Camera sceneCamera = new Camera();
			
			//The position, or origin, if you will, of our grid in 3D space.
			Vector3 position = Vector3.zero;

			//A local variable that lets us calculate the number of lines based off of width.
			var totalLineWidth = 0f;

			//Calculate and store our largest grid axis, for use below.
			float largestGridSizeAxis = 0;
			if (gridSize.x > largestGridSizeAxis)
				largestGridSizeAxis = gridSize.x;

			if (gridSize.y > largestGridSizeAxis)
				largestGridSizeAxis = gridSize.y;

			//Set our lineLength to our faked frustumHeight.  Note, the height is "faked" in the sense that
			//it's actually a "magic number" that works well for both the perspective and orthographic camera.
			lineLength = frustumHeight;

			//Set our maximum drawing distance to the rounded integer of our gridScale and gridSize.
			maximumDistance = RoundInteger((int)gridScale, largestGridSizeAxis + 0.25f);

			//If we successfully found our Scene Camera...
			if (sceneCamObj != null)
			{
				//Get our camera component, and then cache its position.
				sceneCamera = sceneCamObj.GetComponent<Camera>();
				position = sceneCamera.transform.position;

				//Then, work out our draw distance based off of our faked frustrumHeight, as well as the camera's 
				//field of view.
				var distance = frustumHeight * 0.5f / Mathf.Tan(sceneCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);

				//Clamp our distance
				distance = Mathf.Clamp(distance, 1, maximumDistance);

				//Set our totalLineWidth to our calculated distance, rounded to the closest number based on grid axis.
				totalLineWidth = Round(distance, largestGridSizeAxis + 0.25f);

				//Set our line length to our number of lines, thus giving us a square grid.
				lineLength = totalLineWidth;
			}

			//We currently don't use an offset, but we'll leave it here as it "completes" our formula.
			Vector2 offset = Vector2.zero;

			//Snap our grid's position to positions that line up with our gridSize.
			position = SnapToGridSize(position);

			//Work out our number of lines by dividing our totalLineWidth by our gridSize.x.
			float numberOfLines = totalLineWidth / gridSize.x;

			//Draw horizontal lines
			for (float y = -numberOfLines + 1; y < numberOfLines + 1; y++)
			{
				//Figure out our current line's position, based on the position, gridSize and y (which acts as i).
				var yPos = position.y + (gridSize.y * y);

				Gizmos.DrawLine(new Vector3(position.x - lineLength,
											Mathf.Floor(yPos / gridSize.y) * gridSize.y + offset.y,
											0.0f),
								new Vector3(position.x + lineLength,
											Mathf.Floor(yPos / gridSize.y) * gridSize.y + offset.y,
											0.0f));
			}

			//Draw vertical lines
			for (float x = -numberOfLines + 1; x < numberOfLines + 1; x++)
			{
				//Figure out our current line's position, based on the position, gridSize and x (which acts as i).
				var xPos = position.x + (gridSize.x * x);

				Gizmos.DrawLine(new Vector3(Mathf.Floor(xPos / gridSize.x) * gridSize.x + offset.x,
											position.y - lineLength,
											0.0f),
								new Vector3(Mathf.Floor(xPos / gridSize.x) * gridSize.x + offset.x,
											position.y + lineLength,
											0.0f));
			}

			//We're all done here, set the gizmo color back to what it was before we changed it.
			Gizmos.color = originalColor;
		}	

		/// <summary>
		/// Rounds an integer to be a multiple of the specified multipleOf.
		/// </summary>
		private int RoundInteger(int integer, float multipleOf)
		{
			if (integer >= 0)
				return Mathf.FloorToInt(((integer + multipleOf / 2) / multipleOf) * multipleOf);
			else
				return Mathf.CeilToInt(((integer - multipleOf / 2) / multipleOf) * multipleOf);
		}

		//The below methods are actually from our AdvancedSnapping tool, but since we can't easily access it, they have
		//to be copied (and they're slightly modified, anyway).

		/// <summary>
		/// The snapping method.  Handles applying the Rounding method to all of the selected objects.
		/// </summary>
		private Vector3 SnapToGridSize(Vector3 position)
		{
			var pos = position;
			pos.x = Round(pos.x, gridSize.x);
			pos.y = Round(pos.y, gridSize.y);
			//pos.z = Round(pos.z);//we don't need to round the Z plane, since in 2D it's used for Z depth
			return FixIfNaN(pos);
		}

		/// <summary>
		/// Actually handles rounding the positions to the designated snapping amount.
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		private float Round(float input, float SnapTo)
		{
			return SnapTo * Mathf.Round((input / SnapTo));
		}

		/// <summary>
		/// Checks each float in a Vector3, and fixes it if it's NaN (Not a Number)
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		private Vector3 FixIfNaN(Vector3 v)
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
