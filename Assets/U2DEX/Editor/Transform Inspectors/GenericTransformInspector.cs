using UnityEngine;
using UnityEditor;
using System.Collections;

namespace u2dex
{
    /// <summary>
    /// The Generic 2D Transform Inspector.  Used on any of the classes added that aren't officially supported.
    /// Does not have the Scale/Size field in the Inspector, as we can't be sure that all added classes have a scale/size. 
    /// (Not to mention how to handle them if they do!)
    /// </summary>
    public class GenericTransformInspector : TransformInspector2D
    {
        public void DrawInspector(Transform target)
        {
            Transform t = (Transform)target;
            //if we're only editing 1 object, otherwise we need to display an error message since multi-object
            //editing isn't currently supported by this extension.
            if (Selection.transforms.Length < 2)
            {
                //Try to check if the generic object is valid
                if (target != null)
                {
                    DrawSnappingFoldout(t);

                    u2dexGUIUtil.Unity4Space();

                    //We only need 2 vectors (X and Y) for 2DToolkit.  No need to show the Z value.
                    Vector3 position = EditorGUILayout.Vector2Field("Position", new Vector2(t.localPosition.x, t.localPosition.y));

                    u2dexGUIUtil.Unity4Space();

                    //We can't show scale, since we don't know if this object even HAS scale...
                    //Vector2 scale = EditorGUILayout.Vector2Field("Size", new Vector2(tk2dSprite.scale.x, tk2dSprite.scale.y));

                    DrawRotationControls(t);

                    u2dexGUIUtil.Unity4Space();

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
                        //var tk2DScale = new Vector3(scale.x, scale.y, 0);
                        //tk2DScale = this.FixIfNaN(tk2DScale);

                        //Then copy it back
                        //tk2dSprite.scale = new Vector2(tk2DScale.x, tk2DScale.y);

                        //Retrieve the layer by name, and then set it.
                        target.gameObject.layer = LayerMask.NameToLayer(GetSortedLayer());

                        //copy our changed sprite back to our target.
                        EditorUtility.SetDirty(target);
                    }

                    //if we're on a supported version of Unity that's older than 4.3
#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2
                    EditorGUIUtility.LookLikeControls();
#endif
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
