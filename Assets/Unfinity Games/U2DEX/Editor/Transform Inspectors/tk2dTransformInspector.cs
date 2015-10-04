using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using System.Reflection;

using UnfinityGames.Common.Editor;

namespace UnfinityGames.U2DEX
{
	/// <summary>
	/// The officially supported 2D Transform Inspector for 2D Toolkit Sprites.
	/// </summary>
	public class tk2dTransformInspector : TransformInspector2D
	{
		public void DrawInspector(Transform target)
		{
			Transform t = (Transform)target;
			//if we're only editing 1 object, otherwise we need to display an error message since multi-object
			//editing isn't currently supported by this extension.
			if (Selection.transforms.Length < 2)
			{
				// Replicate the standard transform inspector gui if the component isn't part of 2DToolkit       
				if (t.gameObject.GetComponent("tk2dBaseSprite"))
				{
					var tk2dSpriteType = TransformInspectorUtility.GetType("tk2dBaseSprite");
					var tk2dSprite = t.gameObject.GetComponent("tk2dBaseSprite");

					//Get the sprite definition, so we can make sure it isn't null before preceding.
					MethodInfo tk2d_GetCurrentSpriteDef = tk2dSpriteType.GetMethod("GetCurrentSpriteDef", Type.EmptyTypes);
					object SpriteDef = tk2d_GetCurrentSpriteDef.Invoke(tk2dSprite, tk2d_GetCurrentSpriteDef.GetParameters());

					//Try to check if the 2DToolkit sprite object is valid
					if (tk2dSprite != null && SpriteDef != null)
					{
						DrawSnappingFoldout(t);

						UnfinityGUIUtil.Unity4Space();

						//We only need 2 vectors (X and Y) for 2DToolkit.  No need to show the Z value.
						Vector3 position = EditorGUILayout.Vector2Field("Position", new Vector2(t.localPosition.x, t.localPosition.y));

						UnfinityGUIUtil.Unity4Space();

						//Again, we only need X and Y for scale.
						Vector2 scale = EditorGUILayout.Vector2Field("Size",
							new Vector2(TransformInspectorUtility.GetScaleFromClassName("tk2dBaseSprite", "scale", t).x,
							TransformInspectorUtility.GetScaleFromClassName("tk2dBaseSprite", "scale", t).y));

						UnfinityGUIUtil.Unity4Space();

						DrawRotationControls(t);

						//Leave some vertical space between areas!
						UnfinityGUIUtil.Unity4Space();

						DrawLayerAndDepthControls(t);

						//allow the Z Depth to be set
						position.z = (float)EditorGUILayout.IntField("Z Depth", (int)t.localPosition.z);

						EditorGUI.indentLevel = 0;

						//Leave some vertical space between areas!
						EditorGUILayout.Space();

						if (GUI.changed)
						{
							Undo.RecordObject(t, "Transform Change");
							t.localPosition = this.FixIfNaN(position);
							t.localEulerAngles = this.FixIfNaN(EulerAngles);

							//Check if the scale is NaN
							var tk2DScale = new Vector3(scale.x, scale.y, 1);
							tk2DScale = this.FixIfNaN(tk2DScale);

							//Then copy it back
							//tk2dSprite.scale = new Vector2(tk2DScale.x, tk2DScale.y);
							TransformInspectorUtility.SetScaleFromClassName("tk2dBaseSprite", "scale", t, tk2DScale);

							//Retrieve the layer by name, and then set it.
							tk2dSprite.gameObject.layer = LayerMask.NameToLayer(GetSortedLayer());

							//copy our changed sprite back to our target.
							EditorUtility.SetDirty(tk2dSprite);
						}
					}
				}
			}
			else
			{
				EditorGUILayout.LabelField("Multi-object editing is not supported at this time.");
			}
		}

	}
}