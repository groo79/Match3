using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System;
using System.Reflection;

namespace u2dex
{
    /// <summary>
    /// The officially supported 2D Transform Inspector for Unity sprites.
    /// </summary>
    public class unitySpriteTransformInspector : TransformInspector2D
    {
        public void DrawInspector(Transform target)
        {
            Transform t = (Transform)target;
            //if we're only editing 1 object, otherwise we need to display an error message since multi-object
            //editing isn't currently supported by this extension.
            if (Selection.transforms.Length < 2)
            {
                // Replicate the standard transform inspector gui if the component isn't a Unity 4.3 sprite       
                if (t.gameObject.GetComponent("SpriteRenderer"))
                {
                    //var unitySpriteType = TransformInspectorUtility.GetType("SpriteRenderer");
                    var unitySprite = t.gameObject.GetComponent("SpriteRenderer");

                    //Try to check if the 2DToolkit sprite object is valid
                    if (unitySprite != null)
                    {
                        DrawSnappingFoldout(t);

                        u2dexGUIUtil.Unity4Space();

                        //We only need 2 vectors (X and Y) for 2DToolkit.  No need to show the Z value.
                        Vector3 position = EditorGUILayout.Vector2Field("Position", new Vector2(t.localPosition.x, t.localPosition.y));

                        u2dexGUIUtil.Unity4Space();

                        //Again, we only need X and Y for scale.
                        //Vector2 scale = EditorGUILayout.Vector2Field("Size",
                        //new Vector2(TransformInspectorUtility.GetScaleFromClassName("Sprite", "scale", t).x,
                        //TransformInspectorUtility.GetScaleFromClassName("Sprite", "scale", t).y));

                        //Note: Do Unity sprite just use the Transform's scale?
                        Vector2 scale = EditorGUILayout.Vector2Field("Size", new Vector2(t.localScale.x,
                                                                                        t.localScale.y));

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
                            var spriteScale = new Vector3(scale.x, scale.y, 0);
                            spriteScale = this.FixIfNaN(spriteScale);

                            t.localScale = spriteScale;
                            //Then copy it back
                            //tk2dSprite.scale = new Vector2(tk2DScale.x, tk2DScale.y);
                            //TransformInspectorUtility.SetScaleFromClassName("SpriteRenderer", "scale", t, tk2DScale);


                            //Retrieve the layer by name, and then set it.
                            unitySprite.gameObject.layer = LayerMask.NameToLayer(GetSortedLayer());

                            //copy our changed sprite back to our target.
                            EditorUtility.SetDirty(unitySprite);
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
