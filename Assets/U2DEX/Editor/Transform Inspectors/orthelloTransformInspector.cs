using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Reflection;
using System;

namespace u2dex
{
    /// <summary>
    /// The officially supported 2D Transform Inspector for Orthello sprites.
    /// </summary>
    public class orthelloTransformInspector : TransformInspector2D
    {
        public void DrawInspector(Transform target)
        {
            Transform t = (Transform)target;
            //if we're only editing 1 object, otherwise we need to display an error message since multi-object
            //editing isn't currently supported by this extension.
            if (Selection.transforms.Length < 2)
            {
                // Replicate the standard transform inspector gui if the component isn't part of Orthello       
                if (t.gameObject.GetComponent("OTSprite"))
                {
                    var orthelloSpriteType = TransformInspectorUtility.GetType("OTSprite");
                    var orthelloSprite = t.gameObject.GetComponent("OTSprite");

                    //Get the material, so we can make sure it isn't null before preceding.
                    MethodInfo orthello_GetMat = orthelloSpriteType.GetMethod("GetMat", Type.EmptyTypes);
                    object GetMat = orthello_GetMat.Invoke(orthelloSprite, orthello_GetMat.GetParameters());

                    //Try to check if the Orthello sprite object is valid
                    if (orthelloSprite != null) // && GetMat != null) //we don't need to stop if the material is null
                    {
                        DrawSnappingFoldout(t);

                        u2dexGUIUtil.Unity4Space();

                        //We only need 2 vectors (X and Y) for Orthello.  No need to show the Z value.
                        Vector3 position = EditorGUILayout.Vector2Field("Position", new Vector2(t.localPosition.x, t.localPosition.y));

                        u2dexGUIUtil.Unity4Space();

                        //Again, we only need X and Y for scale.
                        Vector2 scale = EditorGUILayout.Vector2Field("Size",
                            new Vector2(TransformInspectorUtility.GetScaleFromClassName("OTSprite", "size", t).x,
                            TransformInspectorUtility.GetScaleFromClassName("OTSprite", "size", t).y));

                        u2dexGUIUtil.Unity4Space();

                        DrawRotationControls(t);

                        //Leave some vertical space between areas!
                        u2dexGUIUtil.Unity3EditorGUILayoutSpace();

                        DrawLayerAndDepthControls(t);

                        //allow the Z Depth to be set
                        position.z = (float)EditorGUILayout.IntField("Z Depth", (int)t.localPosition.z);

                        EditorGUI.indentLevel = 0;

                        //Leave some vertical space between areas!
                        EditorGUILayout.Space();

                        if (GetMat == null)
                        {
                            EditorGUILayout.LabelField("Note:  The material on your Orthello sprite is currently null.",
                                new GUIStyle(EditorStyles.label) { fontStyle = FontStyle.Italic, wordWrap = true });
                        }

                        if (GUI.changed)
                        {
                            //if we're on a supported version of Unity that's older than 4.3
#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2
                            Undo.RegisterUndo(t, "Transform Change");
#else
                            Undo.RecordObject(t, "Transform Change");
#endif
                            t.localPosition = this.FixIfNaN(position);
                            t.localEulerAngles = this.FixIfNaN(EulerAngles);

                            //Check if the scale is NaN
                            var orthelloScale = new Vector3(scale.x, scale.y, 0);
                            orthelloScale = this.FixIfNaN(orthelloScale);

                            //Then copy it back
                            //orthelloSprite.size = new Vector2(orthelloScale.x, orthelloScale.y);
                            TransformInspectorUtility.SetScaleFromClassName("OTSprite", "size", t, orthelloScale);

                            //Retrieve the layer by name, and then set it.
                            orthelloSprite.gameObject.layer = LayerMask.NameToLayer(GetSortedLayer());

                            //copy our changed sprite back to our target.
                            EditorUtility.SetDirty(orthelloSprite);
                        }

                        //if we're on a supported version of Unity that's older than 4.3
#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2
                        EditorGUIUtility.LookLikeControls();
#endif
                    }
                }
            }
            else
            {
                EditorGUILayout.LabelField("Multi-object editing is not supported at this time.");
            }
        }

        /// <summary>
        /// Checks all the float components of a Vector3, and fixes them if they're NaN (Not a Number)
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
