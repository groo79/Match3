using UnityEngine;
using UnityEditor;
using System.Collections;

namespace UnfinityGames.U2DEX
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

            if (GUI.changed)
            {
                Undo.RecordObject(t, "Transform Change");

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
