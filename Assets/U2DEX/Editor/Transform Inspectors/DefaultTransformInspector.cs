using UnityEngine;
using UnityEditor;
using System.Collections;

namespace u2dex
{
    /// <summary>
    /// The default Transform Inspector (untouched, and exactly how Unity does it by default).
    /// </summary>
    public static class DefaultTransformInspector
    {
        /// <summary>
        /// Draws the Inspector
        /// </summary>
        /// <param name="Target"></param>
        public static void Draw(Transform Target)
        {
            Transform t = Target;

            // Replicate the standard transform inspector gui
            EditorGUIUtility.LookLikeControls();
            EditorGUI.indentLevel = 0;
            Vector3 position = EditorGUILayout.Vector3Field("Position", t.localPosition);
            Vector3 eulerAngles = EditorGUILayout.Vector3Field("Rotation", t.localEulerAngles);
            Vector3 scale = EditorGUILayout.Vector3Field("Scale", t.localScale);

            //if we're on a supported version of Unity that's older than 4.3
#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2
            EditorGUIUtility.LookLikeInspector();
#endif

            if (GUI.changed)
            {
                //if we're on a supported version of Unity that's older than 4.3
#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2
                        Undo.RegisterUndo(t, "Transform Change");
#else
                Undo.RecordObject(t, "Transform Change");
#endif

                t.localPosition = FixIfNaN(position);
                t.localEulerAngles = FixIfNaN(eulerAngles);
                t.localScale = FixIfNaN(scale);
            }

            //copy the changed data back into the passed in Target variable
            Target = t;
        }

        /// <summary>
        /// Checks all the float components of a Vector3, and fixes them if they're NaN (Not a Number)
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
